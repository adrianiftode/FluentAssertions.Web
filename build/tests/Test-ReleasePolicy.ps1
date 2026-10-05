<#
.SYNOPSIS
    Tests build/Test-ReleasePolicy.ps1 against synthetic git history.

.DESCRIPTION
    This script decides whether a tag may publish, and it decides that by asking
    git which branches contain the tagged commit. Getting it wrong is expensive in
    both directions: too strict and a legitimate release never ships, too lax and a
    stable version number is burned on a branch nobody merged yet.

    The cases build a throwaway repo with a release branch and a feature branch, so
    they exercise the real git calls without touching the real history.

    Run from the repo root:
        ./build/tests/Test-ReleasePolicy.ps1
#>
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$scriptDir = $PSScriptRoot
$repoRoot = Split-Path -Parent (Split-Path -Parent $scriptDir)
$workDir = Join-Path ([System.IO.Path]::GetTempPath()) "faw-release-policy-$PID"

$script:failures = 0
$script:currentTag = $null

function Write-CaseResult($name, $ok, $expected, $actual) {
    if ($ok) {
        Write-Host "PASS  $name" -ForegroundColor Green
    }
    else {
        Write-Host "FAIL  $name" -ForegroundColor Red
        Write-Host "      expected: $expected" -ForegroundColor Red
        Write-Host "      actual:   $actual" -ForegroundColor Red
        $script:failures++
    }
}

function New-Fixture {
    if (Test-Path -LiteralPath $workDir) {
        Remove-Item -LiteralPath $workDir -Recurse -Force
    }

    New-Item -ItemType Directory -Path (Join-Path $workDir 'build') -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $workDir 'eng') -Force | Out-Null

    Copy-Item -LiteralPath (Join-Path $repoRoot 'build/Test-ReleasePolicy.ps1') `
              -Destination (Join-Path $workDir 'build') -Force
    Copy-Item -LiteralPath (Join-Path $repoRoot 'eng/ReleasePackages.props') `
              -Destination (Join-Path $workDir 'eng') -Force

    [xml] $manifest = Get-Content -LiteralPath (Join-Path $workDir 'eng/ReleasePackages.props') -Raw

    foreach ($package in $manifest.Project.ItemGroup.ReleasePackage) {
        $full = Join-Path $workDir $package.ProjectPath.Replace('\', '/')
        New-Item -ItemType Directory -Path (Split-Path -Parent $full) -Force | Out-Null
        Set-Content -LiteralPath $full -NoNewline -Value @"
<Project>
  <PropertyGroup>
    <Version>2.0.4</Version>
  </PropertyGroup>
</Project>
"@
    }

    Push-Location $workDir
    try {
        # An orphan default branch avoids inheriting this repository's real branches.
        & git init -q -b master . 2>$null | Out-Null
        & git config user.email 'release-tests@example.com'
        & git config user.name 'Release Tests'
        & git config core.autocrlf false
        & git config core.safecrlf false
        & git add -A 2>$null | Out-Null
        & git commit -q -m 'initial' 2>$null | Out-Null
    }
    finally {
        Pop-Location
    }
}

function Set-PackageVersion($id, $version) {
    [xml] $manifest = Get-Content -LiteralPath (Join-Path $workDir 'eng/ReleasePackages.props') -Raw

    $node = $manifest.Project.ItemGroup.ReleasePackage | Where-Object { $_.Include -eq $id }
    $csproj = Join-Path $workDir $node.ProjectPath.Replace('\', '/')

    (Get-Content -LiteralPath $csproj -Raw) -replace '<Version>.*</Version>', "<Version>$version</Version>" |
        Set-Content -LiteralPath $csproj -NoNewline
}

function New-TagOnBranch($branch, $tagName, [switch] $Checkout) {
    Push-Location $workDir
    try {
        if (-not (git branch --list $branch)) {
            & git checkout -q -b $branch master 2>$null | Out-Null
        }
        else {
            & git checkout -q $branch 2>$null | Out-Null
        }

        & git add -A 2>$null | Out-Null
        & git commit -q -m "work on $branch" 2>$null | Out-Null
        & git tag -f $tagName 2>$null | Out-Null

        if (-not $Checkout) {
            & git checkout -q master 2>$null | Out-Null
        }
    }
    finally {
        Pop-Location
    }
}

function Invoke-PolicyCase($name, $tagName, $packages, $expectedAllowed) {
    $selectedFile = Join-Path $workDir 'selected-packages.txt'
    $packages | Set-Content -LiteralPath $selectedFile -Encoding UTF8

    $env:APPVEYOR_REPO_TAG_NAME = $tagName
    try {
        & (Join-Path $workDir 'build/Test-ReleasePolicy.ps1') `
            -SelectedPackagesFile $selectedFile `
            -RepoRoot $workDir 2>&1 | Out-Null

        $allowed = $env:PUBLISH_ALLOWED
    }
    finally {
        $env:APPVEYOR_REPO_TAG_NAME = $null
    }

    Write-CaseResult $name ($allowed -eq $expectedAllowed) "PUBLISH_ALLOWED=$expectedAllowed" "PUBLISH_ALLOWED=$allowed"
}

try {
    New-Fixture

    # The normal case: a tag on the release branch with stable versions.
    New-TagOnBranch -Branch 'master' -TagName '2.0.5'
    Set-PackageVersion 'HttpMessageFormatter' '2.0.5'
    Invoke-PolicyCase 'stable version tagged on master publishes' `
        '2.0.5' @('HttpMessageFormatter') 'true'

    # A release branch other than master, which this repo uses for the v8 line.
    New-TagOnBranch -Branch 'master-v8' -TagName '2.0.5-v8'
    Set-PackageVersion 'HttpMessageFormatter' '2.0.5'
    Invoke-PolicyCase 'stable version tagged on master-v8 publishes' `
        '2.0.5-v8' @('HttpMessageFormatter') 'true'

    # The case this policy exists for. nuget.org packages are immutable, so a
    # stable version published from an unmerged branch can never be reused.
    New-TagOnBranch -Branch 'features/preview' -TagName '2.0.5-preview-branch'
    Set-PackageVersion 'HttpMessageFormatter' '2.0.5'
    Invoke-PolicyCase 'stable version tagged on a feature branch is blocked' `
        '2.0.5-preview-branch' @('HttpMessageFormatter') 'false'

    # The supported way to share a fix before merging: a prerelease version.
    Set-PackageVersion 'HttpMessageFormatter' '2.0.5-preview.1'
    Invoke-PolicyCase 'prerelease tagged on a feature branch publishes' `
        '2.0.5-preview-branch' @('HttpMessageFormatter') 'true'

    # A release mixing a prerelease and a stable version is still blocked, because
    # the stable one would be burned.
    Set-PackageVersion 'HttpMessageFormatter' '2.0.5'
    Set-PackageVersion 'FluentAssertions.Web' '2.0.5-preview.1'
    Invoke-PolicyCase 'release mixing prerelease and stable is blocked' `
        '2.0.5-preview-branch' @('HttpMessageFormatter', 'FluentAssertions.Web') 'false'

    # Nothing selected means nothing publishes, so there is nothing to block.
    Invoke-PolicyCase 'empty release does not block' `
        '2.0.5-preview-branch' @() 'false'

    # -preview, -beta and -rc are all prereleases to NuGet; plain 2.0.5 is not.
    foreach ($version in @('2.0.5-preview.1', '2.0.5-beta', '2.0.5-rc1')) {
        Set-PackageVersion 'HttpMessageFormatter' $version
        Invoke-PolicyCase "'$version' counts as a prerelease" `
            '2.0.5-preview-branch' @('HttpMessageFormatter') 'true'
    }

    foreach ($version in @('2.0.5', '2.0.5.1', '10.0.0')) {
        Set-PackageVersion 'HttpMessageFormatter' $version
        Invoke-PolicyCase "'$version' counts as stable" `
            '2.0.5-preview-branch' @('HttpMessageFormatter') 'false'
    }
}
finally {
    if (Test-Path -LiteralPath $workDir) {
        Remove-Item -LiteralPath $workDir -Recurse -Force -ErrorAction SilentlyContinue
    }
}

Write-Host ''
if ($script:failures -gt 0) {
    Write-Host "$script:failures case(s) failed." -ForegroundColor Red
    exit 1
}

Write-Host 'All cases passed.' -ForegroundColor Green

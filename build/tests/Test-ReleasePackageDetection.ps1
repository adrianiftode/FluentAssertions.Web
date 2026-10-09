<#
.SYNOPSIS
    Tests build/Get-ReleasePackages.ps1 against synthetic git history.

.DESCRIPTION
    Change detection is the part of the release pipeline that is easy to get
    subtly wrong, and a wrong answer either drops a package that needed shipping
    or needlessly republishes ones that did not change. These cases run against a
    throwaway git repo so they exercise the real script, including its git calls,
    without touching the real history or requiring a network.

    Run from the repo root:
        ./build/tests/Test-ReleasePackageDetection.ps1
#>
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$scriptDir = $PSScriptRoot
$repoRoot = Split-Path -Parent (Split-Path -Parent $scriptDir)
$workDir = Join-Path ([System.IO.Path]::GetTempPath()) "faw-release-detection-$PID"

$script:failures = 0

function Write-CaseResult($name, $ok, $expected, $actual) {
    if ($ok) {
        Write-Host "PASS  $name" -ForegroundColor Green
    }
    else {
        Write-Host "FAIL  $name" -ForegroundColor Red
        Write-Host "      expected: $($expected -join ', ')" -ForegroundColor Red
        Write-Host "      actual:   $($actual -join ', ')" -ForegroundColor Red
        $script:failures++
    }
}

function New-Fixture {
    <#
        Builds a minimal repo containing the real manifest and the real detection
        script, so the cases test shipped behaviour rather than a copy of it.
    #>
    if (Test-Path -LiteralPath $workDir) {
        Remove-Item -LiteralPath $workDir -Recurse -Force
    }

    New-Item -ItemType Directory -Path (Join-Path $workDir 'build') -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $workDir 'eng') -Force | Out-Null

    Copy-Item -LiteralPath (Join-Path $repoRoot 'build/Get-ReleasePackages.ps1') `
              -Destination (Join-Path $workDir 'build') -Force
    Copy-Item -LiteralPath (Join-Path $repoRoot 'eng/ReleasePackages.props') `
              -Destination (Join-Path $workDir 'eng') -Force

    # One stub csproj per manifest entry, each with the same version the real
    # packages have, so the version check passes.
    [xml] $manifest = Get-Content -LiteralPath (Join-Path $workDir 'eng/ReleasePackages.props') -Raw
    $watchDirectories = [System.Collections.Generic.HashSet[string]]::new()

    foreach ($package in $manifest.Project.ItemGroup.ReleasePackage) {
        $projectPath = $package.ProjectPath.Replace('\', '/')

        $full = Join-Path $workDir $projectPath
        New-Item -ItemType Directory -Path (Split-Path -Parent $full) -Force | Out-Null
        Set-Content -LiteralPath $full -NoNewline -Value @"
<Project>
  <PropertyGroup>
    <Version>2.0.4</Version>
  </PropertyGroup>
</Project>
"@

        foreach ($dir in ($package.WatchDirectories -split ';' | Where-Object { $_ })) {
            [void] $watchDirectories.Add($dir.Trim())
        }

        foreach ($file in ($package.WatchFiles -split ';' | Where-Object { $_ })) {
            $fullFile = Join-Path $workDir $file.Trim()
            if (-not (Test-Path -LiteralPath $fullFile)) {
                New-Item -ItemType Directory -Path (Split-Path -Parent $fullFile) -Force | Out-Null
                Set-Content -LiteralPath $fullFile -Value "placeholder"
            }
        }
    }

    foreach ($dir in $watchDirectories) {
        $full = Join-Path $workDir $dir
        New-Item -ItemType Directory -Path $full -Force | Out-Null
        Set-Content -LiteralPath (Join-Path $full '.keep') -Value ''
    }

    Push-Location $workDir
    try {
        $gitQuiet = @('-c', 'core.autocrlf=false', '-c', 'core.safecrlf=false')
        & git init -q . 2>$null | Out-Null
        & git config user.email 'release-tests@example.com'
        & git config user.name 'Release Tests'
        & git @gitQuiet add -A 2>$null | Out-Null
        & git @gitQuiet commit -q -m 'initial' 2>$null | Out-Null
        & git tag '2.0.4'
    }
    finally {
        Pop-Location
    }
}

function Invoke-Case($name, $changedFiles, $expected, [string] $Baseline, [switch] $RequireFile) {
    <#
        Commits the given files on top of the baseline tag, runs detection, and
        compares the packages it selects against the expected set. The working
        tree is reset first so cases stay independent of each other.

        -Baseline is forwarded to the detection script so a case can probe how it
        behaves when the baseline ref is unusable. A throw from the script is
        captured and reported as this case's own failure instead of aborting the
        rest of the run, because these cases exist precisely to pin down what
        happens when a git call goes wrong.

        -RequireFile additionally asserts that the selection file was created even
        when the expected selection is empty. Pack-ReleasePackages.ps1 reads that
        file unconditionally, so creating it is part of the contract; without this
        switch an empty expected set passes whether or not the file exists.
    #>
    Push-Location $workDir
    try {
        & git reset -q --hard 2.0.4 2>$null | Out-Null
        & git clean -qfd 2>$null | Out-Null

        foreach ($file in $changedFiles) {
            $full = Join-Path $workDir $file
            New-Item -ItemType Directory -Path (Split-Path -Parent $full) -Force | Out-Null
            if (Test-Path -LiteralPath $full) {
                Add-Content -LiteralPath $full -Value 'x'
            }
            else {
                Set-Content -LiteralPath $full -Value 'x'
            }
        }

        if ($changedFiles) {
            & git -c core.autocrlf=false -c core.safecrlf=false add -A 2>$null | Out-Null
            & git -c core.autocrlf=false -c core.safecrlf=false commit -q -m $name 2>$null | Out-Null
        }

        $selectedFile = Join-Path $workDir 'selected-packages.txt'

        $detectParams = @{
            RepoRoot            = $workDir
            SkipVersionCheck    = $true
            ChangedPackagesFile = $selectedFile
        }
        if ($Baseline) {
            $detectParams['Baseline'] = $Baseline
        }

        $thrown = ''
        try {
            & (Join-Path $workDir 'build/Get-ReleasePackages.ps1') @detectParams 2>$null | Out-Null
        }
        catch {
            $thrown = $_.Exception.Message
        }

        $actual = @()
        if (Test-Path -LiteralPath $selectedFile) {
            $actual = @(Get-Content -LiteralPath $selectedFile | Where-Object { $_ })
        }
        elseif ($RequireFile -and -not $thrown) {
            # The script completed but wrote no manifest. That is a failure on its
            # own: Pack-ReleasePackages.ps1 reads this file unconditionally.
            $actual = @('(selection file was not created)')
        }

        if ($thrown -and -not $actual) {
            $actual = @("(detection script threw: $thrown)")
        }
    }
    finally {
        Pop-Location
    }

    Write-CaseResult $name ($null -eq (Compare-Object $actual @($expected))) $expected $actual
}

function Invoke-NoGitCase($name, $expected) {
    <#
        Simulates a checkout without any git metadata, which is exactly what
        AppVeyor's zip download used to hand the build: no .git directory, so every
        git call fails with "fatal: not a git repository". Detection has neither a
        baseline nor a diff in that state, and the only safe answer is every
        package rather than none. The .git directory is parked and restored so the
        case can run anywhere in the list.
    #>
    $gitDir = Join-Path $workDir '.git'
    $parkedGitDir = Join-Path $workDir '.git-parked'

    Push-Location $workDir
    try {
        & git reset -q --hard 2.0.4 2>$null | Out-Null
        & git clean -qfd 2>$null | Out-Null

        Move-Item -LiteralPath $gitDir -Destination $parkedGitDir -Force

        $selectedFile = Join-Path $workDir 'selected-packages.txt'

        $thrown = ''
        try {
            & (Join-Path $workDir 'build/Get-ReleasePackages.ps1') `
                -RepoRoot $workDir `
                -SkipVersionCheck `
                -ChangedPackagesFile $selectedFile 2>$null | Out-Null
        }
        catch {
            $thrown = $_.Exception.Message
        }

        $actual = @()
        if (Test-Path -LiteralPath $selectedFile) {
            $actual = @(Get-Content -LiteralPath $selectedFile | Where-Object { $_ })
        }

        if ($thrown -and -not $actual) {
            $actual = @("(detection script threw: $thrown)")
        }
    }
    finally {
        if ((Test-Path -LiteralPath $parkedGitDir) -and -not (Test-Path -LiteralPath $gitDir)) {
            Move-Item -LiteralPath $parkedGitDir -Destination $gitDir -Force
        }
        Pop-Location
    }

    Write-CaseResult $name ($null -eq (Compare-Object $actual @($expected))) $expected $actual
}

function Invoke-RejectionCase($name, $mutate, $expectedFragment) {
    <#
        Runs detection against a manifest or project that has been deliberately
        broken, and checks it refuses rather than silently shipping something wrong.
        Offline: these cases rely on -SkipVersionCheck, so they never call nuget.org.
    #>
    Push-Location $workDir
    try {
        & git reset -q --hard 2.0.4 2>$null | Out-Null
        & git clean -qfd 2>$null | Out-Null

        & $mutate

        $failed = $false
        $message = ''
        try {
            & (Join-Path $workDir 'build/Get-ReleasePackages.ps1') `
                -RepoRoot $workDir `
                -SkipVersionCheck `
                -IncludeUnchanged 2>&1 | ForEach-Object { $message += "$_`n" }
        }
        catch {
            $failed = $true
            $message = $_.Exception.Message
        }

        Write-CaseResult $name ($failed -and ($message -like "*$expectedFragment*")) `
            "failure containing '$expectedFragment'" $(if ($message) { $message.Trim() } else { '(no error raised)' })
    }
    finally {
        Pop-Location
    }
}

try {
    New-Fixture

    # Every package in the manifest, for the cases where detection has no usable
    # change information: guessing "nothing changed" would silently drop a release.
    $allPackages = @(
        'HttpMessageFormatter',
        'FluentAssertions.Web', 'FluentAssertions.Web.v8', 'AwesomeAssertions.Web',
        'Assertions.Web.Serializers.NewtonsoftJson',
        'Shouldly.Web')

    # A serializer has no dependency on the assertion packages, so changing one
    # must publish exactly one package. This is the case the old lockstep
    # pipeline got wrong.
    Invoke-Case 'serializer change publishes only that serializer' `
        @('src/Assertions.Web.Serializers.NewtonsoftJson/Serializer.cs') `
        @('Assertions.Web.Serializers.NewtonsoftJson')

    # The three flavours link-compile src/FluentAssertions.Web, so one edit there
    # affects all of them and nothing else.
    Invoke-Case 'shared assertion source publishes the three flavours' `
        @('src/FluentAssertions.Web/HttpStatusCodeAssertions.cs') `
        @('FluentAssertions.Web', 'FluentAssertions.Web.v8', 'AwesomeAssertions.Web')

    # HttpMessageFormatter is a real NuGet dependency of the three assertion
    # packages, so it must never ship without them.
    Invoke-Case 'formatter change pulls in its dependents' `
        @('src/HttpMessageFormatter/Internal/Formatter.cs') `
        @('HttpMessageFormatter', 'FluentAssertions.Web', 'FluentAssertions.Web.v8', 'AwesomeAssertions.Web', 'Shouldly.Web')

    # The repo readme is no longer embedded by any package, and neither is the
    # shared Assertions.Web documentation, so doc edits publish nothing.
    Invoke-Case 'repo readme change publishes nothing' `
        @('readme.md') `
        @()

    # Each package packs its own Readme.md, so a local readme edit republishes
    # that package only.
    Invoke-Case 'package readme change publishes that package' `
        @('src/Shouldly.Web/Readme.md') `
        @('Shouldly.Web')

    # A dependency bump changes what ships in every package.
    Invoke-Case 'dependency bump publishes everything' `
        @('Directory.Packages.props') `
        @('FluentAssertions.Web', 'FluentAssertions.Web.v8', 'AwesomeAssertions.Web',
          'HttpMessageFormatter',
          'Assertions.Web.Serializers.NewtonsoftJson',
          'Shouldly.Web')

    # Docs and CI config are in nobody's package.
    Invoke-Case 'docs change publishes nothing' `
        @('CONVENTIONS.md', 'docs/Assertions.Web/Readme.md') `
        @()

    # A diff that succeeds but lists nothing means there is genuinely nothing new,
    # which is not the same as not being able to read the diff. Nothing should ship,
    # but the selection file must still be created (empty): Pack-ReleasePackages.ps1
    # reads it unconditionally. A branch build whose HEAD is the release tag resolves
    # the baseline to that same tag, so it takes exactly this path, and the missing
    # file used to kill the build with "Cannot find path ... release-packages.txt".
    Invoke-Case 'no changes since the baseline publishes nothing' @() @() -RequireFile

    # A baseline ref git cannot resolve (a tag fetch that failed, a stale ref) must
    # not be mistaken for "no changes": the change list is unknown, and the only
    # safe answer then is every package.
    Invoke-Case 'unreadable baseline selects every package' `
        @('CONVENTIONS.md') `
        $allPackages `
        -Baseline 'refs/tags/does-not-exist'

    # A csproj with no <Version> would pack as 1.0.0 and overwrite a published
    # package, so detection has to stop rather than pass it through.
    Invoke-RejectionCase 'project without a version is rejected' `
        {
            $csproj = Join-Path $workDir 'src/AwesomeAssertions.Web/AwesomeAssertions.Web.csproj'
            (Get-Content -LiteralPath $csproj -Raw) -replace '\s*<Version>.*</Version>', '' |
                Set-Content -LiteralPath $csproj -NoNewline
        } `
        'has no <Version>'

    # A DependsOn typo would otherwise drop the dependent from the release without
    # a word, so it has to fail too.
    Invoke-RejectionCase 'DependsOn pointing at an unknown package is rejected' `
        {
            $manifestFile = Join-Path $workDir 'eng/ReleasePackages.props'
            (Get-Content -LiteralPath $manifestFile -Raw) -replace 'DependsOn="HttpMessageFormatter"', 'DependsOn="HttpMessageFormatterr"' |
                Set-Content -LiteralPath $manifestFile -NoNewline
        } `
        'not in the release manifest'

    # A csproj that the manifest points at but that is not there would pack
    # whatever git resolved, so the path is verified up front.
    Invoke-RejectionCase 'manifest pointing at a missing project is rejected' `
        {
            $manifestFile = Join-Path $workDir 'eng/ReleasePackages.props'
            (Get-Content -LiteralPath $manifestFile -Raw) -replace 'src/HttpMessageFormatter/HttpMessageFormatter\.csproj', 'src/HttpMessageFormatter/DoesNotExist.csproj' |
                Set-Content -LiteralPath $manifestFile -NoNewline
        } `
        'does not exist'

    # The failure AppVeyor hit: a checkout handed to the build as a zip archive,
    # with no .git directory at all. Every git call fails there, and detection has
    # to degrade to publishing everything instead of killing the build.
    Invoke-NoGitCase 'checkout without git metadata selects every package' $allPackages
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
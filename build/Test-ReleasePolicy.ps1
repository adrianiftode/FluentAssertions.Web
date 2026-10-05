<#
.SYNOPSIS
    Decides whether a tag is allowed to publish, and exposes that decision to
    AppVeyor's deploy conditions.

.DESCRIPTION
    A tag on a feature branch is a legitimate way to try a release out, but it is
    almost never what anyone means for a stable version number. nuget.org packages
    are immutable, so publishing 2.0.5 from an unmerged tag burns that number
    permanently and the eventual merge cannot reuse it.

    So the rule is:

    - the tag's commit must be contained in a release branch (master, master-v8),
      OR
    - every selected version must be a NuGet prerelease, meaning it carries a
      suffix such as -preview.1 or -beta. Those are cheap to publish and cheap to
      supersede.

    Stable versions from unmerged tags are rejected. Prereleases from any branch
    pass, which is the supported way to share a fix before merging it.

    Results are published as environment variables because AppVeyor deploy
    conditions can only read environment variables:

    RELEASE_ON_RELEASE_BRANCH   true when the tag is on master or master-v8
    RELEASE_IS_PRERELEASE       true when every selected version has a suffix
    PUBLISH_ALLOWED             the two above ORed together
    RELEASE_BLOCKED_REASON      human readable explanation when PUBLISH_ALLOWED is false

.PARAMETER SelectedPackagesFile
    Package ids, one per line, as written by Get-ReleasePackages.ps1. Used to read
    the versions being published.

.PARAMETER ReleaseBranches
    Branches a stable release may come from. Defaults to master and master-v8.

.PARAMETER RepoRoot
    Repository root. Defaults to the parent of this script's folder.

.EXAMPLE
    ./Test-ReleasePolicy.ps1 -SelectedPackagesFile $selected
    Prints whether this tag may publish.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string] $SelectedPackagesFile,

    [string] $ReleaseBranches = 'master,master-v8',

    [string] $RepoRoot = (Split-Path -Parent $PSScriptRoot)
)

$ErrorActionPreference = 'Stop'

function Write-Step($message) {
    Write-Host "==> $message"
}

function Test-IsPrereleaseVersion([string] $version) {
    # NuGet treats anything after the first '-' as the prerelease label, so
    # 2.0.5-preview.1 and 2.0.5-rc are prereleases while 2.0.5 is not.
    return $version -match '-'
}

# Which branches contain the tagged commit. Remotes are included because a shallow
# CI clone has no local branch other than the one it checked out, so a local-only
# lookup would report master as missing on every build.
$tagName = $env:APPVEYOR_REPO_TAG_NAME

if (-not $tagName) {
    throw 'APPVEYOR_REPO_TAG_NAME is not set. This script only makes sense for a tag build.'
}

$tagRef = "refs/tags/$tagName"

$containingBranches = @(
    & git -C $RepoRoot branch -a --contains $tagRef --format='%(refname:short)' 2>$null |
        ForEach-Object { $_.Trim() } |
        Where-Object { $_ }
)

if ($LASTEXITCODE -ne 0) {
    throw "Could not determine which branches contain '$tagName'. The tag fetch may have failed."
}

# Only the branch name is compared, so origin/master and master both match.
$releaseBranchPattern = '(^|/)({0})$' -f (($ReleaseBranches -split ',' | ForEach-Object { $_.Trim() }) -join '|')

$onReleaseBranch = @($containingBranches | Where-Object { $_ -match $releaseBranchPattern }).Count -gt 0

Write-Step "Tag '$tagName' is contained in: $(if ($containingBranches) { $containingBranches -join ', ' } else { '(no known branch)' })"
Write-Step "On a release branch ($ReleaseBranches): $onReleaseBranch"

$selected = @(Get-Content -LiteralPath $SelectedPackagesFile | Where-Object { $_ })

# Versions come from the csprojs via the manifest, the same source the detection
# script validated, so this cannot disagree with what is about to be packed.
$manifestPath = Join-Path $RepoRoot 'eng/ReleasePackages.props'
[xml] $manifest = Get-Content -LiteralPath $manifestPath -Raw

$versions = foreach ($node in $manifest.Project.ItemGroup.ReleasePackage) {
    if ($selected -contains $node.Include) {
        $projectPath = Join-Path $RepoRoot $node.ProjectPath.Replace('\', '/')
        [xml] $projectXml = Get-Content -LiteralPath $projectPath -Raw

        $versionNode = $projectXml.Project.PropertyGroup.Version | Where-Object { $_ } | Select-Object -First 1
        $version = if ($versionNode -is [string]) { $versionNode } else { $versionNode.InnerText }

        [pscustomobject]@{
            Id      = $node.Include
            Version = $version.Trim()
        }
    }
}

if (-not $versions) {
    # Nothing selected means nothing publishes, so there is no policy to enforce.
    Write-Step 'No packages selected. Nothing to publish.'
    $isPrerelease = $false
    $allPrerelease = $true
}
else {
    $isPrerelease = @($versions | Where-Object { Test-IsPrereleaseVersion $_.Version }).Count -gt 0
    $allPrerelease = @($versions | Where-Object { -not (Test-IsPrereleaseVersion $_.Version) }).Count -eq 0

    Write-Step "Versions in this release: $(($versions | ForEach-Object { "$($_.Id) $($_.Version)" }) -join ', ')"
    Write-Step "All versions are prereleases: $allPrerelease"
}

$allowed = $onReleaseBranch -or $allPrerelease

$blockedReason = ''
if (-not $allowed) {
    $stable = @($versions | Where-Object { -not (Test-IsPrereleaseVersion $_.Version) } | ForEach-Object { "$($_.Id) $($_.Version)" })

    $blockedReason = "Tag '$tagName' is not on any release branch ($ReleaseBranches) and publishes stable versions: $($stable -join ', '). Merge the branch and tag again, or use a prerelease version such as 2.0.5-preview.1 to share this before merging."
}

$env:RELEASE_ON_RELEASE_BRANCH = if ($onReleaseBranch) { 'true' } else { 'false' }
$env:RELEASE_IS_PRERELEASE = if ($isPrerelease) { 'true' } else { 'false' }
$env:PUBLISH_ALLOWED = if ($allowed) { 'true' } else { 'false' }
$env:RELEASE_BLOCKED_REASON = $blockedReason

Write-Host "    RELEASE_ON_RELEASE_BRANCH = $env:RELEASE_ON_RELEASE_BRANCH"
Write-Host "    RELEASE_IS_PRERELEASE = $env:RELEASE_IS_PRERELEASE"
Write-Host "    PUBLISH_ALLOWED = $env:PUBLISH_ALLOWED"

if ($blockedReason) {
    Write-Host ''
    Write-Host "    NOT PUBLISHING: $blockedReason" -ForegroundColor Yellow
}

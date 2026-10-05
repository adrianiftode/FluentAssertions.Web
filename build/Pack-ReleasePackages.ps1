<#
.SYNOPSIS
    Packs only the packages selected for this release, and records which feeds
    they belong to.

.DESCRIPTION
    Reads the package ids chosen by Get-ReleasePackages.ps1 and packs exactly
    those projects. Every other package is skipped, so a release does not rebuild
    and republish packages that did not change.

    Writes a file listing the feeds that have work in this release, because
    AppVeyor's deploy conditions can only read environment variables.

.PARAMETER SelectedPackagesFile
    Package ids, one per line, as written by Get-ReleasePackages.ps1.

.PARAMETER Configuration
    Build configuration to pack.

.PARAMETER RepositoryUrl
    Sets the repository metadata embedded in the produced packages.

.PARAMETER PublishableFeedsFile
    Receives one line per feed that has at least one selected package, in the form
    `FeedName=<true|false>`.

.EXAMPLE
    ./Pack-ReleasePackages.ps1 -SelectedPackagesFile $selected -Configuration Release
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string] $SelectedPackagesFile,

    [string] $Configuration = 'Release',

    [Parameter(Mandatory)]
    [string] $RepositoryUrl,

    [string] $PublishableFeedsFile
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$manifestPath = Join-Path $repoRoot 'eng/ReleasePackages.props'

[xml] $manifest = Get-Content -LiteralPath $manifestPath -Raw

$packages = @{}
foreach ($node in $manifest.Project.ItemGroup.ReleasePackage) {
    $packages[$node.Include] = $node
}

$selected = @(Get-Content -LiteralPath $SelectedPackagesFile | Where-Object { $_ })

if (-not $selected) {
    Write-Host "==> No packages selected. Nothing to pack."
    $selected = @()
}

# Feed assignment mirrors the deploy section in appveyor.yml. FluentAssertions.Web
# is matched first because its glob also covers FluentAssertions.Web.v8 and the
# serializer package, exactly as the artifact globs do.
function Get-FeedFor($id) {
    if ($id -like 'AwesomeAssertions.Web*') { return 'AwesomeAssertions' }
    if ($id -like 'FluentAssertions.Web*') { return 'FluentAssertions' }
    if ($id -like 'HttpMessageFormatter*') { return 'HttpMessageFormatter' }
    return $null
}

$feeds = [System.Collections.Generic.HashSet[string]]::new()

foreach ($id in $selected) {
    if (-not $packages.ContainsKey($id)) {
        throw "'$id' is not in the release manifest. Run Get-ReleasePackages.ps1 to regenerate the selection."
    }

    $projectPath = $packages[$id].ProjectPath.Replace('\', '/')
    Write-Host "==> Packing $id from $projectPath"

    & dotnet pack $projectPath -c $Configuration --include-symbols --nologo `
        -p:RepositoryUrl=$RepositoryUrl -p:RepositoryType=git
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet pack failed for $id."
    }

    $feed = Get-FeedFor $id
    if (-not $feed) {
        throw "No NuGet feed is mapped for package '$id'. Add one to Get-FeedFor in this script and to appveyor.yml."
    }

    [void] $feeds.Add($feed)
}

# AppVeyor reads deploy conditions from environment variables, so each feed gets a
# flag. Feeds with nothing to publish get false, and their deploy step is skipped.
# Assigned explicitly rather than in a loop: AppVeyor reads these in the deploy
# section, and a plain $env: assignment is what makes the value visible to the
# later steps of the build.
$env:PUBLISH_FLUENTASSERTIONS     = $(if ($feeds.Contains('FluentAssertions'))     { 'true' } else { 'false' })
$env:PUBLISH_AWESOMEASSERTIONS    = $(if ($feeds.Contains('AwesomeAssertions'))    { 'true' } else { 'false' })
$env:PUBLISH_HTTPMESSAGEFORMATTER = $(if ($feeds.Contains('HttpMessageFormatter')) { 'true' } else { 'false' })

# Gates the GitHub release, so a tag that affects nothing does not create an
# empty release with no assets.
$env:PUBLISH_ANY = $(if ($feeds.Count -gt 0) { 'true' } else { 'false' })

$feedResults = [ordered]@{
    FluentAssertions     = $env:PUBLISH_FLUENTASSERTIONS
    AwesomeAssertions    = $env:PUBLISH_AWESOMEASSERTIONS
    HttpMessageFormatter = $env:PUBLISH_HTTPMESSAGEFORMATTER
}

foreach ($feed in $feedResults.Keys) {
    Write-Host "    PUBLISH_$($feed.ToUpperInvariant()) = $($feedResults[$feed])"
}

Write-Host "    PUBLISH_ANY = $env:PUBLISH_ANY"

if ($PublishableFeedsFile) {
    $feedResults.GetEnumerator() | ForEach-Object { "$($_.Key)=$($_.Value)" } |
        Set-Content -LiteralPath $PublishableFeedsFile -Encoding UTF8
    Write-Host "==> Wrote feed decisions to '$PublishableFeedsFile'."
}
<#
.SYNOPSIS
    Decides which NuGet packages a release should publish.

.DESCRIPTION
    Reads eng/ReleasePackages.props, works out which packages are affected by the
    commits since the previous release tag, and prints the ones to publish.

    A package is affected when a changed file sits under one of its
    WatchDirectories or matches one of its WatchFiles. Affected packages also pull
    in everything that DependsOn them, so a dependency is never published without
    its dependents.

    Packages that are not affected are skipped, which is the whole point: a change
    to one package no longer forces a new version of every other package.

.PARAMETER Baseline
    Git ref to diff against. Defaults to the most recent tag reachable from HEAD,
    excluding the tag being built, or the merge base with the default branch when
    the repository has no tags yet.

.PARAMETER RepoRoot
    Repository root. Defaults to the parent of this script's folder.

.PARAMETER EndRef
    Git ref for the tip of the range. Defaults to HEAD. Exists so a historical
    range can be replayed to verify detection, e.g.
    `-Baseline 2.0.3 -EndRef 2.0.4`.

.PARAMETER ChangedPackagesFile
    Writes the selected package ids, one per line, to this path. Used by CI to
    decide what to pack and deploy.

.PARAMETER IncludeUnchanged
    Emit every package in the manifest, ignoring change detection. Useful for
    local verification and for a first release after adopting this script.

.PARAMETER SkipVersionCheck
    Skip the nuget.org check for already published versions.

.PARAMETER EnforceVersionCheck
    Fail when a selected package's version is already on nuget.org. CI passes this
    on tag builds only, so an ordinary branch build does not fail just because a
    changed package has not been versioned yet.

.EXAMPLE
    ./Get-ReleasePackages.ps1
    Prints the packages to publish for the current commit.
#>
[CmdletBinding()]
param(
    [string] $Baseline,
    [string] $EndRef = 'HEAD',
    [string] $RepoRoot = (Split-Path -Parent $PSScriptRoot),
    [string] $ChangedPackagesFile,
    [switch] $IncludeUnchanged,
    [switch] $SkipVersionCheck,
    [switch] $EnforceVersionCheck
)

$ErrorActionPreference = 'Stop'

$manifestPath = Join-Path $RepoRoot 'eng/ReleasePackages.props'

function Write-Step($message) {
    Write-Host "==> $message"
}

# Runs git and reports the result as data instead of as an exception.
#
# Under $ErrorActionPreference = 'Stop' a native command's stderr becomes a
# terminating error the moment it is redirected (2>$null or 2>&1), so the exit
# code fallbacks below would never be reached and every git failure would crash
# the script. That is exactly what happened on CI when the checkout had no .git
# directory at all. Running git with the preference temporarily relaxed and
# collecting both streams keeps a failed git call something this script can
# recover from: the exit code and the first line of output come back in an object.
function Invoke-Git {
    param(
        [Parameter(Mandatory, Position = 0)]
        [string[]] $Arguments
    )

    if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
        return [pscustomobject]@{
            Output   = @()
            ExitCode = -1
            Reason   = 'git is not installed or not on PATH.'
        }
    }

    $previousEap = $ErrorActionPreference
    $ErrorActionPreference = 'Continue'
    try {
        $output = @(& git @Arguments 2>&1 | ForEach-Object { "$_" })
        $exitCode = $LASTEXITCODE
    }
    finally {
        $ErrorActionPreference = $previousEap
    }

    return [pscustomobject]@{
        Output   = $output
        ExitCode = $exitCode
        Reason   = $(if ($output) { @($output)[0] } else { '' })
    }
}

function Get-ManifestPackages {
    if (-not (Test-Path -LiteralPath $manifestPath)) {
        throw "Release manifest not found at '$manifestPath'."
    }

    [xml] $xml = Get-Content -LiteralPath $manifestPath -Raw

    $packages = foreach ($node in $xml.Project.ItemGroup.ReleasePackage) {
        $watchDirectories = @()
        if ($node.WatchDirectories) {
            $watchDirectories = $node.WatchDirectories -split ';' | Where-Object { $_ } | ForEach-Object { $_.Trim().Replace('\', '/') }
        }

        $watchFiles = @()
        if ($node.WatchFiles) {
            $watchFiles = $node.WatchFiles -split ';' | Where-Object { $_ } | ForEach-Object { $_.Trim().Replace('\', '/') }
        }

        $dependsOn = @()
        if ($node.DependsOn) {
            $dependsOn = $node.DependsOn -split ';' | Where-Object { $_ } | ForEach-Object { $_.Trim() }
        }

        [pscustomobject]@{
            Id               = $node.Include
            ProjectPath      = $node.ProjectPath.Replace('\', '/')
            WatchDirectories = $watchDirectories
            WatchFiles       = $watchFiles
            DependsOn        = $dependsOn
        }
    }

    if (-not $packages) {
        throw "No ReleasePackage entries found in '$manifestPath'."
    }

    return $packages
}

# Reading the version from the csproj XML rather than from MSBuild: MSBuild reports
# 1.0.0 for a project that never sets <Version>, so an absent element is only
# visible here.
function Get-PackageVersion($package) {
    $projectFullPath = Join-Path $RepoRoot $package.ProjectPath

    if (-not (Test-Path -LiteralPath $projectFullPath)) {
        throw "Package '$($package.Id)' points at '$($package.ProjectPath)', which does not exist."
    }

    [xml] $projectXml = Get-Content -LiteralPath $projectFullPath -Raw

    $versionNode = $projectXml.Project.PropertyGroup.Version | Where-Object { $_ } | Select-Object -First 1

    if (-not $versionNode) {
        throw "Package '$($package.Id)' has no <Version> in '$($package.ProjectPath)'. Without it the project would pack as 1.0.0 and overwrite the existing package."
    }

    # PowerShell's XML adapter returns a plain string for an element that only has
    # text, and an XmlElement when it has attributes or child nodes.
    $version = if ($versionNode -is [string]) { $versionNode } else { $versionNode.InnerText }

    return $version.Trim()
}

function Get-MergedTags {
    # Every tag reachable from HEAD, newest first. Git failing (no git installed,
    # no repository, unreadable refs) is treated as "this clone has no tags",
    # which sends the caller down the fallbacks rather than failing the build.
    $result = Invoke-Git @('-C', $RepoRoot, 'tag', '--sort=-creatordate', '--merged', 'HEAD')

    if ($result.ExitCode -ne 0) {
        Write-Step "Could not list release tags$(if ($result.Reason) { ": $($result.Reason)" }). Treating this clone as having none."
        return @()
    }

    return @($result.Output | Where-Object { $_ })
}

function Get-BaseLineRef {
    if ($Baseline) { return $Baseline }

    $script:currentTag = $env:APPVEYOR_REPO_TAG_NAME

    # The tag being built is HEAD itself, so it cannot be the diff baseline.
    $tags = @(Get-MergedTags)
    $tagRef = $tags | Select-Object -First 1

    if ($tagRef -and $script:currentTag -and $tagRef -eq $script:currentTag) {
        $tagRef = $tags | Select-Object -Skip 1 -First 1
    }

    if ($tagRef) {
        Write-Step "Diffing against previous release tag '$tagRef'."
        return $tagRef
    }

    $defaultBranch = $env:APPVEYOR_REPO_BRANCH
    if ($defaultBranch) {
        Write-Step "No release tag found. Diffing against origin/$defaultBranch."
        return "origin/$defaultBranch"
    }

    Write-Step "No release tag or baseline branch available. Treating everything as changed."
    return $null
}

function Get-ChangedFiles($baselineRef) {
    if (-not $baselineRef) { return $null }

    $gitArgs = @('-C', $RepoRoot, 'diff', '--name-only')
    if ($baselineRef -like 'origin/*') {
        # Diff against the merge base so commits already on the default branch are
        # not counted as part of this release.
        $gitArgs += '--merge-base'
    }
    $gitArgs += "$baselineRef...$EndRef"

    $result = Invoke-Git -Arguments $gitArgs

    if ($result.ExitCode -ne 0) {
        # No diff means the change list is unknown, not that nothing changed, so
        # the caller publishes every package rather than none.
        Write-Step "Could not diff against '$baselineRef'$(if ($result.Reason) { " ($($result.Reason))" }). Treating everything as changed."
        return $null
    }

    # The comma keeps an empty result an empty array instead of collapsing to
    # $null: a diff that succeeded but lists nothing means there is genuinely
    # nothing new, which must stay distinguishable from the failure above.
    return ,@($result.Output | Where-Object { $_ } | ForEach-Object { $_.Replace('\', '/') })
}

function Test-PackageAffected($package, $changedFiles) {
    if ($null -eq $changedFiles) { return $true }

    # Collect every reason rather than stopping at the first, so the log explains
    # the whole picture when a shared file like Directory.Packages.props matches
    # all six packages at once.
    $reasons = [System.Collections.Generic.List[string]]::new()

    foreach ($file in $changedFiles) {
        foreach ($dir in $package.WatchDirectories) {
            if ($file -eq $dir -or $file.StartsWith("$dir/")) {
                $reasons.Add($file)
            }
        }

        if ($package.WatchFiles -contains $file) {
            $reasons.Add($file)
        }
    }

    if (-not $reasons) { return $false }

    $shown = @($reasons | Select-Object -Unique | Select-Object -First 4)
    $suffix = if ($reasons.Count -gt $shown.Count) { " (+$($reasons.Count - $shown.Count) more)" } else { '' }
    Write-Host "    $($package.Id) <- $($shown -join ', ')$suffix"

    return $true
}

function Test-VersionAlreadyPublished($id, $version) {
    try {
        $response = Invoke-RestMethod -Uri "https://api.nuget.org/v3-flatcontainer/$($id.ToLowerInvariant())/index.json" -TimeoutSec 30
        return @($response.versions) -contains $version
    }
    catch {
        # A 404 here just means the package has never been published.
        Write-Host "    (could not read published versions for $id, assuming not published)"
        return $false
    }
}

Write-Step "Reading release manifest."
$packages = Get-ManifestPackages

$baselineRef = Get-BaseLineRef
$changedFiles = Get-ChangedFiles $baselineRef

if ($null -eq $changedFiles) {
    Write-Step "No change information available. Selecting every package."
    $selected = @($packages | ForEach-Object { $_.Id })
}
elseif ($IncludeUnchanged) {
    Write-Step "-IncludeUnchanged was passed. Selecting every package."
    Write-Host "    $($changedFiles.Count) changed file(s) since '$baselineRef'."
    $selected = @($packages | ForEach-Object { $_.Id })
}
else {
    Write-Step "Finding packages affected by $($changedFiles.Count) changed file(s)."
    $affected = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

    foreach ($package in $packages) {
        Write-Host "  $($package.Id)"
        if (Test-PackageAffected $package $changedFiles) {
            [void] $affected.Add($package.Id)
        }
    }

    # Pull in dependents so a dependency is never published on its own.
    $changed = $true
    while ($changed) {
        $changed = $false
        foreach ($package in $packages) {
            if ($affected.Contains($package.Id)) { continue }

            foreach ($dependency in $package.DependsOn) {
                if ($affected.Contains($dependency)) {
                    Write-Host "  $($package.Id) (depends on $dependency)"
                    [void] $affected.Add($package.Id)
                    $changed = $true
                    break
                }
            }
        }
    }

    $selected = @($packages | Where-Object { $affected.Contains($_.Id) } | ForEach-Object { $_.Id })
}

Write-Step "Validating selected packages."

$byId = @{}
foreach ($package in $packages) { $byId[$package.Id] = $package }

# Every package in the manifest must be well formed, not just the selected ones,
# so a typo surfaces on the release that would otherwise hit it.
$allVersions = [ordered]@{}
foreach ($package in $packages) {
    $version = Get-PackageVersion $package
    $allVersions[$package.Id] = $version

    foreach ($dependency in $package.DependsOn) {
        if (-not $byId.ContainsKey($dependency)) {
            throw "Package '$($package.Id)' depends on '$dependency', which is not in the release manifest."
        }
    }
}

foreach ($id in $selected) {
    $version = $allVersions[$id]

    # Only warn off a release build. On a branch build the version is often still
    # the published one, because the bump happens in the release commit, so
    # failing there would break every ordinary build.
    if (-not $SkipVersionCheck -and (Test-VersionAlreadyPublished $id $version)) {
        if ($EnforceVersionCheck) {
            throw "Version $version of $id is already on nuget.org. Either bump <Version> in '$($byId[$id].ProjectPath)' or remove that package from the release by reverting its changes."
        }

        Write-Host "    NOTE: $id $version is already on nuget.org; bump <Version> before releasing it."
    }
}

if (-not $selected) {
    Write-Step "No packages are affected by these changes. Nothing to publish."
}

foreach ($id in $selected) {
    Write-Host "PUBLISH $id $($allVersions[$id])"
}

if ($ChangedPackagesFile) {
    $selected | Set-Content -LiteralPath $ChangedPackagesFile -Encoding UTF8
    Write-Step "Wrote $($selected.Count) package id(s) to '$ChangedPackagesFile'."
}

$env:RELEASE_PACKAGE_IDS = ($selected -join ',')
$env:RELEASE_PACKAGE_VERSIONS = (($selected | ForEach-Object { "$_=$($allVersions[$_])" }) -join ',')

# Success here means "did not throw", not "the last git call returned 0": the
# fallbacks above deliberately recover from non-zero git exit codes, so the stale
# code is cleared rather than left for a caller inspecting $LASTEXITCODE after &.
$global:LASTEXITCODE = 0
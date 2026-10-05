# Releasing

Each package in this repo is versioned and published **independently**. A change
to one package does not force a new version of the others.

| Package | Project | Depends on |
|---|---|---|
| `HttpMessageFormatter` | `src/HttpMessageFormatter` | — |
| `FluentAssertions.Web` | `src/FluentAssertions.Web` | `HttpMessageFormatter` |
| `FluentAssertions.Web.v8` | `src/FluentAssertions.Web.v8` | `HttpMessageFormatter` |
| `AwesomeAssertions.Web` | `src/AwesomeAssertions.Web` | `HttpMessageFormatter` |
| `FluentAssertions.Web.Serializers.NewtonsoftJson` | `src/FluentAssertions.Web.Serializers.NewtonsoftJson` | — |
| `AwesomeAssertions.Web.Serializers.NewtonsoftJson` | `src/AwesomeAssertions.Web.Serializers.NewtonsoftJson` | — |

## How to release

1. Bump `<Version>` in the csproj of each package you want to ship.
2. Commit.
3. Push a tag.

That is the whole process. CI works out which packages to publish from the commits
since the previous tag, so step 1 is the only manual part.

```powershell
# see what the current commit would publish, without publishing
./build/Get-ReleasePackages.ps1

# release everything in the manifest regardless of what changed
./build/Get-ReleasePackages.ps1 -IncludeUnchanged
```

The script fails the build if a package's source changed but its version was not
bumped, which turns "forgot to bump the version" into an error instead of a
silently skipped release.

## Worked example: a non-breaking HttpMessageFormatter change

You changed something in `src/HttpMessageFormatter`, it is not breaking, and you
already bumped `<Version>` to `2.0.5` in
`src/HttpMessageFormatter/HttpMessageFormatter.csproj`.

You also have to bump the three assertion packages. This is the step that is easy
to miss, and the build will stop you if you skip it.

### Why the bump is not optional

`HttpMessageFormatter` is a real NuGet dependency of `FluentAssertions.Web`,
`FluentAssertions.Web.v8` and `AwesomeAssertions.Web`. When NuGet packs an
assertion package it writes the referenced project's version into the nuspec as a
floor. With `HttpMessageFormatter` at `2.0.5` the three assertion packages each
produce:

```xml
<dependency id="HttpMessageFormatter" version="2.0.5" exclude="Build,Analyzers" />
```

That floor is baked into the published nuspec and cannot be edited afterwards,
because nuget.org packages are immutable. If you shipped the assertion packages
still pinned to the floor from the previous release, the new formatter would never
reach anyone who installs them: their nuspec would keep asking for `2.0.4`, and
NuGet would resolve down to it.

So the rule is: **when you bump `HttpMessageFormatter`, bump its three dependents in
the same commit.** CI pulls them into the release automatically via the `DependsOn`
entries in `eng/ReleasePackages.props`, but their `<Version>` is your job.

### The steps

1. Bump the three dependents to `2.0.5` as well:

   ```
   src/HttpMessageFormatter/HttpMessageFormatter.csproj              2.0.4 -> 2.0.5
   src/FluentAssertions.Web/FluentAssertions.Web.csproj              2.0.4 -> 2.0.5
   src/FluentAssertions.Web.v8/FluentAssertions.Web.v8.csproj       2.0.4 -> 2.0.5
   src/AwesomeAssertions.Web/AwesomeAssertions.Web.csproj           2.0.4 -> 2.0.5
   ```

   The two serializer packages stay at `2.0.4`. They do not depend on
   `HttpMessageFormatter`, so nothing about them changed.

2. Build and test, as usual:

   ```powershell
   dotnet build
   dotnet test
   ```

3. Check what the commit will publish. This is read-only, so it is safe to run as
   often as you like:

   ```powershell
   ./build/Get-ReleasePackages.ps1
   ```

   You want to see exactly four `PUBLISH` lines, all at `2.0.5`:

   ```
   PUBLISH HttpMessageFormatter 2.0.5
   PUBLISH FluentAssertions.Web 2.0.5
   PUBLISH FluentAssertions.Web.v8 2.0.5
   PUBLISH AwesomeAssertions.Web 2.0.5
   ```

   If a serializer shows up here, something else changed since the last tag; check
   `git diff 2.0.4...HEAD --stat`. If you see `NOTE: <package> <version> is already
   on nuget.org`, that package still carries its old version, which is the mistake
   this whole section is about.

4. Commit. Keep the source change and the four version bumps in one commit, since
   the tag is what triggers the release:

   ```powershell
   git add src/HttpMessageFormatter src/FluentAssertions.Web src/FluentAssertions.Web.v8 src/AwesomeAssertions.Web
   git commit -m "Fix formatter handling of empty bodies"
   ```

5. Push the branch and tag. The tag name is yours to choose; the previous releases
   use the package-agnostic form `2.0.4`, so `2.0.5` keeps the sequence readable:

   ```powershell
   git push origin master
   git push origin 2.0.5
   ```

6. Watch the AppVeyor build. It will report `Release includes 4 package(s)` and, once
   it finishes, `HttpMessageFormatter`, `FluentAssertions.Web`,
   `FluentAssertions.Web.v8` and `AwesomeAssertions.Web` will be on nuget.org. The
   AwesomeAssertions feed and the GitHub release are published by the same build.

### What you did not have to do

- You did not edit `eng/ReleasePackages.props`. The `DependsOn` entries already
  handle the dependency chain.
- You did not bump the two serializer packages.
- You did not touch `appveyor.yml` or run any publishing command yourself.

### If the build fails

| Message | Cause | Fix |
|---|---|---|
| `Version 2.0.4 of FluentAssertions.Web is already on nuget.org` | You bumped the formatter but not its dependents | Bump the three dependents to `2.0.5` and push a new tag |
| `Version 2.0.5 of HttpMessageFormatter is already on nuget.org` | That version already shipped | Bump to `2.0.6` |
| `No git tags available` | Tag history was not fetched | Usually transient; re-run the build. If it persists, push a baseline tag |
| `dotnet pack failed for <id>` | A compile or pack error | Read the error above it; this is not a versioning problem |

Because nuget.org does not allow overwriting a published version, a failed release
cannot be retried with the same numbers. Bump to the next version and tag again.

## What decides which packages ship

`eng/ReleasePackages.props` maps paths to packages. A package is affected when a
changed file sits under one of its `WatchDirectories` or matches one of its
`WatchFiles`.

The map encodes two things worth knowing:

- **The three assertion flavours share source.** They link-compile
  `src/FluentAssertions.Web` with different `#define`s, so an edit there affects
  all three. They usually release together, but they carry separate versions, so
  a change confined to one of them (its `csproj`, a new serializer dependency)
  can still ship alone.
- **`HttpMessageFormatter` is a real dependency** of the three assertion
  packages. When it ships, its dependents ship too, so nothing is left published
  against a dependency version it was never built with.

`Directory.Packages.props` is watched by every package on purpose. Bumping
`FluentAssertions`, `AwesomeAssertions`, `Newtonsoft.Json` or `System.Text.Json`
changes what ships inside every package.

## Adding a package

1. Add `<Version>` to the new csproj.
2. Add a `ReleasePackage` entry to `eng/ReleasePackages.props` with its watch
   paths and any `DependsOn`.
3. Add it to `Get-FeedFor` in `build/Pack-ReleasePackages.ps1` so it lands on the
   right NuGet feed.

## Local verification

```powershell
dotnet build
dotnet test
./build/tests/Test-ReleasePackageDetection.ps1
```

The detection test builds a throwaway git repo and checks that each kind of change
selects the right packages, so you can change the manifest without guessing.
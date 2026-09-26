# .NET 10 Upgrade Plan

## Overview

**Target**: Invest.MVC.csproj (net8.0 → net10.0)
**Scope**: Single ASP.NET Core Razor Pages project, 77 files, ~7.8k LOC

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: Single project, already on modern .NET (net8.0), SDK-style, straightforward TFM update with package upgrades.

## Tasks

### 01-prerequisites: Validate upgrade prerequisites

Verify the development environment is ready for .NET 10 upgrade. Check that the .NET 10 SDK is installed and that any global.json files in the repository are compatible with .NET 10. This ensures the upgrade can proceed without toolchain compatibility issues.

**Done when**: .NET 10 SDK verified installed, global.json validated or updated if needed, no SDK version conflicts detected

---

### 02-upgrade-tfm-packages: Upgrade target framework and packages

Update the project's TargetFramework to net10.0 and upgrade all NuGet packages to compatible versions. This includes replacing the 1 incompatible package with a compatible alternative and upgrading the 5 packages that have recommended updates. Address the 14 identified issues including source incompatibilities, behavior changes, and API migrations.

The assessment identified specific concerns: API compatibility issues (ruleId=Api.0002, Api.0003), package incompatibilities (NuGet.0001), and recommended package updates (NuGet.0002). Research these specific issues in the assessment before starting work.

**Done when**: TargetFramework set to net10.0, all packages upgraded to net10.0-compatible versions, incompatible package replaced, solution builds with zero errors and warnings

---

### 03-enable-nullable: Enable nullable reference types

Enable nullable reference types in the project and systematically annotate the codebase. This modernization step helps prevent null reference exceptions and improves code quality by making nullability explicit.

This is a code quality improvement that takes advantage of C# features available in .NET 10. The work involves enabling the feature in the project file and annotating types across the 77 source files.

**Done when**: Nullable reference types enabled in project file, code annotated for nullability, all nullable warnings resolved, solution builds with zero errors and warnings

---

### 04-final-validation: Validate upgrade completion

Perform final validation of the upgrade. Build the solution, run all tests, and verify that the application functions correctly on .NET 10. Document any recommendations for future improvements or deferred modernizations.

**Done when**: Solution builds successfully on net10.0, all tests pass, application runs without runtime errors, upgrade completion documented

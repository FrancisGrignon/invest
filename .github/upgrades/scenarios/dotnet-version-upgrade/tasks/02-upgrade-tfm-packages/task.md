# 02-upgrade-tfm-packages: Upgrade target framework and packages

Update the project's TargetFramework to net10.0 and upgrade all NuGet packages to compatible versions. This includes replacing the 1 incompatible package with a compatible alternative and upgrading the 5 packages that have recommended updates. Address the 14 identified issues including source incompatibilities, behavior changes, and API migrations.

The assessment identified specific concerns: API compatibility issues (ruleId=Api.0002, Api.0003), package incompatibilities (NuGet.0001), and recommended package updates (NuGet.0002). Research these specific issues in the assessment before starting work.

**Done when**: TargetFramework set to net10.0, all packages upgraded to net10.0-compatible versions, incompatible package replaced, solution builds with zero errors and warnings

## Research Findings

### Project Affected
- **Invest.MVC.csproj** — ASP.NET Core Razor Pages, SDK-style, net8.0 → net10.0

### Packages to Update

| Package | Current | Target | Action |
|---------|---------|--------|--------|
| Microsoft.EntityFrameworkCore.Design | 8.0.29 | 10.0.10 | Upgrade |
| Microsoft.EntityFrameworkCore.Sqlite | 8.0.28 | 10.0.10 | Upgrade |
| Microsoft.EntityFrameworkCore.Tools | 8.0.28 | 10.0.10 | Upgrade |
| Microsoft.VisualStudio.Web.CodeGeneration.Design | 8.0.23 | 10.0.2 | Upgrade |
| System.Text.Json | 8.0.6 | 10.0.10 | Upgrade |
| Microsoft.VisualStudio.Azure.Containers.Tools.Targets | 1.23.0 | N/A | **Remove** (incompatible, no net10.0 version) |
| Highsoft.Highcharts | 11.4.6.5 | — | Keep (compatible) |
| NuGet.Packaging | 6.14.3 | — | Keep (compatible) |
| NuGet.Protocol | 6.14.3 | — | Keep (compatible) |

### API Issues Identified

**Source incompatibilities (Api.0002) — 3 occurrences:**
- `TimeSpan.FromMinutes(double)` in Startup.cs (lines 37, 52)
- `TimeSpan.FromSeconds(double)` in Startup.cs (line 65)
- **Resolution**: These APIs remain but might have floating-point precision changes. Review usage after upgrade.

**Behavior changes (Api.0003) — 4 occurrences:**
- `UseExceptionHandler(string)` in Startup.cs (line 80) — behavior change in exception handling
- `JsonDocument.Parse` in StockService.cs (line 51) — behavior change in JSON parsing
- `HttpContent.ReadAsStringAsync` in StockService.cs (line 50) — behavior change

### Files to Modify
1. **Invest.MVC.csproj** — Update TargetFramework, package versions, remove incompatible package
2. **Startup.cs** — Verify TimeSpan and UseExceptionHandler behavior after upgrade
3. **Infrastructure/Services/StockService.cs** — Verify JsonDocument and HttpContent behavior after upgrade

### Execution Plan
1. Update TargetFramework from net8.0 to net10.0
2. Update EF Core packages to 10.0.10
3. Update CodeGeneration.Design to 10.0.2
4. Update System.Text.Json to 10.0.10
5. Remove Microsoft.VisualStudio.Azure.Containers.Tools.Targets
6. Build and fix any breaking changes
7. Verify all API behaviors match expectations

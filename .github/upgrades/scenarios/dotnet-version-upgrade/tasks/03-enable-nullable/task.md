# 03-enable-nullable: Enable nullable reference types

Enable nullable reference types in the project and systematically annotate the codebase. This modernization step helps prevent null reference exceptions and improves code quality by making nullability explicit.

This is a code quality improvement that takes advantage of C# features available in .NET 10. The work involves enabling the feature in the project file and annotating types across the 77 source files.

**Done when**: Nullable reference types enabled in project file, code annotated for nullability, all nullable warnings resolved, solution builds with zero errors and warnings

## Research Findings

### Project Status
- **Invest.MVC.csproj** — ASP.NET Core Razor Pages on net10.0
- **Current nullable setting**: Not set (disabled by default)
- **Source files**: 77 files to annotate
- **Language version**: C# 12 (default for .NET 10) — supports nullable reference types

### Migration Strategy
Using **Warnings-first** approach (recommended for projects with 50+ files):
1. Enable `<Nullable>warnings</Nullable>` — enables warnings without changing type semantics
2. Build and fix all dereference warnings (CS8602, CS8600, CS8603, CS8604)
3. Change to `<Nullable>enable</Nullable>` — activates full annotations
4. Fix annotation warnings (CS8618, CS8625, CS8601)
5. Clean build verification

### File Categories to Annotate
- Models & DTOs (cross trust boundaries — properties nullable by default)
- Domain models (internal invariants — prefer non-nullable with constructor enforcement)
- Services & infrastructure
- Controllers & Pages
- Startup & configuration

### Key Principles
- Return types must reflect semantic nullability (methods named `*OrDefault` return `T?`)
- Do not remove existing `ArgumentNullException` checks
- Do not let warnings drive annotations — decide intended nullability first
- For libraries: track breaking changes in public APIs

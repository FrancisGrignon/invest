# Mise à niveau vers .NET 10

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0 (.NET 10 LTS)

## Source Control
- **Source Branch**: main
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: Single Commit at End
- **Branch Sync**: None, no merges allowed

## Upgrade Options
**Source**: .github/upgrades/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

### Compatibility
- Unsupported Packages: Replace with Compatible Alternatives

### Modernization
- Nullable Reference Types: Enable

## Strategy
**Selected**: All-at-Once
**Rationale**: Single project already on modern .NET (net8.0), SDK-style format, straightforward TFM bump with package updates - atomic upgrade is most efficient.

### Execution Constraints
- Single atomic upgrade — all changes applied together
- Validate full solution build after upgrade completes
- No multi-targeting or phased rollout
- Build errors must be resolved in a single bounded pass
- Testing occurs after atomic upgrade succeeds

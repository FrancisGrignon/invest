# Upgrade Options — Invest.MVC

Assessment: 1 project (net8.0 → net10.0), 77 files, 7848 LOC, 14 issues, 1 incompatible package, 5 upgrade recommended

## Strategy

### Upgrade Strategy
Single project upgrade — straightforward TFM update with package upgrades

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade all projects simultaneously in a single atomic pass |

## Compatibility

### Unsupported Packages
1 package is incompatible with net10.0

| Value | Description |
|-------|-------------|
| **Replace with Compatible Alternatives** (selected) | Find and migrate to compatible replacements |
| Accept Build Failures | Document incompatible packages, proceed with build errors |

## Modernization

### Nullable Reference Types
Target is net10.0, C# project, nullable references not yet enabled

| Value | Description |
|-------|-------------|
| **Enable** (selected) | Enable nullable reference types and annotate code |
| Skip | Leave nullable disabled |

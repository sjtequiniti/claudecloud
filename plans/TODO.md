# TODO

## Session notes (2026-02-04)

### Completed this session

**APL64 CPC integration complete:**
- Received fat CPC package from APL2000 (`cpcroots/`)
- Confirmed .NET 9 requirement (SDK 9.0.308, target `net9.0`)
- Installed .NET 9.0.310 SDK locally
- Upgraded Azure Function and test projects to .NET 9
- Renamed `install-dotnet8.ps1` to `install-dotnet9.ps1`
- Added `cpcroots/` to repo with NuGet package
- Created `NuGet.config` for local package source
- Integrated `RootsClass` into `GetRoot.cs` (replaces `Math.Sqrt`/`Math.Cbrt`)
- All 8 tests pass with APL implementation

**CPC API:**
- `RootsClass.SquareRootCalculation(double)` returns `(double result, bool hasError, string errMsg)`
- `RootsClass.CubeRootCalculation(double)` returns `(double result, bool hasError, string errMsg)`

**Fat CPC contents (5.8 MB uncompressed):**
- `CPCRoots.dll` (28 KB) - APL workspace
- `APLNow.Core.dll` (4.8 MB) - APL interpreter
- Support DLLs for data, files, XML, etc.

### Next steps
- Deploy Azure Function to Azure
- Test on Azure platform
- Dyalog APL integration (Part 1 remaining)

---

## Session notes (2026-02-03)

### Completed this session
- Reviewed APL64 2025.0.9 update PDF from APL2000
- Discovered thin CPC export is now available (excludes .NET runtime)
- Updated deployment decision: use thin CPC instead of fat CPC
- Researched Azure Functions .NET 9 support (confirmed GA)
- Identified open question: APL64 may require .NET 9 (per PDF screenshots)

### APL64 2025.0.9 Update (September 2025)

Reviewed `docs/APL64_UPDATE_SEPTEMBER_2025.pdf`. Key finding:

- **Thin CPC now available**: New WRE output type "Single Exe File Excluding .Net Runtime"
  - Produces a single exe with APL64 runtime but without .NET runtime
  - Target must have .NET runtime installed (Azure Functions already do)
  - Significantly smaller than fat CPC (~300 MB)

This makes thin CPC viable for Azure deployment.

### Answered: .NET 9 is required

Confirmed via received CPC package. Azure Functions supports .NET 9 (GA until November 2026).

**Azure Functions .NET support** (isolated worker model):
| Version | Status | End-of-Support |
|---------|--------|----------------|
| .NET 10 | GA | November 2028 |
| .NET 9 | GA | November 2026 |
| .NET 8 | GA | November 2026 |

Sources:
- https://learn.microsoft.com/en-us/azure/azure-functions/supported-languages
- https://learn.microsoft.com/en-us/azure/azure-functions/functions-versions

---

## Session notes (2026-02-02, afternoon)

### Completed this session
- Created `azure-function-initial` branch
- Initialized Azure Function project: `src/SupervalFunction`
  - .NET 8 Isolated worker model
  - HTTP trigger function (`GetRoot`)
- Added test project: `tests/SupervalFunction.Tests`
  - 8 unit tests for GetRoot function (TDD approach)
- GetRoot function calculates square/cube roots via HTTP GET
- Cleaned up .gitignore files (removed Node.js entries, now C#-focused)
- Tested locally: function responds correctly

### Next steps
- Integrate APL64 CPC for root calculation

---

## Session notes (2026-02-02, morning)

### Completed this session
- Created `scripts/install-dotnet8.ps1` for reproducible local setup
- Installed .NET 8.0.417 SDK via winget
- Created `scripts/install-azure-tools.ps1` and `.sh` for Azure Functions Core Tools
- Installed Azure Functions Core Tools v4.6.0 via winget
- Selected C# as the implementation language (APL64 produces .NET assemblies)
- Read APL2000's CPC documentation - APL64 CPC is a NuGet package, integrates natively with C#

### Decisions made
- **Language**: C# (not Node.js) - native .NET integration with APL64 CPC
- **CPC deployment**: Use thin CPC (excludes .NET runtime, Azure provides it)
- **Azure Functions model**: .NET 8.0 Isolated worker (per APL2000 example)

### Answered questions
- Can APL64/Dyalog target Linux? **Yes** (both can target Linux, enabling 1.5 GB temp storage option)

### Open questions to investigate
- Does Flex Consumption support Durable Functions?

---

## Session notes (2026-02-01)

### Completed this session
- Set up CLAUDE.md and README.md project documentation
- Answered viability questions: project is feasible
- Determined Flex Consumption is the right hosting plan (scales to zero, 800 MB temp storage)
- Documented sources for Azure findings

### Next steps
- Answer remaining open questions in README (scripting language, file storage, etc.)
- Begin Part 1: Create a basic Azure Function with a scripting language
- Decide which scripting language to use (Node is most familiar per README)

### Open questions to investigate
- Does Flex Consumption support Durable Functions?
- Does Flex Consumption support custom handlers?
- Can APL64/Dyalog target Linux (for 1.5 GB temp storage option)?

---

## Constraints

- Use Flex Consumption hosting plan (800 MB temp storage, scales to zero, pay-per-use)
- Deploy APL executable via custom handler configuration

## Part 1: Azure Function

- [x] Create an Azure Function (C# .NET 8 Isolated)
- [x] Upgrade to .NET 9 (required by APL64 CPC)
- [x] Integrate APL64 CPC (`APLNext_LLC.CPCRoots` package)
- [ ] Replace C# root calculation with Dyalog APL

## Part 2: Azure Durable Function

- [ ] Create an Azure Durable Function using a scripting language
- [ ] Replace the script with an APL64 executable
- [ ] Replace the script with a Dyalog APL executable

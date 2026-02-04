# Claude Code Session - 2026-02-03

## Project: claudecloud (Azure Functions with APL64)

---

## Session Start

**User:** Ready to resume?

**Claude:** Checked TODO.md for project status. Last session:
- Created a C# Azure Function with `GetRoot` that calculates square/cube roots
- Added 8 unit tests (TDD approach)
- Tested locally and it works

**Next step**: Integrate APL64 CPC to replace the C# root calculation.

---

## APL64 2025.0.9 Update Review

**User:** News just in from APL2000: see APL64 Update September 2025 PDF in docs folder, describing how the APL64 interpreter can export a thin CPC. Please read and summarise what you learn.

**Claude:** Read the PDF (20 pages). Key findings:

### Key Finding: Thin CPC Export Option

The most relevant news for this project is on **page 17**:

**New WRE Output Type: "Single Exe File Excluding .Net Runtime"**

This creates a single executable containing:
- APL64 application-specific files
- APL64 runtime files
- **Without** the .NET Runtime files

The target workstation must have the appropriate .NET runtime already installed.

This is significant for Azure deployment because:
- Azure Functions already have .NET 8 runtime available
- The thin CPC can be much smaller than ~300 MB
- Reduces cold start time and deployment package size

### Other Notable Features

| Feature | Description |
|---------|-------------|
| `⎕PDIST` | Statistical distributions via MathNet (useful for actuarial work) |
| `⎕ROUND` | Multiple rounding modes (MidpointRounding options) |
| `⎕CSR` / `⎕CPCIS` | New system functions for CPC creation tooling |
| `⎕CPC PropValue` | Programmatic control of CPC configuration |
| Intellisense | CPC tool can generate .NET Intellisense documentation from APL comments |
| Case insensitivity | .NET data types in public function headers are now case-insensitive |

### Implications for This Project

1. **Thin CPC is now viable** - we can export a CPC without bundling the .NET runtime
2. **Smaller deployment** - should fit comfortably within Azure Flex Consumption limits
3. **Faster cold starts** - less to load on function startup

---

## TODO Update

**User:** Update TODO with just the information relevant to this project.

**Claude:** Updated TODO.md:
1. Added new session note documenting the APL64 2025.0.9 thin CPC capability
2. Changed decision from "start with fat CPC" to "use thin CPC"
3. Removed resolved question about thin CPC feasibility

---

## .NET Version Investigation

**User:** The PDF illustrates the export of a CPC. The .Net Framework version is not mentioned or specified in the document, but the image shows filepaths that mention .Net 9. It is possible that an APL64 CPC requires .Net 9. What do we know about Azure Functions support for .Net 9?

**Claude:** Researched Azure Functions .NET 9 support.

### Azure Functions .NET 9 Support

**.NET 9 is fully supported** in Azure Functions (GA) with the isolated worker model.

| .NET Version | Support Level | End-of-Support |
|--------------|---------------|----------------|
| .NET 10 | GA | November 2028 |
| .NET 9 | GA | November 2026 |
| .NET 8 | GA | November 2026 |

### Implications for This Project

1. **We're already using the isolated worker model** - correct approach
2. **If APL64 CPC requires .NET 9**, we can upgrade from .NET 8 to .NET 9
3. **Flex Consumption plan supports both** .NET 8 and .NET 9

### Recommendation

We should confirm with APL2000 which .NET version the thin CPC targets. The filepath `\net9.0\` in the screenshots suggests .NET 9, but it could be configurable. If .NET 9 is required, upgrading our Azure Function from .NET 8 to .NET 9 is straightforward.

Sources:
- https://learn.microsoft.com/en-us/azure/azure-functions/supported-languages
- https://learn.microsoft.com/en-us/azure/azure-functions/functions-versions

---

## Final TODO Update

**User:** Yes, with your sources.

**Claude:** Added open question about .NET version requirements to TODO.md with Azure Functions support table and source links.

---

## Commit & Push

**User:** commit

**Claude:** Committed changes:
```
444c6ac Add APL64 2025.0.9 update notes: thin CPC now viable

- APL64 can now export CPC without bundled .NET runtime
- Updated deployment decision from fat to thin CPC
- Added open question re .NET 9 requirement (per PDF paths)
- Documented Azure Functions .NET support matrix
```

Files committed:
- `plans/TODO.md` (modified)
- `docs/APL64_UPDATE_SEPTEMBER_2025.pdf` (new)

**User:** push

**Claude:** Pushed to origin/main.

---

## Session Summary

### Key Outcomes
1. Reviewed APL64 2025.0.9 update - thin CPC export is now available
2. Updated project decision to use thin CPC instead of fat CPC
3. Identified potential .NET 9 requirement - needs confirmation from APL2000
4. Confirmed Azure Functions supports .NET 9 (GA)

### Open Questions
- Which .NET version does APL64 CPC require? (screenshots suggest .NET 9)
- Does Flex Consumption support Durable Functions?

### Next Steps
- Confirm .NET version requirement with APL2000
- Integrate APL64 CPC for root calculation

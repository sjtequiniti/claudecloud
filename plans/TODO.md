# TODO

## Session notes (2026-02-02)

### Completed this session
- Selected C# as the implementation language (APL64 produces .NET assemblies, most APL64 users use C#)
- Read APL2000's CPC documentation - APL64 CPC is a NuGet package, integrates natively with C#
- No custom handler needed: APL functions become .NET methods called directly from C#

### Decisions made
- **Language**: C# (not Node.js) - native .NET integration with APL64 CPC
- **CPC deployment**: Start with fat CPC (~300 MB, includes .NET runtime), explore thin CPC later
- **Azure Functions model**: .NET 8.0 Isolated worker (per APL2000 example)

### Next steps
- Begin Part 1: Create a basic C# Azure Function
- Follow APL2000's example as a guide
- Verify .NET version compatibility with APL64 CPC

### Answered questions
- Can APL64/Dyalog target Linux? **Yes** (both can target Linux, enabling 1.5 GB temp storage option)

### Open questions to investigate
- Which .NET version does APL64 require? (affects thin CPC feasibility)
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

- [ ] Create an Azure Function using a scripting language
- [ ] Replace the script with an APL64 executable
- [ ] Replace the script with a Dyalog APL executable

## Part 2: Azure Durable Function

- [ ] Create an Azure Durable Function using a scripting language
- [ ] Replace the script with an APL64 executable
- [ ] Replace the script with a Dyalog APL executable

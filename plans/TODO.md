# TODO

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

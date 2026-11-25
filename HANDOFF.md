# Handoff

Updated: 2026-11-25

## State
See `context/CURRENT-STATE.md` for batch status and `research/journals/` for the per-batch log.

## Owner actions outstanding
1. **Visibility:** public content limited to owner decision 0D05 classes; stand up the restricted store for evidence and findings.
2. **Install Praxis** in this repository (`praxis init`), pin the version in `ros.json`/`.echelon/toolchain.json`, and enable `requireVerifiedCompletion` if adopted. Not done in the research session: the installer download was not permitted there.
3. **Require the `assurance-validation` check** in a `main` branch ruleset; CI only advises until the check is required.
4. Done: register tooling ported to F# with tests (owner decision 0D06; journal JR-ASR-2026-0D00). Still to do: install Praxis itself.

## Owner approvals outstanding
- Approve the reconciled baseline (owner stage P0 exit, `requirements/reconciliation-and-roadmap.md`).
- Review the requirement challenge review and the capability verification matrix.

## How to continue
1. Read `AGENTS.md`, `SECURITY-AND-CONFIDENTIALITY.md`, `methodology/`.
2. Run `dotnet run --project src/Echelon.Assurance.Cli -- merge` after adding fragments, then `... -- validate` and `dotnet test`.
3. Pick the next open item from `registers/open-questions.csv` or the roadmap.

# Handoff

Updated: 2026-11-25

## State
See `context/CURRENT-STATE.md` for batch status and `research/journals/` for the per-batch log.

## Owner actions outstanding
1. **Visibility:** public content limited to owner decision 0D05 classes; stand up the restricted store for evidence and findings.
2. **Install Praxis** in this repository (`praxis init`), pin the version in `ros.json`/`.echelon/toolchain.json`, and enable `requireVerifiedCompletion` if adopted. Not done in the research session: the installer download was not permitted there.
3. **Require the `assurance-validation` check** in a `main` branch ruleset; CI only advises until the check is required.
4. Port the Python register tooling to F# with equivalent tests (owner decision 0D06; see journal JR-ASR-2026-0D00).

## How to continue
1. Read `AGENTS.md`, `SECURITY-AND-CONFIDENTIALITY.md`, `methodology/`.
2. Run `python3 -I tools/merge_fragments.py` after adding fragments, then `python3 -I tools/validate_registers.py`.
3. Pick the next open item from `registers/open-questions.csv` or the roadmap.

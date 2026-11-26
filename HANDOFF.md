# Handoff

Updated: 2026-11-26 (final reconciliation; review record `findings/reviews/RV-ASR-2026-11-26.md`; Praxis work item WI-ASR-FINAL)

## State
See `context/CURRENT-STATE.md` and `findings/executive-findings.md`. The
consolidation branch `claude/echelon-assurance-consolidation-jygy23` contains
PR #1 (`claude/fervent-tesla-ytnxz6`) and PR #2
(`claude/echelon-transparency-governance-rvt82z`) plus the consolidation.

## Owner actions outstanding
1. All twelve owner decision package items are decided (DF-ASR-2026-0D0F, DF-ASR-2026-0D10). The emergency delegate and protected-reporting roles are defined; designating individuals is deferred until operationally necessary (ODP-04). Until then out-of-band alerting reaches only the owner (RSK-065).
2. Apply a ruleset on `main` requiring the `assurance-validation` and `Praxis validation` checks. This is WI-ASR-03. Praxis 3.25.0 was installed on 2026-11-25. As of 2026-11-25 the checks are advisory only.
3. Visibility (decision 0D05): keep public content to the permitted classes; stand up the restricted store before any production evidence exists.
4. Production activation requires every gate in `registers/production-gates.csv` to be met with evidence. None is met.
5. Provision a Praxis verifier with its own identity and pinned keys (WI-PRAXIS-03) for later work. PR #3 was merged into `main` (9ab2b00) on the owner's instruction without verified completion, and plain completion was accepted under the owner's delegation (DF-ASR-2026-0D12). The fixes made after the merge are in pull request #4 (branch `claude/echelon-assurance-consolidation-jygy23`).
6. Owner decisions on two sensitivity items: earlier specific wording in git history (commit 23f06b4), and commit author metadata recorded in `.ros/events/events.jsonl` (`SECURITY-AND-CONFIDENTIALITY.md`).

## How to continue
1. Read `AGENTS.md`, `SECURITY-AND-CONFIDENTIALITY.md` and `requirements/README.md`.
2. Change requirements only in `requirements/baseline.csv`. Disposition any new source requirement in `requirements/source-dispositions.csv`.
3. Run `dotnet run --project src/Echelon.Assurance.Cli -- validate .`, `-- trace .`, `-- roadmap .`, `-- econ .` and `dotnet test`. CI fails on stale generated views.
4. Pick the next item from `roadmap/backlog.csv` (P0 first). Items in other repositories are proposals for those repositories' own governance.

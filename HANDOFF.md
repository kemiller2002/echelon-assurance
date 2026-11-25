# Handoff

Updated: 2026-11-25 (assurance consolidation, journal `JR-ASR-2026-C001`)

## State
See `context/CURRENT-STATE.md` and `findings/executive-findings.md`. The
consolidation branch `claude/echelon-assurance-consolidation-jygy23` contains
PR #1 (`claude/fervent-tesla-ytnxz6`) and PR #2
(`claude/echelon-transparency-governance-rvt82z`) plus the consolidation.

## Owner actions outstanding
1. Decide the ten items in `docs/decisions/owner-decision-package-2026-11-25.md`.
2. Apply a ruleset on `main` requiring the `assurance-validation` check, and install Praxis (`praxis init`, pinned version). This is WI-ASR-03. As of 2026-11-25 the checks are advisory only.
3. Visibility (decision 0D05): keep public content to the permitted classes; stand up the restricted store before any production evidence exists.

## How to continue
1. Read `AGENTS.md`, `SECURITY-AND-CONFIDENTIALITY.md` and `requirements/README.md`.
2. Change requirements only in `requirements/baseline.csv`. Disposition any new source requirement in `requirements/source-dispositions.csv`.
3. Run `dotnet run --project src/Echelon.Assurance.Cli -- validate .`, `-- trace .`, `-- roadmap .`, `-- econ .` and `dotnet test`. CI fails on stale generated views.
4. Pick the next item from `roadmap/backlog.csv` (P0 first). Items in other repositories are proposals for those repositories' own governance.

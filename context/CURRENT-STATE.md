# Current state

Updated: 2026-11-25

## Baseline

- Repository created by owner (initial commit `48f04ef`, README only).
- Repository is public; owner decision 0D05 limits it to public research,
  policies, control objectives, methodology and sanitized results.
- Praxis CLI not installed (see `HANDOFF.md`); conventions adopted manually.
- Register tooling is F# (`assurance` CLI, 33 tests) per owner decision 0D06.
- Existing assurance work elsewhere (read-only inputs):
  - Unbound: `requirements/governance/` (SOC 2 readiness directive `SOC2-001…036`,
    master enterprise control matrix `UNB-CTRL-*`, specific executable controls
    `UNB-SCTRL-*`, continuous control monitoring requirements and tests),
    `architecture/CONTINUOUS-CONTROL-MONITORING-AND-EVIDENCE-ASSURANCE.md`.
  - daf-landscape: `research/soc2-assessment-2026-11-11.md`,
    `research/technology-control-map.md`, `docs/assurance-methodology.md`,
    `docs/financial-core-control-spec-v0.5.md`.

## Research batches

| Batch | Scope | Status |
|---|---|---|
| B0 | Repository establishment | complete |
| B1 | Echelon system evaluation | complete |
| B2 | SOC landscape | complete |
| B3 | Regulatory and framework crosswalk | complete |
| B4 | Penetration testing, compliance vendors | complete |
| B5 | Policy as code, evidence technology | complete |
| B6 | AI governance | complete |
| B7 | Synthesis: control catalog, policies, architecture, economics, roadmap | complete |
| B8 | Review passes 1–10 | complete (journals JR-ASR-2026-5C01..5C03) |

Registers: 276 sources, 278 requirements, 104 controls, 83 policy entries,
30 evidence types, 32 validation procedures, 58 risks, 16 decisions (14 owner-accepted 0D01..0D0E; 2 proposals superseded),
open questions in `registers/open-questions.csv`. See
`research/journals/` for the batch log.

## Claims status

No control is claimed above `Designed`. No compliance, attestation or
effectiveness claim is made.

# Requirements

## Authoritative registers

| File | What it is |
|---|---|
| `baseline.csv` | **Unified requirements baseline.** It holds 117 baseline requirements: the 113 approved in principle by the owner (ODP-01, DF-ASR-2026-0D10) plus four additions, each traced to an owner item — BR-VAL-004 (ODP-02, DF-ASR-2026-0D0F), BR-LOG-002 (ODP-06), BR-APP-014 (ODP-07) and BR-ASR-012 (OT-01, evidence by construction), the last three DF-ASR-2026-0D10. The count change is recorded in the ODP-01 row of `source-dispositions.csv` (`BR-<AREA>-NNN`), the authoritative statement of every Echelon-internal requirement. Each row sits under control objectives (`control_ids`) and policies, and traces to tenets, risks and original requirement IDs (`source_refs`). Each row also records: basis, owner, enforcement mechanism and class, automation target, validation method and triggers, evidence, dependencies, failure behavior, residual risk, implementation status, external dependencies (open questions), incremental cost, disposition, phase (the earliest work-item phase) and launch gate (the latest point by which all of its work must be complete: before production, before an observation period, before external examination, on demand, or not a launch gate). The validator checks the gate against the latest phase of the work items that address it. Architectural guarantees state what they **prevent** and **do not prevent**. |
| `source-dispositions.csv` | **Consolidation and removal register.** It has one row for each of the 222 source items: the 189 original owner requirement IDs (SBC, FIN-BC, PEC, AIG-BC, VAL-BC, CST, ECON, GOV-BC, CA), the 18 reconciliation challenge findings (REC-01..18), the security-by-construction validation paragraph (SBC-VC-01), and the 14 owner decision items of 2026-11-25 (ODP-01..12, OT-01 evidence by construction, OT-02 external services not the default). Each row gives its disposition (retain, consolidate, strengthen, simplify, replace, defer, remove, escalate), the baseline rows it maps to, and the reasoning. |
| `generated/traceability-matrix.md` | Generated forward (requirement → tenet, policy, control, validation, evidence) and reverse (control → requirements) views. |

Externally derived obligations remain in `regulatory/requirements.csv`. The
owner-requirement rows there (REQ-SBC, REQ-FINBC, REQ-PEC, REQ-AIGBC, REQ-VALBC)
keep their IDs and are dispositioned here (decision DF-ASR-2026-C001, accepted).

## Hierarchy

```
tenets (registers/tenets.csv)
  → policies (policies/policy-inventory.csv)
    → control objectives (controls/control-catalog.csv)
      → baseline requirements (requirements/baseline.csv)
        → enforcement mechanism + class
        → validation (registers/validation-procedures.csv, triggers)
        → evidence (registers/evidence-types.csv, obligation)
```

## Source documents (historical, not edited further)

The Markdown files in this directory are the source records the baseline
consolidates:

- `security-by-construction.md`
- `financial-integrity-by-construction.md`
- `executable-policy-and-continuous-evidence.md`
- `ai-governance-independent-validation.md`
- `continuous-security-testing.md`
- `assurance-economics.md`
- `organizational-governance.md`
- `reconciliation-and-roadmap.md`
- `challenge-review.md`
- `continuous-assurance-requirements.md` (PR #2)

Where they conflict with `baseline.csv`, the baseline governs.

## Validation

`assurance validate` enforces the following:

- every reference resolves;
- every live baseline row has its required fields;
- source-to-baseline links are bidirectional;
- every owner requirement is dispositioned;
- every control is supported by a requirement and has an objective, owner, validation and evidence;
- no baseline row claims a status above its controls;
- every tenet is realized;
- every open question has a disposition and next action;
- the backlog has no later-phase dependencies or cycles.

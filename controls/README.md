# Unified control catalog

`control-catalog.csv` is the single source of truth for Echelon Foundry
controls. One control satisfies many requirements; requirements from every
framework point at controls (`regulatory/requirements.csv → control_ids`), and
controls point back (`requirement_ids`). The validator enforces that both
directions reference existing records.

## Status vocabulary (never infer one state from another)

Adopted from Unbound `SOC2-019` and extended:

| Status | Meaning | Evidence needed to claim it |
|---|---|---|
| `Proposed` | Identified as needed | Rationale |
| `Designed` | Objective, owner, mechanism, evidence defined | This catalog row |
| `Specified` | Implementation specification exists | Linked specification |
| `Implemented` | Mechanism exists in a system | Code/config reference and test |
| `Operating` | Has produced operating evidence in the real environment | Dated evidence records |
| `Tested` | Effectiveness tested by a validation procedure | A `VAL-` result at `technically-separated` level or higher (separately credentialed verifier with bound receipts, VAL-040); Multiple-agent review (`agent-review`, VAL-041) is approved for internal development review only (owner direction DF-ASR-2026-0D0F) and never supports `Tested`; judgment controls (automation target L0–L2) therefore cannot reach `Tested` until a technically separated or organizational validator exists, and this is disclosed. Levels are distinct and never interchangeable: `agent-review`, `technically-separated`, `internal-independent` (organizational independence: a person outside management), `qualified-professional`, `external-technical`, `external-penetration`, `external-attestation`. Automated or self-assessment results alone never support `Tested`. |
| `Deficient` | Failed a test or found not operating | Finding reference (restricted) |
| `Remediating` | Deficiency under remediation | Remediation record |
| `AuditorReviewed` | Examined by an independent auditor in an attestation | Auditor report reference |
| `Retired` | No longer used | Decision record |

As of 2026-11-25 no control in this catalog is above `Designed` on the basis
of evidence held in this repository.

## Automation levels

| Level | Meaning |
|---|---|
| `L0` | Manual documentation |
| `L1` | Automated reminders and task creation |
| `L2` | Automated evidence collection |
| `L3` | Automated control validation |
| `L4` | Automated preventive enforcement |
| `L5` | Continuous enforcement with bounded automated remediation |

`automation_target` is the highest *appropriate* level, not the highest
possible. See `automation/control-automation-matrix.md` for the reasoning.

## Failure modes

`fail-closed` (the protected action is blocked when the control cannot
evaluate), `fail-open-justified` (action proceeds, exception raised; rationale
required in `failure_response`), `detective-only` (no blocking point exists),
`n/a`.

## Catalog summary (B7, 2026-11-25)

Seeded at B7 by a one-time builder (since retired; see git history). The CSV is
authoritative. 104 controls across 19 domains at B7; 121 across 20 domains after the owner requirement records (APP domain added for SBC). Every
requirement not classified `not-applicable` maps to at least one control,
except these, deliberately:

| Requirement | Why no control |
|---|---|
| REQ-GLBA-019, REQ-PENTEST-012 | Safeguards Rule small-institution exemption (< 5,000 consumers): an exemption, not an obligation; tracked so its loss is noticed |
| REQ-PENTEST-009 | NYDFS limited exemption; NYDFS applicability undetermined (OQ-216, OQ-301) |
| REQ-TPRM-008 | Agencies' illustrative resource, not a requirement |

Every policy and policy section is implemented by at least one control;
top-level policies inherit their sections' controls.

Distribution of targets (after owner decisions and owner requirement records SBC, FIN-BC, PEC and AIG-BC/VAL-BC of 2026-11-25; 121 controls): L4 61,
L3 31, L2 12, L1 6, L0 6, L5 5. L5 controls: dormant
accounts, offboarding, entitlement drift in non-production, configuration
drift on reversible settings, and routine retention expiry (irreversible,
permitted with legal-hold checks and bounded batches under owner decision
DF-ASR-2026-0D09; evidence records are excluded and treated as evidence
destruction under the elevated safeguards of 0D09, repository
interpretation pending owner confirmation). Judgment controls remain L0–L2 by design.

The catalog CSV is authoritative and is curated directly.

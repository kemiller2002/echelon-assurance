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
| `Tested` | Effectiveness tested by a validation procedure | `VAL-` result with independence level `internal-independent` or higher; automated or self-assessment results alone never support `Tested` |
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
authoritative. 104 controls across 19 domains at B7; 123 across 20 domains after the owner requirement records (APP domain added for SBC). Every
requirement not classified `not-applicable` maps to at least one control,
except these, deliberately:

| Requirement | Why no control |
|---|---|
| REQ-GLBA-019, REQ-PENTEST-012 | Safeguards Rule small-institution exemption (< 5,000 consumers): an exemption, not an obligation; tracked so its loss is noticed |
| REQ-PENTEST-009 | NYDFS limited exemption; NYDFS currently not applicable |
| REQ-TPRM-008 | Agencies' illustrative resource, not a requirement |

Every policy and policy section is implemented by at least one control;
top-level policies inherit their sections' controls.

Distribution of targets (after owner decisions and owner requirement records through the 2026-11-25 reconciliation; 123 controls): L4 63,
L3 32, L2 12, L1 6, L0 6, L5 4. L5 controls (bounded reversible remediation only,
reconciliation finding 8): dormant accounts, offboarding, entitlement drift in
non-production, configuration drift on reversible settings. Routine retention
expiry is L4 enforcement with legal-hold checks (owner decision 0D09). Judgment controls remain L0–L2 by design.

The catalog CSV is authoritative and is curated directly.

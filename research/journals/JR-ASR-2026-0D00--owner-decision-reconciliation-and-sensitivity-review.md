---
id: JR-ASR-2026-0D00
title: Reconciliation with owner-approved foundations and pre-merge sensitivity review
status: complete
created: 2026-11-25
---

# Reconciliation with owner decisions (2026-11-25)

Source: `docs/decisions/owner-approved-foundations-2026-11-25.md`, committed by
the owner to this branch (commit 1b0a086). Registered as DF-ASR-2026-0D01..0D0E
(accepted). Proposals DF-ASR-2026-3A1E and DF-ASR-2026-7B20 marked superseded.

## Changes made

| Decision | Reconciled in |
|---|---|
| 1 Operating model | Open questions annotated (OQ-753, OQ-756, OQ-102 stay open: legal structure unconfirmed) |
| 2 SOC 1 + SOC 2, all five categories | REQ-TSC A/C/PI/P and REQ-SOC1 rows → `framework-selected` as design target; CTL-DAT-003 applicability; soc applicability addendum; economics SOC 2 fee range raised (assumption); exec findings |
| 3 GLBA baseline | No change needed (already the recommendation); OQ-200..203 annotated, legal applicability left undetermined |
| 4 Approval model | POL-01.07, POL-11.03, ai-governance §4, CTL-GOV-009, CTL-SDL-009; economics line renamed to fractional human review |
| 5 Disclosure | SECURITY-AND-CONFIDENTIALITY.md rewritten; roadmap P0; exec findings |
| 6 Praxis and F# | Roadmap P0; OQ-708 answered; migration of Python tools recorded as work item |
| 7 Release controls | CTL-SDL-002 note; integration matrix §5 |
| 8 Agent identity | CTL-SDL-009, POL-11.03 |
| 9 Retention automation | CTL-DAT-002 back to L5 with legal-hold check and bounded batches; POL-05.03; POL-11.06; automation matrix L5 rule (reversibility not universal); controls README |
| 10 Evidence storage | CTL-ASR-002; integration matrix; proposed changes |
| 11 Folio | SYS-FOLIO on CTL-ASR-010 and CTL-CUS-003 |
| 12 Strata | Integration matrix §5 |
| 13 Vigila | SYS-VIGILA restored on CTL-LOG-001/003 and CTL-ASR-004; heartbeat independently operated |
| 14 Lifecycle cost | Automation matrix rule ordering; OQ-802/803 answered as design direction |

Not resolved by these decisions, deliberately left open: legal applicability,
auditor acceptance, actual GitHub/cloud configuration, staffing, operating
effectiveness.

## Pre-merge sensitivity review

Method: full-repository pattern scan for secrets (validator patterns), email
addresses, personal names, IP addresses, cloud account identifiers,
credential assignments, private keys, internal hostnames; targeted review of
content derived from the two private repositories (Organization
Administration, Clarity framework); review of vulnerability-like language in
integration, financial, validation, architecture and AI governance documents.

Findings:
- No secrets, credentials, personal data, production evidence or live
  vulnerability details.
- Repository names and commit hashes of Echelon repositories are public.
- From private sources: Echelon Foundry's legal form (Indiana LLC, public
  registry information) and a high-level governance observation; no
  confidential documents reproduced. Retained as sanitized results under
  decision 5.
- Design-level gaps of Echelon systems (self-reported provenance, disabled
  verified completion, missing external anchoring) are already disclosed in
  those systems' own documentation; retained as sanitized results. No
  exploitation detail is present.

## Owner by-construction requirements (commits b0bcd6a, 30d05ab)

- Registered SBC-001..018 as REQ-SBC-001..018 and FIN-BC-001..024 as
  REQ-FINBC-001..024 (owner IDs in `framework_ref`; classification
  `voluntary` = owner-mandated internal requirement, not externally imposed).
- Mapped to existing controls first; added APP domain and CTL-APP-001..010,
  plus CTL-FIN-015..017 where no control covered the requirement. All at
  `Designed`.
- Annotated OQ-104/105/106/115/116/204–213/757/758/762/763 with the owner's
  proposed dispositions; all remain open pending external confirmation.
- Challenge review: `requirements/challenge-review.md` (bypass paths, cost,
  feasibility, unintended effects). Key finding: several requirements assume
  capabilities not observed in the assessed repositories (Strata runtime
  adapter, Forma safe rendering, bundle inventory, authenticated provenance).
- POL-06 and POL-12 reference the records; roadmap P1 sequencing follows the
  owner's priority order; economics notes the requirements are product
  engineering common to every scenario.

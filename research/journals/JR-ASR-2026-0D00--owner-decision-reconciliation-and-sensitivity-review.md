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

## F# migration of register tooling (owner decision 0D06)

- Installed the .NET 10 SDK from Ubuntu packages (10.0.115).
- New solution `Echelon.Assurance.sln`: `src/Echelon.Assurance.Registers` (pure:
  Csv, Schema, Validation, Merge, Roadmap, Tco), `src/Echelon.Assurance.Cli`
  (`assurance validate | merge [--check] | roadmap | tco [--rate r]`; the only
  code with effects), `tests/Echelon.Assurance.Tests` (xUnit + FsCheck, 33 tests,
  including repository acceptance tests for traceability rules).
- **Parity verified** against the retired Python tools before removal:
  identical `validate` output on the repository and on a deliberately
  corrupted copy (duplicate ID, dangling reference, planted fake token);
  identical `merge --check` output; identical roadmap body; identical TCO
  markdown and CSV at discount rates 0 and 0.08.
- **Mutation check**: disabling all per-register checks inside `validate`
  initially *survived* the suite (unit tests covered the check functions, not
  their composition). Added composition tests; both mutants (per-register
  checks dropped, secret scan dropped) now fail the suite; restored code passes.
- Fantomas formatting enforced; CI now builds, tests (fails on an empty run),
  validates, checks fragment drift and fails if generated views are stale.
- Removed: tools/validate_registers.py, merge_fragments.py, render_roadmap.py,
  schema.py, build_control_catalog.py (retired seed), economics/tco_model.py,
  and an accidentally committed `__pycache__` file.
- Praxis CLI itself is still not installed in this repository.

## Owner record: executable policy and continuous evidence (commit acdbf81)

- Registered PEC-001..028 as REQ-PEC-001..028 (owner IDs in `framework_ref`).
- Mapped to existing controls; new controls only for gaps: CTL-GOV-014
  (executable policy model with classified obligations), CTL-GOV-015 (human
  obligation tracking), CTL-ASR-011 (evidence freshness; unknown is not pass).
- Checked the existing policy-as-code specification and evidence architecture
  first, as instructed: both already preferred F# deciders and flagged
  crypto-shredding as a legal question. Tightened the architecture to state
  crypto-shredding is not assumed to satisfy legal erasure (PEC-021) and that
  public witness networks and qualified timestamps are deferred.
- Annotated OQ-108/312/500–510/703/705/751/758/759/762/765/802–805/804 with
  the owner's proposed dispositions; auditor reliance and legal questions stay open.

## Owner record: AI governance and independent validation (commit f2d9ea9)

- Registered AIG-BC-001..018 as REQ-AIGBC-* and VAL-BC-001..014 as REQ-VALBC-*.
- All but one mapped to existing controls; new CTL-AI-010 (AI involvement
  disclosure and correction path) for AIG-BC-016.
- Owner A0–A4 authority model reconciled with Unbound's A0–A5 tiers in the AI
  governance model (A3 clarified; A5 = hard prohibitions) and added to POL-11.
- RSK-001: residual prompt-injection risk recorded as explicitly not accepted.
- Annotated OQ-107/113/510/600–616/701–706/763/765/800/803 with owner
  dispositions; legal applicability, auditor reliance, staffing and quotes stay open.

## Owner record: continuous security testing (commit e983dd6)

- Registered CST-001..026 as REQ-CST-*; mapped to existing VUL, SDL, CFG, APP,
  TEN, FIN, AI and ASR controls; new CTL-VUL-005 (authorized, isolated active
  testing) and CTL-VUL-006 (risk-based orchestration with validated harnesses).
- Pen-test strategy, roadmap and economics updated; OQ-300..313, 604, 612
  annotated with owner dispositions (all still open where external answers are needed).

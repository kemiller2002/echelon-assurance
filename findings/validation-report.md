---
id: ASR-VAL-REPORT-001
title: Validation report — assurance consolidation
status: final for this initiative (owner decisions recorded: DF-ASR-2026-0D0F, DF-ASR-2026-0D10)
created: 2026-11-25
journal: JR-ASR-2026-C001
---

# Validation report

## 1. Repository validation (executed)

The following commands were run locally on the final head with the .NET 10 SDK
(10.0.404). The same steps run in CI (`.github/workflows/assurance-validation.yml`).
CI passed on c346fa7 and later pushed commits whose checks have completed, except 5ca1af2; the final head is validated locally as recorded in the completion report. CI failed on two intermediate commits. The first was ba8c2aa (an unquoted comma in two OT-02 rows broke CSV parsing; 5 of 93 tests and the validator failed), which was fixed sixteen seconds later by c346fa7. CI also failed on 5ca1af2 (REQ-STATE-005 had no mapped control after its reclassification; the local gate had not stopped the commit), fixed by 54b2913; CI history after c346fa7 is recorded in `findings/reviews/RV-ASR-2026-11-26.md`.

| Check | Command | Result |
|---|---|---|
| Formatting | `dotnet fantomas --check src tests` | pass |
| Build | `dotnet build Echelon.Assurance.sln -c Release` | 0 errors, 0 warnings |
| Unit and repository tests | `dotnet test` | 64 passed at consolidation (33 before this initiative); 95 passed after the final reconciliation (2026-11-26, section 7) |
| Register validation and traceability | `assurance validate .` | `OK: all registers valid` |
| Fragment merge | `assurance merge --check .` | no warnings |
| Generated views current | `assurance roadmap`, `trace`, `econ`, `tco`, then `git diff --exit-code` | current |

**What `assurance validate` enforces**, beyond identifiers, vocabularies,
references and secret patterns:

- **Baseline content.** Every live baseline requirement has its required fields. Source-to-baseline dispositions are bidirectional, and every owner requirement registered in `regulatory/requirements.csv` is dispositioned.
- **Control support and alignment.**
  - Every control is supported by a baseline requirement and has an objective, owner, validation and evidence.
  - A requirement's policies, validation procedures and evidence types belong to its controls.
- **Status ceilings.**
  - No requirement claims a status above its controls.
  - `Tested` requires a validation procedure at `technically-separated` level or above (protected deterministic verification, VAL-040); multiple-agent review (VAL-041) never supports `Tested` (owner direction DF-ASR-2026-0D0F; corrected 2026-11-26).
  - Capability status `verified-implemented` requires executed tests.
- **Coverage.** Every tenet is realized, every question has a disposition and next action, and question status agrees with that disposition.
- **Roadmap.**
  - Every live requirement has a work item, and its phase is the earliest work-item phase.
  - Its launch gate is met by the latest work-item phase.
  - The backlog has no later-phase dependencies and no cycles.
- **Customer answers.**
  - No answer claims `operating` above its controls.
  - Public answers do not cite non-operating controls.
  - No answer describes or cites deferred mechanisms.

These checks are structural. They cannot prove that a link is semantically
right, which is why the challenge passes reviewed content.

## 2. Successive challenge passes

*(Historical: section 2 and sections 3–3d describe the challenge passes before the final reconciliation of 2026-11-26; the later review A–F and adversarial rounds are in `findings/reviews/RV-ASR-2026-11-26.md`.)*

Six review rounds were run by separate agent contexts (rounds 1–4 on the consolidation, rounds 5–6 on the owner-direction reconciliation). The agents were given
the repository and the earlier findings, not the coordinator's conclusions. Each
round actively tried to disprove the previous conclusions. All findings are in
`findings/challenge-passes/`.

| Round | Scope | Findings | Material | Outcome |
|---|---|---|---|---|
| 1a | Passes 1, 2, 9, 13, 14, 15: completeness, consistency, simplicity, readiness, traceability, assumptions | 29 | 18 | Fixed in 8bec065 |
| 1b | Passes 3–8, 10–12: regulatory, security, financial, AI, evidence, independence, economics, organization, customer | 30 | 15 | Fixed in 8bec065 |
| 2 | Verification of round 1, plus new defects | 40 | 21 fixed, 9 partially fixed, 4 new | Fixed in 49883a3 |
| 3 | Verification of round 2, plus new defects | 26 | 4 fixed, 5 open, 3 new | Fixed in e49934f |
| 4 | Stopping check | 11 | 4 fixed, 1 partially fixed, 1 new | Both remaining items were unescalated owner decisions, now ODP-11 and ODP-12 |

**The most consequential corrections the passes forced:**

1. **Economics conclusion reversed twice.** The first run favored targeted automation, then the hybrid. Once maintenance, non-reliance, mandatory gates and reviewer growth were charged to every alternative, the evidence supports minimum viable assurance. Internal automation shows no clear cash saving at one or two products. Evidence-collection automation pays only with auditor reliance and scale. *(Superseded for decisions by the ODP-09 recalculation (DF-ASR-2026-0D0F; owner brief of 2026-11-26): costs are now separated by evidence purpose, no product-count threshold applies, and statements that evidence automation pays only with auditor reliance and scale no longer hold; see `economics/v2/README.md`.)*
2. **FinCEN analysis.** Limb (i)(B), "any other person engaged in the transfer of funds", had been omitted. A legal reading was stated as fact. Legal engagement was decided under ODP-12: none now; internal research and prepared questions (DF-ASR-2026-0D0F).
3. **Evidence storage.** The replacement for the custom ledger was not actually immutable in the interim. A compliance-mode retention lock now applies from the first production evidence.
4. **Independence wording.** Owner-administered reconciliation, heartbeat and alerts were called "independent". They are separately credentialed and detect failure, not owner action.
5. **Payment dispatch.** Unbound could have held credentials sufficient to move money. Activation is now blocked for any channel without a sponsor-side control.
6. **Launch gates.** Deriving phases from work items had removed "before production" semantics. A `launch_gate` per requirement, checked at work completion, restored them.
7. **Customer answers.** Answers overstated independent review and described the removed ledger. Public answers disclosed designed-but-not-operating controls.
8. **Semantic traceability.** Area-level bulk links for policies, validation and evidence were replaced by per-control derivation, and the validator now enforces it.

## 3. Stopping rationale

Passes stopped after round 4. The trend in new material findings was 33, then
4, then 3, then 1. The last round found no new design or factual defect. Its
material items were decisions that only the owner can make: a pre-launch
penetration test, and spending on counsel. Both were escalated then and are
now decided by owner direction DF-ASR-2026-0D0F (ODP-11, ODP-12): no external
test or legal engagement now; reassessment on recorded triggers.
*(Historical stopping rationale for round 4; superseded by section 3a and
section 7.)*

The remaining minor items are recorded in the round-4 file. They are wording
and count slips; those found were corrected. The mission's stopping condition is
met: remaining issues require owner decisions (twelve ODP items), external
evidence (auditor, legal resolution, bank, insurer and customer answers in
`registers/question-dispositions.csv`), or implementation (`roadmap/backlog.csv`).

**Independence statement.** All rounds were performed by AI agents controlled
by Echelon. This is technically separated self-review, not organizational
independence or external assurance (BR-VAL-001, BR-ASR-002).

## 3a. Owner direction reconciliation (after round 4)

The owner then pushed `docs/decisions/owner-review-direction-2026-11-25.md`, registered as DF-ASR-2026-0D0F (accepted). It was reconciled as follows:

- **BR-GOV-004.** Self-approval of material exceptions is not authorized. This replaces the interim self-approval rule proposed in round 1.
- **BR-VUL-012, BR-VAL-003 and BR-FIN-020.** External testing and counsel are deferred. Activation gates are retained, and production with unresolved material legal risk is not authorized.
- **BR-VAL-004.** Added: the multi-agent review protocol.
- **BR-GOV-017 and BR-IAM-002.** Updated.
- **Backlog.** WI-EXT-02 is withdrawn. WI-EXT-03 and WI-EXT-04 are retitled to question preparation and reassessment. WI-ASR-14 is added.
- **Economics.** Scenario S8 is added, with an excluded-cost-class mechanism and a unit test. C005 is superseded.
- **Questions.** External-dependency next actions are annotated. No question is marked resolved.
- **Risks.** RSK-064 added.
- **Customer answers.** CAQ-005 and CAQ-049 updated.

Validation was rerun after reconciliation (64 tests at that point; 95 after the final reconciliation, section 7) and all registers validate. Challenge round 5 reviewed the reconciliation (section 3c).

## 3b. Owner remaining approvals and Praxis installation

The owner then pushed `docs/decisions/owner-remaining-approvals-2026-11-25.md`, registered as DF-ASR-2026-0D10. It was reconciled as follows:

- **Decisions.** C001 (in principle), C002, C003, C004 and C006 are accepted.
- **Tenets.** TNT-17, evidence by construction, is added and realized by six baseline requirements.
- **Baseline.** BR-GOV-013, BR-HR-002, BR-LOG-001, BR-ASR-008, BR-APP-001, BR-AI-004 and BR-SDL-001 are updated.
- **Questions.** Twelve questions were answered by owner decision at that point (OQ-208, 304, 505, 509, 611, 616, 760, 800, 802, 803, 806, 809); OQ-208 and OQ-616 were later reopened (round F4, dispositions register), which holds 16 resolved-owner-decision rows. Others are reclassified to implementation or external dependencies. No question blocks requirements finalization.
- **Backlog.** WI-ASR-01 and WI-ASR-02 are done (WI-STRATA-04 was later reopened, round B8); WI-VIGILA-01 and WI-ASR-03 are retitled.

**Praxis.** Praxis 3.25.0 was installed with `praxis init`, from the checksum-verified native release.

- `praxis verify` passes.
- `praxis validate` passes, including with `PRAXIS_BASE_REF` set to `main` as CI runs it.
- `praxis registry check` reports current registries.
- The consolidation runs as Praxis work item WI-ASR-CONSOLIDATION.
- Provenance is recorded for the consolidation journal. Journals created before installation are acknowledged as origin-unrecorded, with git history as the record.
- Verified completion (`requireVerifiedCompletion` with pinned verifier keys) is not enabled in this installation; enabling it is WI-PRAXIS-03. The work item is closed with `praxis work complete` and recorded research evidence in the final commit, not with a verifier-signed receipt (section 3d).

## 3c. Challenge round 5 (owner-direction reconciliation)

The owner's release gate requires adversarial challenge passes. Round 5 reviewed the reconciliation of DF-ASR-2026-0D0F and 0D10 (`findings/challenge-passes/round5-owner-reconciliation.csv`; the reviewing agent numbered its findings R6-xxx). It found 19 findings, 8 of them material.

**Material findings:**

- the roadmap narrative, operating model and executive findings still planned a contracted reviewer, counsel memo or pre-launch pen test;
- S8 called examinations and pen tests "launch-stage obligations";
- the review-protocol deliverable did not exist;
- the work-item state was overstated;
- no challenge round covered the reconciliation;
- the protected-reporting role was "defined" without any defined authority or procedure.

**Fixes (in b755a01):**

- text aligned with the owner directions where the round found it (round 6 found remaining instances, fixed afterwards);
- `methodology/review-protocol.md` created;
- role authority and procedures added to `governance/operating-model.md`;
- S8 provisions relabelled;
- draft POL-07 aligned;
- OQ-208 kept open as a customer dependency;
- OQ-607 restored to its authority-matrix scope;
- next actions rewritten for six questions (the rest were completed in round 6);
- decision qualifiers added.

Round 6 verified these fixes (section 3d).

## 3d. Challenge round 6 (verification) and closure

Round 6 (`findings/challenge-passes/round6-verification.csv`, agent IDs R7-xxx) re-checked the 19 round-5 findings: 12 fixed, 6 partially fixed, 1 not fixed. It also found 3 new findings, 2 of them material:

- the executive findings' question counts were stale after OQ-208 was reopened;
- draft POL-01 still planned contracted review.

All remaining items were fixed in the final commits:

- the work-item state statement is corrected, and the item is completed in Praxis;
- dispositions for every round-5 and round-6 finding are in `findings/challenge-passes/round5-6-dispositions.csv`;
- control counts use the computed VAL-025 figure;
- question statistics are recomputed from the registers;
- POL-01 is aligned;
- the naming deadline is labelled as a proposed interpretation;
- every counsel-package next action is made conditional on owner reassessment;
- OQ-311 has no quotes now;
- BR-GOV-011 and BR-GOV-017 carry the reassessment qualifier;
- stale counts are corrected.

**Stopping rationale.** Across rounds 5 and 6, material findings fell from 8 to 2 new ones plus 3 incomplete fixes. All were textual alignment or count errors, not design or factual-law defects. Every finding now has a recorded disposition. No further round was run at that time (later rounds: `findings/reviews/RV-ASR-2026-11-26.md`): the remaining risks need owner decisions, external evidence or implementation, which meets the mission's stopping condition.

## 4. Completion criteria (mission section 23)

| Criterion | Status | Evidence |
|---|---|---|
| Every original requirement has a documented disposition | Met | 222 rows (2026-11-26; 208 at consolidation) in `requirements/source-dispositions.csv`; validator enforces bidirectional links |
| Every control has an objective and validation strategy | Met | Validator: objective, owner, validation and evidence required for all 121 controls |
| Every policy has traceable enforcement or accountable human obligations | Met | Repository test: every policy section maps to controls; baseline policies derive from controls; `enforcement_class` distinguishes human judgment |
| Every outstanding question has a disposition and next action | Met | 115 rows in `registers/question-dispositions.csv`; validator enforces it |
| Every material contradiction resolved or escalated | Met | Decisions C001–C004 and C006 (C005 superseded by owner direction); ODP-01 to ODP-12; owner-record conflicts (Vigila, Strata) escalated as ODP-06 and ODP-07 |
| Proposed capabilities distinguished from verified | Met | `integration/capability-verification.csv` with basis and status; validator blocks inflation |
| Economic model reproducible and internally consistent | Met, with hypothetical inputs | `assurance econ`; CI currency check; 17 unit tests; no quotes exist |
| Roadmap dependency-driven | Met | Validator: phase order, cycles, coverage, launch gates |
| All applicable repository validation checks pass | Met for the assurance validator, tests and Praxis validate/registry check; **verified completion not met** (WI-ASR-CONSOLIDATION was closed by plain completion; WI-ASR-FINAL is the final work item; see section 7) | Section 1; section 3b; `findings/reviews/RV-ASR-2026-11-26.md` |
| No unsupported claims of compliance, certification or effectiveness | Met to the extent reviewed | All controls `Designed`; customer answers capped; review A–F and the adversarial rounds (`findings/reviews/RV-ASR-2026-11-26.md`) |
| Concise owner package limited to genuine owner decisions | Met | `docs/decisions/owner-decision-package-2026-11-25.md` (12 items, all decided by DF-ASR-2026-0D0F and 0D10) |

## 5. Conflicts with existing tooling and standards (documented, not disabled)

- **`main` not protected.** Praxis 3.25.0 is installed and CI runs it, but without required checks CI is advisory (RSK-060). Fixing this is an owner action (WI-ASR-03, ODP-10).
- **Praxis and Tutela test suites are console programs.** `dotnet test` on them runs nothing and still exits 0, so they must be run with `dotnet run`. This repository's CI guards against an empty test run.
- **Strata and Dokimos target .NET 8.** They ran here only with roll-forward. The Echelon F#-first standard is met, but toolchain alignment is a proposal for those repositories.
- **Tutela's Python conformance oracle** conflicts with the F#-first standard. It expires 2027-01-31 (WI-TUTELA-03).
- **Python was used only for scratch authoring.** It was not committed. All repository tooling is F#, per owner decision 0D06.

## 6. Limits of this validation

- Capability verification used depth-1 clones. Only five systems had tests executed.
- Regulatory research read primary text for FinCEN, the FTC Safeguards Rule, 12 U.S.C. 1867 and the 2023 interagency guidance. Nacha Rules, licensed frameworks (SIG, SOC criteria PDFs) and state statutes were read only in summary.
- No auditor, counsel, bank, insurer or customer has reviewed anything.
- Economic inputs are hypothetical or vendor claims.

## 7. Final reconciliation (2026-11-26)

Commits after `fd6153a` reconciled the owner directions of 2026-11-25 and
were reviewed by six separately initialized agents (A–F), with findings
reconciled on evidence (`findings/reviews/RV-ASR-2026-11-26.md`). The
validator was hardened against ten corruption experiments, and regression tests
corrupt the committed registers and require failure. Counts at the end of this
reconciliation are in `context/CURRENT-STATE.md`; adversarial rounds and their
stopping rationale are in the review record.

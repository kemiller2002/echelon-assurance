---
id: ASR-VAL-REPORT-001
title: Validation report — assurance consolidation
status: final for this initiative (owner decisions outstanding)
created: 2026-11-25
journal: JR-ASR-2026-C001
---

# Validation report

## 1. Repository validation (executed)

The following commands were run locally on the final head with the .NET 10 SDK
(10.0.404). The same steps run in CI (`.github/workflows/assurance-validation.yml`).
CI passed on every pushed commit of PR #3.

| Check | Command | Result |
|---|---|---|
| Formatting | `dotnet fantomas --check src tests` | pass |
| Build | `dotnet build Echelon.Assurance.sln -c Release` | 0 errors, 0 warnings |
| Unit and repository tests | `dotnet test` | 64 passed, 0 failed (33 before this initiative) |
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
  - `Tested` requires a validation procedure at internal-independent level or above.
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

Four review rounds were run by separate agent contexts. The agents were given
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

1. **Economics conclusion reversed twice.** The first run favored targeted automation, then the hybrid. Once maintenance, non-reliance, mandatory gates and reviewer growth were charged to every alternative, the evidence supports minimum viable assurance. Internal automation shows no clear cash saving at one or two products. Evidence-collection automation pays only with auditor reliance and scale.
2. **FinCEN analysis.** Limb (i)(B), "any other person engaged in the transfer of funds", had been omitted. A legal reading was stated as fact. Counsel is now a pre-launch owner decision (ODP-12).
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
penetration test, and spending on counsel. Both are now escalated.

The remaining minor items are recorded in the round-4 file. They are wording
and count slips; those found were corrected. The mission's stopping condition is
met: remaining issues require owner decisions (twelve ODP items), external
evidence (auditor, counsel, bank, insurer and customer answers in
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

Validation was rerun after reconciliation: 64 tests pass and all registers validate. No further challenge round was run on these edits: they implement an explicit owner decision and change no factual claim beyond it.

## 3b. Owner remaining approvals and Praxis installation

The owner then pushed `docs/decisions/owner-remaining-approvals-2026-11-25.md`, registered as DF-ASR-2026-0D10. It was reconciled as follows:

- **Decisions.** C001 (in principle), C002, C003, C004 and C006 are accepted.
- **Tenets.** TNT-17, evidence by construction, is added and realized by six baseline requirements.
- **Baseline.** BR-GOV-013, BR-HR-002, BR-LOG-001, BR-ASR-008, BR-APP-001, BR-AI-004 and BR-SDL-001 are updated.
- **Questions.** Twelve questions are answered by owner decision (OQ-208, 304, 505, 509, 611, 616, 760, 800, 802, 803, 806, 809). Others are reclassified to implementation or external dependencies. No question blocks requirements finalization.
- **Backlog.** WI-ASR-01, WI-ASR-02 and WI-STRATA-04 are done; WI-VIGILA-01 and WI-ASR-03 are retitled.

**Praxis.** Praxis 3.25.0 was installed with `praxis init`, from the checksum-verified native release.

- `praxis verify` passes.
- `praxis validate` passes, including with `PRAXIS_BASE_REF` set to `main` as CI runs it.
- `praxis registry check` reports current registries.
- The consolidation runs as Praxis work item WI-ASR-CONSOLIDATION.
- Provenance is recorded for the consolidation journal. Journals created before installation are acknowledged as origin-unrecorded, with git history as the record.
- Verified completion (`requireVerifiedCompletion` with pinned verifier keys) is not enabled in this installation; enabling it is WI-PRAXIS-03. The work item is therefore completed with evidence, not with a verifier-signed receipt.

## 4. Completion criteria (mission section 23)

| Criterion | Status | Evidence |
|---|---|---|
| Every original requirement has a documented disposition | Met | 208 rows in `requirements/source-dispositions.csv`; validator enforces bidirectional links |
| Every control has an objective and validation strategy | Met | Validator: objective, owner, validation and evidence required for all 121 controls |
| Every policy has traceable enforcement or accountable human obligations | Met | Repository test: every policy section maps to controls; baseline policies derive from controls; `enforcement_class` distinguishes human judgment |
| Every outstanding question has a disposition and next action | Met | 115 rows in `registers/question-dispositions.csv`; validator enforces it |
| Every material contradiction resolved or escalated | Met | Decisions C001–C006; ODP-01 to ODP-12; owner-record conflicts (Vigila, Strata) escalated as ODP-06 and ODP-07 |
| Proposed capabilities distinguished from verified | Met | `integration/capability-verification.csv` with basis and status; validator blocks inflation |
| Economic model reproducible and internally consistent | Met, with hypothetical inputs | `assurance econ`; CI currency check; 12 unit tests; no quotes exist |
| Roadmap dependency-driven | Met | Validator: phase order, cycles, coverage, launch gates |
| All applicable repository validation checks pass | Met | Section 1; Praxis verify, validate and registry check (section 3b) |
| No unsupported claims of compliance, certification or effectiveness | Met to the extent reviewed | All controls `Designed`; customer answers capped; four review rounds |
| Concise owner package limited to genuine owner decisions | Met | `docs/decisions/owner-decision-package-2026-11-25.md` (12 items, all decided by DF-ASR-2026-0D0F and 0D10) |

## 5. Conflicts with existing tooling and standards (documented, not disabled)

- **Praxis not installed here, and `main` not protected.** CI is therefore advisory (RSK-060). Fixing this is an owner action (WI-ASR-03, ODP-10).
- **Praxis and Tutela test suites are console programs.** `dotnet test` on them runs nothing and still exits 0, so they must be run with `dotnet run`. This repository's CI guards against an empty test run.
- **Strata and Dokimos target .NET 8.** They ran here only with roll-forward. The Echelon F#-first standard is met, but toolchain alignment is a proposal for those repositories.
- **Tutela's Python conformance oracle** conflicts with the F#-first standard. It expires 2027-01-31 (WI-TUTELA-03).
- **Python was used only for scratch authoring.** It was not committed. All repository tooling is F#, per owner decision 0D06.

## 6. Limits of this validation

- Capability verification used depth-1 clones. Only five systems had tests executed.
- Regulatory research read primary text for FinCEN, the FTC Safeguards Rule, 12 U.S.C. 1867 and the 2023 interagency guidance. Nacha Rules, licensed frameworks (SIG, SOC criteria PDFs) and state statutes were read only in summary.
- No auditor, counsel, bank, insurer or customer has reviewed anything.
- Economic inputs are hypothetical or vendor claims.

---
id: ASR-FIND-CLE-001
title: Clarity result and EDF diagnosis for the assurance program
status: draft
version: 0.1.0
created: 2026-11-25
method_refs: [clarity-framework docs/templates/clarity-result-template.md, echelon-diagnostic-framework docs/edf/specification.md (v0.3)]
---

# Clarity result and EDF diagnosis

**Status of this application.** Both frameworks were applied by the research
program to its own conclusions. This is not an independent Clarity
assessment or an EDF review by an assessor outside the program. EDF describes
itself as a working grammar whose incremental value is not yet validated
(EDF README, "Evidence maturity"); the diagnosis below is offered as
structure to challenge, not as proof.

## Part A — EDF v0.3 diagnosis: why conventional compliance is costly and weak

1. **System context.** A small, AI-native organization (one product,
   Unbound, pre-production) that will process sponsor fund accounting and
   donor data, rely on GitHub and a cloud provider, and sell to sponsors and
   institutions whose auditors and banks perform due diligence.
2. **Outcome.** Credible, low-recurring-cost assurance that sponsors'
   auditors, banks and customers rely on, without misrepresentation.
3. **Manifestations (in comparable organizations; ours is prospective).**
   Recurring pre-audit evidence scrambles; screenshot-based evidence; audit
   findings on access reviews and change tickets; platform subscriptions that
   leave most work manual; reports that cover security but not the financial
   controls customers' auditors need; claims that outrun evidence.
4. **Origin network.**
   - O1: Evidence is produced *after* the event, by people, for the audit,
     rather than *by* the event.
   - O2: Controls are procedural (someone must remember) rather than
     structural (the system cannot do otherwise).
   - O3: Populations are not provably complete, so auditors sample and test
     IPE manually.
   - O4: Separate programs per framework duplicate work.
   - O5: Independence is scarce in small teams, so self-assessment is
     dressed up as assurance (compliance theater).
   - O6: Financial correctness is verified by sampling rather than by
     invariants and independent reconciliation.
5. **Propagation.** O1+O3 → high fieldwork hours and exception findings →
   audit cost and remediation cycles. O2 → recurring failures (missed
   reviews, unreviewed emergency changes). O4 → multiplied questionnaire and
   mapping effort. O5 → customer and auditor distrust, or worse, false
   assurance. O6 → SOC 1 effort and residual misstatement risk.
6. **Evidence.** External costs and the limits of platforms: `economics/`,
   `vendors/`. Auditor procedures (sampling, IPE): `soc/`. Echelon's current
   state (strong designs, nothing operating, key gates not enabled):
   `integration/`, `financial/unbound-existing-assurance-inventory.md`.
   Confidence: `medium` for O1–O4 and O6 (well documented); `medium` for O5
   as a general pattern, `high` as a specific risk for Echelon given headcount.
7. **Control points.**
   - CP1 (O1, O3): evidence envelope at source + completeness
     reconciliation + anchoring (CTL-ASR-001..004).
   - CP2 (O2): preventive enforcement in rulesets, IdP, org policies,
     application authorization (L4 controls).
   - CP3 (O4): one control catalog, requirements mapped to it (CTL-GOV-005).
   - CP4 (O5): explicit independence levels, `Tested` requires
     internal-independent, contracted independent reviewer, external
     validation of the assurance machinery (CTL-ASR-007/008).
   - CP5 (O6): write-time invariants + two-way independent reconciliation
     (CTL-FIN-001..005).
8. **Ranking.** CP2 (cheapest, immediate, removes whole failure classes) >
   CP5 (core of SOC 1 value; must exist for production anyway) > CP4 (without
   it nothing else is credible) > CP1 (largest audit-cost lever, but its value
   depends on auditor reliance) > CP3 (useful, low cost, lower impact at one
   product).
9. **Diagnostic sufficiency.** Sufficient to set the roadmap; **insufficient**
   to predict audit cost savings, because the decisive variable (auditor
   reliance on system-generated evidence, OQ-500/601/759) is unobserved.

## Part B — Clarity result for the program's central recommendations

**Executive understanding.** Unbound will very likely need SOC 1 Type 2 as
well as SOC 2 Type 2 if it operates sponsors' fund ledgers for separate
sponsor entities. The most capable route at roughly the cost of a commercial platform is a hybrid: build only the
assurance capabilities that existing Echelon systems nearly provide, enforce
controls structurally, and buy independence (pen test, readiness review,
external validation, attestation).

**Clarity snapshot.** Most important now: who the customer and the service
organization legally are (OQ-753/756), and whether the first auditor will
rely on system-generated evidence (OQ-500/601/759).

**Current understanding.** Standards text (AT-C 320, AU-C 402/9402) supports
SOC 1 relevance; no law requires either report; banks and sponsors' auditors
drive demand. GLBA status is contested. Existing Echelon systems hold strong
but disabled or unoperated assurance capabilities.

**Evidence landscape.** Supporting: AICPA standards text read by the SOC
batch; Unbound's own SOC 1 tagging; vendor and auditor price evidence;
system code read in 31 repositories. Challenging: TSC and description
criteria were read only in summary (login wall); all prices are vendor
claims; no system observed operating; no auditor consulted.

**Assumption landscape.** Sponsor and operator are separate entities;
sponsors' auditors will want SOC 1; labor rate USD 100–175/h; auditor
reliance on system evidence; a contracted independent reviewer can be
afforded; Arca can be extended into an anchored ledger.

**Calibration review.** Confidence in *what controls are needed* is
`medium-high`; in *which reports* `medium`; in *cost savings* `low`. The
roadmap commits early only to actions that are cheap and useful under every
answer (P0/P1 preventive controls, financial invariants), and defers the
build-vs-buy decision to P3. That calibration is appropriate.

**Correctability review.** Highly correctable: policies and catalog are
data; the build-vs-buy decision has an explicit exit. Least correctable:
WORM retention locks (irreversible; deferred until periods are set) and any
public claim of compliance (prohibited by CTL-GOV-012).

**Monitoring plan.** Watch: AICPA 2027 TSC/description criteria finalization;
FTC/CFPB positions on nonprofits and fintech service providers; bank partner
questionnaires; auditor feedback; platform pricing; Arca/Praxis maturity.

**Reassessment plan (reconsideration criteria).**
- If sponsor and operator are the same entity → SOC 1 relevance changes;
  re-run `soc/unbound-soc-applicability.md`.
- If the auditor will not rely on system evidence → buy a platform for the
  SaaS/cloud evidence layer.
- If counsel concludes the Echelon entity is a GLBA financial institution →
  Safeguards elements become legal, not framework-selected.
- If Unbound card payments are activated → PCI scope analysis.
- If a second product joins → re-run economics §4.

**Learning priorities.** In order of cost per uncertainty removed: owner
decisions (free); call with a sponsor's audit firm (free); bank
questionnaire request (free); counsel memo (fixed fee); CPA readiness
scoping (P3).

**Recommended next posture.** Progressive commitment: execute P0, build P1
preventive and financial controls, defer anchored ledger and platform
decisions until auditor input exists.

**Current best understanding.** The controls are knowable now and mostly
cheap; the reports are likely SOC 1 + SOC 2; the savings are plausible but
unproven and hinge on a conversation with an auditor that has not yet
happened.

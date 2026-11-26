---
id: ASR-ECON-V2-001
title: Assurance economics model v2
status: proposed model; not a budget or purchase authorization
created: 2026-11-25
requirements: [BR-GOV-015]
preserves: economics/parameters.csv and economics/generated/ (v1, unchanged, for comparison)
---

# Assurance economics model v2

**Reproduce:** `dotnet run --project src/Echelon.Assurance.Cli -- econ .`
Inputs: `activities.csv`, `alternatives.csv`, `scenarios.csv`.
Output: `generated/model.md`, `generated/results.csv`.
CI fails if the generated files are stale.

## What changed from v1, and why

| v1 issue | v2 treatment |
|---|---|
| Baseline engineering not separated | `BASELINE` lines reported separately and excluded from the incremental comparison (ECON-001) |
| Fractional independent reviewer only in automated scenarios | Included in every alternative for comparable assurance outcomes; reduced in A3, where the retainer provides some challenge (ECON-008) |
| One product only | Product count N ∈ {1, 2, 5, 10, 15}; per-product lines scale as `1 + (N−1) × scaling` (ECON-005) |
| Lump-sum labor | Activity hours × loaded rate (USD 100–175/h, hypothetical) (ECON-003) |
| Custom ledger infrastructure (TSA, witness) | Removed per DF-ASR-2026-C002 |
| Auditor rejection as one variant | Scenario dimension: lines with `condition = nonreliance` apply only when the auditor does not rely on system evidence (ECON-012) |
| No sensitivity | Maintenance ×0.7 / ×1.5 / ×2.0, slow adoption (extra products from year 3), 8% discounting (ECON-013) |
| No opportunity cost | Build hours × rate × 0.5 reported separately, never added to cash (ECON-003) |
| No marginal cost or break-even | Marginal cost of the N-th product; break-even N for each automated alternative against each conventional one (ECON-013) |

## Evidence classes

No quotes exist. External examination and test fees are **vendor claims**
carried over from v1 (SRC-ECON-001…005, SRC-PTV-029/030/036). Every hour
estimate, scaling factor and remediation figure is **hypothetical**. Each line
carries its class in `activities.csv`.

## Results (generated `generated/model.md` is authoritative)

**Revision note.** Three challenge rounds corrected biases in earlier runs:

- Maintenance and auditor non-reliance are now charged to every alternative that produces system-generated evidence.
- The fractional reviewer grows per product.
- The enforced release gates, registers and validator that owner decisions 0D06 and 0D07 require are now charged to every alternative (A4, A7 and A8 include them in their builds; the others carry explicit lines).
- A8 models the package that DF-ASR-2026-C005 recommends: minimum viable assurance plus prevention and testing automation, without evidence-collection automation.

**Base case (S0), five-year midpoints** (regenerated 2026-11-26, including the shared observability lines ACT-114..116 charged identically to every alternative):

- **At N=1 the alternatives other than A7 are close.** Midpoints run USD 1.27M (A4) to 1.54M (A1); A7 is USD 2.12M. The ranges span about 3×, so no ranking at N ≤ 2 is reliable. A4 has the lowest midpoint at N=1 and N=2, with A3 within about 3%.
- **At N=5 targeted automation (A5) has the lowest midpoint** under auditor reliance: USD 2.87M against about USD 3.09M (A3, A4) and USD 3.36M (A2). This is a base-case result that depends on reliance; it is not a decision threshold (ODP-09).
- **A7 (full internal platform)** is never the lowest midpoint at any modelled product count (at N=15, A5 is USD 6.41M against A7's 6.70M).
- **A8 costs about USD 245k more than A4 over five years at N=1** (prevention USD 192k, financial integrity USD 34k, test infrastructure about USD 19k). The model counts the cost of prevention and testing automation but deliberately not its benefit (ECON-011).

**Auditor non-reliance (S4) and the downside (S5):**

- **A4 and A3 are effectively tied** at every product count: within about 4% at N=1 and about 1% at N=15. A4 is lower up to about five products, and A3 is lower at larger scale.
- **A5, A6 and A8 never overtake A3.** Against A2, A5 breaks even at 5 products and A6 at 8.

**Slow adoption (S6)** delays but does not remove A5's advantage under reliance.

## Owner capacity

Steady-state recurring assurance labor implied by the inputs (hypothetical). It
excludes baseline product engineering (200–500 h a year per product) and build
hours:

| Alternative | N=1 | N=3 |
|---|---|---|
| A3 outsourced | 125–400 h | 230–780 h |
| A4 minimum viable | 300–810 h | 570–1,540 h |
| A8 recommended package | 395–1,070 h | 680–1,840 h |
| A5 targeted (reliance) | 315–860 h | 420–1,160 h |
| A5 targeted (non-reliance) | 515–1,360 h | 860–2,260 h |

- **Capacity.** One owner has about 1,800 working hours a year, so capacity, not cash, is likely the binding constraint.
- **Threshold.** The proposed threshold is 25% of capacity, about 450 hours a year (BR-GOV-011). The recommended package can exceed it even at one product. These tables exclude shared observability operation (ACT-115, 40–120 h a year), which adds equally to every alternative.
- **Not modelled.** Relief for capacity is an owner decision; no external consulting budget is authorized (ODP-09, TNT-18). Automation that reduces recurring hours, or deferral of lower-value work, is considered first.

## Owner direction planning case (S8, DF-ASR-2026-0D0F)

The owner directed on 2026-11-25:

- Recalculate costs and benefits **without assuming auditor reliance**.
- Purchase no external consulting or recurring independent review at the development stage.

Scenario S8 implements this:

- Non-reliance lines apply.
- Contracted reviewer (the "fractional independent reviewer" lines) and external technical validation lines are excluded.
- Examinations and penetration testing stay in as conservative cost provisions on the model's standard timing (pen test yearly from year 1, examinations from year 2). They are not commitments: the owner reassesses them against actual obligations. Removing them lowers every alternative equally and does not change the ranking.

| Five-year midpoint | N=1 | N=2 | N=5 | N=10 | N=15 |
|---|---|---|---|---|---|
| A4 minimum viable | USD 1.19M | 1.65M | 3.03M | 5.32M | 7.61M |
| A3 outsourced | 1.33M | 1.79M | 3.18M | 5.49M | 7.81M |
| A5 targeted automation | 1.50M | 1.96M | 3.33M | 5.62M | 7.91M |
| A8 recommended package | 1.44M | 1.92M | 3.38M | 5.81M | 8.24M |

(Regenerated 2026-11-26 after adding the shared observability lines ACT-114..116, ODP-06, which are identical in every alternative; the earlier figures were about USD 0.10M lower at N=1.)

**Cost.** Without auditor reliance, A4 has the lowest midpoint at every product count. A8 and A5 cost about USD 245k and 310k more than A4 over five years at one product. A5, A6 and A8 stay above A3 at every modelled product count.

**Benefit.** The benefit of reusable continuous assurance in this case is operational, not reduced audit effort:

- earlier detection of control failure;
- prevention of vulnerability classes and financial errors;
- generated customer answers and reuse across products.

None of these is monetized. Without reliance, automation does not reduce owner hours either. Steady-state recurring hours, excluding baseline engineering and build:

| Alternative | N=1 | N=3 |
|---|---|---|
| A3 | 165–520 h | 320–1,040 h |
| A4 | 340–930 h | 660–1,800 h |
| A8 | 435–1,190 h | 770–2,100 h |
| A5 | 515–1,360 h | 860–2,260 h |

**Consequence for the owner's priority on reusable continuous assurance:**

- **The case rests on risk reduction and operational evidence, not on cost or labor savings.** Build first the automation whose operational value is highest and which is reused across products:
  - security-by-construction rules;
  - financial invariant suites;
  - release gates;
  - population reconciliation;
  - continuous security testing.
- **Defer** collection automation whose only purpose is audit evidence.

## ODP-09 recalculation (owner direction DF-ASR-2026-0D0F, 2026-11-26)

The owner directed that costs be separated by purpose, recalculated across
product counts 1, 2, 5, 10 and 15 and across reliance cases, and that **no
product-count threshold** gate investment. The originals are preserved (v1 in
`economics/`, scenarios S0–S8 unchanged in definition); the additions are:

- **Purpose classes** (`purpose` column in `activities.csv`, validated by a
  repository test): baseline-engineering, operational-visibility,
  financial-integrity, prevention, continuous-validation,
  operational-evidence, shared-infrastructure, auditor-packaging,
  customer-packaging and external-services. The generated model reports
  five-year cost by purpose for A4, A8 and A5 in each ODP-09 scenario. The
  classification of each activity is hypothetical.
- **Reliance cases** (`auditor_reliance` now none, partial or full):
  S9 no examination, S10 partial reliance, S11 full reliance, and S12
  customer-specific. S12 is a sensitivity of S9 (customer packaging doubled),
  not a separate model: the model has no customer-count dimension, and
  customer-mandated tests or audit-rights assessments are not modelled. All
  exclude a contracted reviewer; S10 and S11 include external technical
  validation of the assurance machinery, which an examiner relying on system
  evidence would need. Break-even N is not reported for these scenarios.
- **Shared observability** (Vigila, ODP-06) is charged identically to every
  alternative (ACT-114..116), so it changes totals but not rankings.

Five-year midpoints (USD, generated `generated/model.md` is authoritative):

| Scenario | Alternative | N=1 | N=2 | N=5 | N=10 | N=15 |
|---|---|---|---|---|---|---|
| S9 no exam | A4 | 0.79M | 1.09M | 2.02M | 3.56M | 5.10M |
| | A8 | 1.03M | 1.37M | 2.37M | 4.05M | 5.72M |
| | A5 | 0.96M | 1.18M | 1.86M | 2.98M | 4.10M |
| S10 partial reliance | A4 | 1.17M | 1.61M | 2.94M | 5.16M | 7.38M |
| | A8 | 1.41M | 1.88M | 3.30M | 5.65M | 8.00M |
| | A5 | 1.44M | 1.83M | 3.01M | 4.97M | 6.93M |
| S11 full reliance | A4 | 1.14M | 1.57M | 2.86M | 5.00M | 7.14M |
| | A8 | 1.39M | 1.84M | 3.21M | 5.49M | 7.77M |
| | A5 | 1.33M | 1.66M | 2.64M | 4.28M | 5.92M |
| S12 customer-specific | A4 | 0.88M | 1.23M | 2.30M | 4.07M | 5.84M |
| | A8 | 1.12M | 1.51M | 2.65M | 4.55M | 6.46M |
| | A5 | 1.03M | 1.29M | 2.07M | 3.37M | 4.66M |

What this shows, without thresholds:

1. **Purpose, not product count, decides each investment.** The A8 increment
   over A4 is prevention (about USD 192k over five years at N=1),
   financial integrity (about USD 34k) and test infrastructure (about
   USD 19k). Remediation of findings is reported as its own class, not as
   prevention. Those lines are justified by the risk they reduce under TNT-01,
   TNT-02 and TNT-16, whatever the product count.
2. **Auditor packaging is the line most sensitive to reliance** (USD 118k at
   N=1 to about USD 1.0M at N=15 in S8 for A4). It disappears when no
   examination is commissioned. Under partial reliance only the non-reliance
   evidence line halves; audit fieldwork support stays, so A4's packaging falls
   by about 21–23% (USD 118k to 93k at N=1). Build it
   only when an examination is actually planned.
3. **Evidence-collection automation (A5) is not only an audit investment.**
   In S9 and S12 (no examination) its midpoint falls below A4 from five
   modelled products, as in S11 (full reliance); at one and two products A4
   stays lowest in every ODP-09 case. This is a description of modelled
   results, not a threshold: the driver is manual evidence preparation
   needed for operations and customers, not auditor reliance. It depends on the
   hypothetical assumption that A4's manual evidence preparation
   (ACT-029, operational-evidence) is still needed without an examination;
   if it is not, A4 stays lowest. Measure actual manual evidence hours
   before deciding (measurement strategy below).
4. **External services** (examination and penetration-test provisions) are
   the largest non-baseline line in S8 and S10–S11. They are provisions, not
   commitments (TNT-18, ODP-11).

### Quantified and qualitative benefits

| Benefit | Quantified here? | Measurement strategy |
|---|---|---|
| Lower manual evidence and packaging hours | Yes (cost lines) | Record hours per evidence request and per audit request from the first customer request onward |
| Earlier detection of control failure | No (qualitative) | Time from control failure to detection, from gate and heartbeat records (BR-LOG-002) |
| Vulnerability classes and financial errors prevented | No (ECON-011) | Gate-blocked defects by class; invariant-suite failures caught before release |
| Faster customer diligence | No | Elapsed days per questionnaire; share of answers generated without edits |
| Reuse across products | Partly (scaling factors) | Actual hours to onboard each additional product to gates, suites and observability |

## What the numbers do and do not support (DF-ASR-2026-C005, superseded for ODP-09 by DF-ASR-2026-0D0F; items 2 and 4 superseded, items 1, 3 and 5 retained)

1. **At one or two products, no option is clearly cheaper on cash.** Minimum viable assurance has the lowest midpoint, but within overlapping ranges. The choice rests on security value, independence and owner capacity.
2. **Evidence-collection automation** *(superseded by the ODP-09 recalculation above: no product-count threshold applies; decide by purpose and measured manual evidence hours).* Original text: pays only with auditor reliance and three or more products; gate it on a written auditor position (WI-EXT-01) and a credible product pipeline.
3. **Prevention and testing automation (A8 increment, about USD 50–70k a year) is justified by risk reduction the model does not monetize,** under tenets TNT-01, TNT-02 and TNT-16. Much of it, such as financial invariant suites and security-by-construction rules, is arguably responsible baseline engineering for a financial product. Classifying it as baseline or as a discretionary assurance investment is an owner decision (ODP-09).
4. *(Superseded by ODP-09 and TNT-18: no external consulting budget is authorized; external services are not the default.)* Original text: if owner capacity binds, A3-style outsourced support is a natural relief.
5. **Never without demonstrated demand:**
   - A7 feature-parity platform;
   - custom anchoring;
   - OSCAL;
   - AI certifications.

## Limitations

- Hypothetical inputs dominate. The ranges are wide, and the midpoints hide that width.
- Security benefit (incidents prevented) is deliberately not monetized (ECON-011). The model therefore understates the value of prevention-focused automation, and it does so equally across alternatives that include it.
- Scaling factors for examinations (0.3 per extra product) assume products share one system description. Separate reports for separate legal boundaries would raise every alternative's marginal cost.
- Customer diligence and commercial impact (ECON-017) are deferred until pipeline data exists.

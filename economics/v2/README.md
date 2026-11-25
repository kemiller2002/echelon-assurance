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

**Base case (S0), five-year midpoints:**

- **At N=1 the alternatives other than A7 are close.** Midpoints run USD 1.17M (A4) to 1.44M (A1); A7 is USD 2.02M. The ranges span about 3×, so no ranking at N ≤ 2 is reliable. A4 has the lowest midpoint at N=1 and N=2, with A3 within about 4%.
- **From N=5, targeted automation (A5) has the lowest midpoint.** It is USD 2.76M against about USD 2.98M (A3, A4) and USD 3.25M (A2). It breaks even with A3 at 3 products on midpoints, but the margin stays small until about five. This depends on auditor reliance.
- **A7 (full internal platform)** breaks even with A2 at 6 products and with A3 at 8.
- **A8 costs about USD 245k more than A4 over five years at N=1, and about USD 350k more at N=5** (roughly USD 50–70k a year). The model counts the cost of prevention and testing automation but deliberately not its benefit (ECON-011).

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
- **Threshold.** The proposed threshold is 25% of capacity, about 450 hours a year (BR-GOV-011). The recommended package can exceed it even at one product, so contracting support is probable.
- **Not modelled.** The cost of that contracting has not been modelled. It is to be estimated when the owner sets the threshold (ODP-02), for example by pricing A3-style support for the hours above the threshold.

## What the numbers do and do not support (DF-ASR-2026-C005, proposed)

1. **At one or two products, no option is clearly cheaper on cash.** Minimum viable assurance has the lowest midpoint, but within overlapping ranges. The choice rests on security value, independence and owner capacity.
2. **Evidence-collection automation pays only with auditor reliance and three or more products.** Gate it on a written auditor position (WI-EXT-01) and a credible product pipeline.
3. **Prevention and testing automation (A8 increment, about USD 50–70k a year) is justified by risk reduction the model does not monetize,** under tenets TNT-01, TNT-02 and TNT-16. Much of it, such as financial invariant suites and security-by-construction rules, is arguably responsible baseline engineering for a financial product. Classifying it as baseline or as a discretionary assurance investment is an owner decision (ODP-09).
4. **If owner capacity binds, A3-style outsourced support is a natural relief,** with a similar total cost under non-reliance, and it adds some independent challenge. Its incremental cost on top of A4 or A8 is not yet modelled.
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

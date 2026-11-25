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

**Revision note.** The first v2 run charged maintenance and auditor non-reliance only to Echelon-built alternatives. The challenge passes (`findings/validation-report.md`, R1-012, R2-022, R2-024) found that biased. The model now does the following:

- Platform integration upkeep is charged to A2 and A6.
- Non-reliance lines are charged to A2 and A3, because their evidence is also system-generated.
- A6's non-reliance hours cover the financial and AI controls it shares with A5.
- The fractional reviewer grows per product.

The conclusions below come from the corrected run.

**Base case (S0), five-year midpoints:**

- **At N = 1–2 the alternatives are indistinguishable.** The ranges span about 3×, while the midpoints differ by under 10% (A3 1.13M, A4 1.17M, A2 1.21M, A5 1.27M at N=1). Break-even counts at N ≤ 2 carry no signal.
- **From N = 5, targeted automation (A5) has the lowest midpoint.** It reaches 2.67M at N=5 against 2.90M for A3 and 3.16M for A2. It breaks even with A3 at 3 products.
- **The full internal platform (A7) breaks even late.** It matches A2 at 6 products and A3 at 9.
- **Marginal five-year cost per extra product (midpoint):**

  | A7 | A5 | A6 | A4 | A3 | A2 | A1 |
  |---|---|---|---|---|---|---|
  | ~325k | ~350k | ~375k | ~450k | ~440k | ~490k | ~720k |

**Auditor non-reliance (S4) and the downside case (S5):**

- **The minimum viable option (A4) has the lowest or near-lowest midpoint at every product count.** It relies on little system-generated evidence.
- **Outsourced management (A3) is within about 3–4% of A4.**
- **Targeted automation and the hybrid never break even against A3** within 30 products. Against A2 they break even at 5 (A5) and 4 (A6) products, or 7 and 6 in S5.

**Slow adoption (S6)** delays A5's advantage but does not remove it (A5 is lowest from N=5).

## Owner capacity

The model prices hours but a single owner has about 1,800 working hours a year,
shared with product and company work. Recurring assurance labor implied by the
inputs (steady state, hypothetical):

| Alternative | N=1 | N=3 | N=3, auditor non-reliance |
|---|---|---|---|
| A3 outsourced | 85–300 h | 190–680 h | 280–940 h |
| A4 minimum viable | 300–810 h | 570–1,540 h | 570–1,540 h |
| A5 targeted | 275–760 h (+120–290 h/yr build, amortized) | 380–1,060 h | 820–2,160 h |
| A6 hybrid | 265–760 h | 370–1,060 h | 720–1,940 h |

At three products, every alternative except A3 can consume most of one
person's year. Capacity, not cash, is likely the binding constraint. The model
proposes an owner-set threshold (for example, recurring assurance labor above
25% of owner capacity, about 450 h a year) that triggers contracting
(A3-style support or a fractional reviewer) or a hire (BR-GOV-011, ODP-09).

## Limitations

- Hypothetical inputs dominate. The ranges are wide, and the midpoints hide that width.
- Security benefit (incidents prevented) is deliberately not monetized (ECON-011). The model therefore understates the value of prevention-focused automation, and it does so equally across alternatives that include it.
- Scaling factors for examinations (0.3 per extra product) assume products share one system description. Separate reports for separate legal boundaries would raise every alternative's marginal cost.
- Customer diligence and commercial impact (ECON-017) are deferred until pipeline data exists.

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

## Results (base case, five years, undiscounted, incremental only)

The figures are summarized here; `generated/model.md` is authoritative.

- **At one product** the alternatives overlap heavily. Midpoints run from USD 1.11M (A3) to 1.33M (A1), and USD 2.0M for A7. External examinations, penetration testing and core diligence are identical across alternatives. At the midpoint they are about 35–41% of each alternative's cost (23% for A7); at the low end of the ranges they are about half. The choice of assurance approach matters far less than the external scope.
- **Marginal cost per extra product (midpoint).** Lowest is A7 (USD ~300k) and A5 (USD ~325k), against A2 (~460k) and A1 (~690k). Shared automation pays off through reuse, as tenet TNT-14 predicts.
- **Break-even (base case):**
  - A5 targeted automation matches A2 commercial platform at 2 products and A3 outsourced management at 3.
  - A7 full internal platform needs 7–8 products.
- **Auditor non-reliance (S4) changes the conclusion:**
  - A5 never breaks even against A2 or A3 within 30 products.
  - A6 hybrid still breaks even at 4 (A2) and 8 (A3).
  - A4 minimum viable is unaffected, because it relies on little system-generated evidence.
- **High maintenance plus non-reliance (S5):** A6 is the only automated option still competitive. A7 never breaks even against A2 or A3.

## Decisions supported (DF-ASR-2026-C005, proposed)

1. **Immediately:** A4 minimum viable assurance, plus prevention and testing automation, which has value regardless of auditor reliance.
2. **Deferred:** evidence-collection automation, until a written auditor position on reliance exists (WI-EXT-01). If reliance is unavailable, use A6.
3. **Never without demonstrated demand:**
   - A7 feature-parity platform;
   - custom ledger or anchoring;
   - OSCAL;
   - AI certifications.

## Limitations

- Hypothetical inputs dominate. The ranges are wide, and the midpoints hide that width.
- Security benefit (incidents prevented) is deliberately not monetized (ECON-011). The model therefore understates the value of prevention-focused automation, and it does so equally across alternatives that include it.
- Scaling factors for examinations (0.3 per extra product) assume products share one system description. Separate reports for separate legal boundaries would raise every alternative's marginal cost.
- Customer diligence and commercial impact (ECON-017) are deferred until pipeline data exists.

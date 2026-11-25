---
id: ASR-ECON-002
title: Assurance economics model v2 (incremental, common scope)
status: draft — methodology per owner record; not a budget or purchase authorization
created: 2026-11-25
inputs: [requirements/assurance-economics.md, economics/v2/parameters.csv, economics/v2/generated/tco-summary.md]
---

# Assurance economics v2

The original model (`economics/parameters.csv`, `economics/compliance-economics.md`)
is preserved unchanged as the comparison baseline (owner record:
"do not silently overwrite estimates"). v2 applies ECON-001..018 where the
existing F# `assurance tco` tool can express them:

| Rule | How v2 applies it |
|---|---|
| ECON-001 baseline excluded | Product engineering, including the SBC and FIN-BC requirements, is common to every scenario and **excluded**; v2 counts incremental assurance cost only. v1 remains the total-lifecycle view. |
| ECON-002 cost classes | Every line carries a class in `category`: shared-incremental-build, shared-maintenance, manual-administration, external-assurance, infrastructure, remediation. |
| ECON-004 equal scope | All scenarios: one product, SOC 1 Type 2 + SOC 2 Type 2 (five categories), annual independent pen test, same remediation and bespoke-diligence lines. |
| ECON-006 maturity stages | Year 1 = readiness / observation period: **no examination fees**; years 2–5 recurring operations. Finer stages need per-year modelling (see limits). |
| ECON-007 evidence classes | Every line is `vendor-claim` or `assumption`; no quotes exist. |
| ECON-008 independent review | Fractional independent human review is charged to **every** scenario (v1 charged it only to the automated ones). |
| ECON-012 auditor rejection | Scenario Bx: targeted automation where the auditor does not rely on system evidence. |

## Result (undiscounted, from `generated/tco-summary.md`)

| Scenario | Unavoidable external | Reducible | Total, 5 years |
|---|---|---|---|
| A Minimum viable assurance | $324k – $795k | $266k – $966k | $590k – $1.76M |
| B Targeted automation (hybrid) | $339k – $840k | $217k – $832k | $556k – $1.67M |
| Bx B, auditor rejects system evidence | $339k – $840k | $397k – $1.46M | $736k – $2.30M |
| C Commercial platform | $324k – $795k | $269k – $889k | $593k – $1.68M |
| D Outsourced management | $324k – $795k | $272k – $980k | $596k – $1.77M |
| E Full parity build | $354k – $870k | $287k – $1.05M | $641k – $1.92M |

At an 8% discount rate the ordering is unchanged
(`assurance tco --rate 0.08 --input economics/v2/parameters.csv`).

## What changes from v1, and what does not

- **Fairer comparison, same conclusion.** Charging independent review to
  every scenario and removing year-1 examination fees narrows absolute totals
  but does not separate B, A and C: their ranges overlap almost entirely.
  Choosing between them is a question of fit, evidence quality and lock-in,
  not of cost.
- **E (feature parity) is the most expensive non-failure case**, and ECON-014
  rules it out without demand.
- **Bx remains the dominant downside.** Defer automation whose value depends
  on auditor reliance until a CPA firm has scoped it (roadmap P3).

## Marginal cost per product (owner illustration, not a forecast)

Per-product average annual incremental cost = (B/5 + M)/N + P. With the
owner's illustrative B = $60k, M = $15k, P = $5k: N = 1 → $32k; 2 → $18.5k;
5 → $10.4k; 10 → $7.7k. Applying v2's scenario-B ranges instead
(B = $30k–$122.5k, M = $15k–$52.5k, P assumed $5k–$15k) gives N = 1 →
$26k–$92k; N = 5 → $9.2k–$30.4k. Valid only under linear assumptions,
equal scope and a shared audit boundary; ECON-005 warns that distinct legal
or audit boundaries break the sharing.

## Limits (to address when implementation is authorized)

- The F# tool models year 1 plus a flat annual rate; the five maturity stages
  of ECON-006 and break-even curves (ECON-013) need per-year lines.
- No telemetry yet for questionnaires (ECON-010), testing (ECON-011) or
  actual costs (ECON-015); every number is an assumption until replaced.
- Commercial effects (ECON-017: procurement cycle, conversion) are not
  modelled; correlation must not be read as causation.
- Mandatory legal, contractual and safety requirements are kept regardless
  of return (ECON-018).

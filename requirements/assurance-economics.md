# Assurance economics
Date: 2026-11-25
Status: Proposed economic methodology; not a validated budget or purchase authorization.

## Review of existing model
Inspected economics/parameters.csv. Full parity scenario assumes $80k–280k initial build and $30k–105k annual maintenance; hybrid $30k–122.5k build and $15k–52.5k maintenance. All scenarios assume recurring SOC 1 $29k–60k, SOC 2 $15k–40k and pentest $15k–35k. These are vendor claims/assumptions, not quotes. Fractional review $10k–30k annually is explicit in automated/hybrid scenarios but omitted as a separate line in conventional alternatives; determine whether bundled elsewhere before comparing. Existing model does not explicitly charge all baseline engineering to compliance; incremental scope of automation is insufficiently decomposed. Preserve original model as comparison; do not silently overwrite estimates.

## Requirements
ECON-001: Define common baseline of responsible product engineering and operations across alternatives; count only incremental assurance costs in incremental comparison, retain total lifecycle view separately.
ECON-002: Classify costs as baseline, shared incremental build, shared maintenance, product-specific incremental, manual administration, external assurance, infrastructure, remediation and opportunity cost; avoid double counting.
ECON-003: Use activity-based estimates (hours, loaded rates, frequency, infrastructure consumption, AI tokens, reviewer effort); separate cash expense from opportunity cost and sunk cost.
ECON-004: Compare minimum viable assurance, targeted automation, commercial platform, outsourced, hybrid and full parity only where justified; equal scope, risk and evidence quality.
ECON-005: Allocate shared costs across actual and scenario product counts, accounting for integration, complexity, support and distinct legal/audit boundaries; no assumption one report covers all products.
ECON-006: Model commercial maturity stages: research/prelaunch, first customer, observation period, examination and recurring operations. External spend triggered by obligations and risk, not arbitrary annual start; no deferral of mandatory controls.
ECON-007: Separate actual quotes, observed costs, vendor claims and assumptions; record source, date, scope, uncertainty and sensitivity.
ECON-008: Include independent reviewer costs consistently across alternatives, distinguishing technical verification and organizational independence.
ECON-009: Evaluate automation per control: incremental build and maintenance versus avoided labor, risk and evidence benefit; prioritize preventive constraints and reusable high-frequency workflows.
ECON-010: Track questionnaire volume, preparation/review hours, response reuse, customer-specific questions and turnaround; never automate unsupported assertions.
ECON-011: Measure security testing runtime, compute, coverage, remediation, escaped defects and independent test findings; do not monetize hypothetical prevented incidents as guaranteed savings.
ECON-012: Model auditor acceptance and rejection, combined engagement discount, customer-specific scope, provider costs, maintenance overruns and slower product adoption.
ECON-013: Calculate five-year cash flow, discounted cost with explicit rate, break-even and marginal cost per product/customer; show ranges and downside cases, no invented precision.
ECON-014: Compare build/reuse/buy using lifecycle cost, lock-in, security, evidence quality, maintenance and opportunity cost; no feature parity objective without demand.
ECON-015: Maintain actual cost telemetry and periodically reconcile estimates; use existing systems where verified, no new service by default.
ECON-016: Stage investment with measurable exit criteria, reversible decisions and budget approval; no paid consultations or purchases authorized.
ECON-017: Treat compliance as commercial enabler: measure procurement cycle, customer conversion and onboarding effort; distinguish correlation from causal impact.
ECON-018: Preserve mandatory legal, contractual and safety requirements irrespective of ROI; record residual risk and owner approval for discretionary tradeoffs.

## Illustrative sensitivity, not forecast
Shared build B=$60k, shared annual maintenance M=$15k, product-specific annual P=$5k, N products. Five-year average annual per-product incremental cost = (B/5+M)/N+P. N=1: $32k; 2: $18.5k; 5: $10.4k; 10: $7.7k. Excludes baseline engineering, external examinations, taxes, discounting and other costs. Break-even against comparable alternative annual cost A per product occurs when N >= (B/5+M)/(A-P), if A>P; only valid with linear assumptions and equal scope.

## Questions
OQ-111,755: seek combined SOC engagement indicative quotes once scope known; no discount assumed.
OQ-311,612: obtain external testing/reviewer quotes when justified; no engagement authorized.
OQ-312,500,604,765: model auditor reliance and rejection explicitly.
OQ-508: compare GitHub plan incremental costs and evidence coverage; no upgrade authorized.
OQ-606,611: defer specialized certification/formal modeling until demand or risk benefit supports investment.
OQ-802,803: reconcile risk-based automation and independence requirements with consistent budget assumptions; do not lower assurance status thresholds to meet budget.
OQ-215: measure actual customer diligence requirements and reusable evidence value.

## Next work
Rebuild model as a separate version with traceable line-item decomposition, sensitivity and common scope; reconcile catalog, roadmap and questions. F# tooling when implementation authorized. No other repo changes, tests, purchases or external consultations authorized. Run governance/register checks before merge.

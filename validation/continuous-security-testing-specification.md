---
id: ASR-SPEC-CST-001
title: Continuous security testing specification
status: proposed specification (no implementation authorized)
created: 2026-11-25
requirements: [BR-VUL-001..013, BR-APP-001..013, BR-FIN-001..023 (validation), BR-AI-004, BR-ASR-008, BR-ASR-010, BR-BCR-001]
supersedes_detail_of: requirements/continuous-security-testing.md (CST-001..026, dispositioned in requirements/source-dispositions.csv)
---

# Continuous security testing specification

Security validation is continuous engineering, not an annual event. This
specification turns the baseline into suites, triggers, gates and reports. It
reuses mature tools as controlled dependencies, and builds F# only for:

- orchestration and change-impact selection;
- domain suites (financial, tenant, AI authority);
- normalization of findings into evidence envelopes.

**Distinctions that every output preserves:**

| Artifact | Producer | What it is | What it is not |
|---|---|---|---|
| Internal security assessment report | Automated, Echelon-controlled | Scoped results of internal suites with coverage and limitations | Not a penetration test, not independent, not an attestation |
| Technically separated verification (`technically-separated`; planned, WI-DOKIMOS-01) | Separate verifier identity (Dokimos; currently same CI trust domain, `integration/capability-verification.csv`) | Re-execution or check of receipts under separate credentials | Not organizational independence |
| Independent penetration test | External qualified tester | Human-led adversarial testing under rules of engagement | Not an attestation |
| SOC 1 / SOC 2 report | Independent CPA firm | Attestation over a described system and period | Not a security guarantee |

## 1. Suites

| Suite | Baseline | Technique | Fails the release gate when | Run on |
|---|---|---|---|---|
| S1 Architecture rules | BR-APP-001–004, BR-APP-012 | F# analyzers: no string-built SQL outside Strata; no raw markup sinks; no JS outside Limen; no unsafe/P/Invoke; no float in money paths | Any violation without an unexpired exception | Every change touching F# or web assets |
| S2 Static analysis and secrets | BR-VUL-002, BR-APP-012 | Existing SAST and secret scanners, push protection, client-bundle scan | Material finding or secret | Every change; bundle scan on deployment candidates |
| S3 Dependency admission | BR-APP-013, BR-VUL-010 | Lockfile and SBOM diff, provenance, license and advisory match | Unadmitted dependency; known exploitable advisory in reachable code | Dependency change; advisory publication; daily SBOM match |
| S4 Authorization matrix | BR-APP-005 | Enumerate handlers; missing, expired and forged identity; role escalation | Handler without policy; unexpected allow | Change to handlers, policies or identity |
| S5 Tenant isolation | BR-APP-006 | Multi-tenant property tests; ID substitution across API, jobs, exports, caches; row-level-security snapshot | Any cross-tenant read or write | Change to data access, jobs, exports or storage |
| S6 Input and fuzz | BR-APP-007, BR-VUL-004 | FsCheck properties; time- and resource-bounded fuzzing of decoders | Property failure; crash; budget breach | Decoder change (bounded); nightly (extended) |
| S7 API and browser (DAST) | BR-VUL-003, BR-APP-009 | Schema-driven API tests and browser automation against an ephemeral isolated deployment | Material finding | Deployment candidates |
| S8 Financial abuse and integrity | BR-VUL-005, BR-FIN-001–023 | Duplicate requests, payload conflicts, concurrency, period race, replay-never-posts, rounding, reconciliation breaks, sanctions fence race | Any invariant violation | Change to financial modules; nightly full |
| S9 Infrastructure and identity configuration | BR-VUL-006, BR-APP-008, BR-IAM-004 | Managed cloud policy evaluation; egress rules; ruleset and IAM snapshots against declared baselines | Drift on protected settings | Configuration change; continuous drift |
| S10 AI authority and prompt-injection containment | BR-AI-001–004 | Indirect injection fixtures; out-of-scope, expired and over-budget tool calls; data-class egress | Any unauthorized tool call or disclosure | Model, prompt, tool or policy change; weekly |
| S11 Recovery and fault injection | BR-BCR-001, BR-FIN-016 | Crash mid-transaction; provider outage; deployment interruption; restore and reconcile | Lost or duplicated accepted request; failed restore | Change to persistence; monthly restore |
| S12 Evidence integrity | BR-ASR-003–008 | Drop, reorder or duplicate evidence; stop collector; attempt overwrite or delete | Undetected gap; silent collector | Change to collectors; continuous heartbeat |
| S13 Security tool mutation | BR-ASR-010 | Seeded known-positive fixtures per scanner; mutate controls and expect detection; selector mutation | Fixture not detected | Tool or rule change; weekly |

## 2. Triggers and change-impact selection

| Trigger | Suites |
|---|---|
| Every relevant change (pull request) | Selected subset of S1–S6, S8, S10 by impact map; S2 always |
| Deployment candidate | S1–S8 full, S9 on the candidate configuration, S12 smoke |
| Configuration change | S9; S5 when isolation settings change; S12 when evidence settings change |
| Vulnerability disclosure (advisory) | S3 re-match against deployed SBOMs; targeted S7/S8 if reachable |
| Model or dependency change | S10 (model/prompt/tool); S3 + affected S1–S8 (dependency) |
| Scheduled | Nightly: full S1–S8 regression (catches selection misses). Weekly: S10, S13. Monthly: S11 restore |
| Continuous | S9 drift, S12 heartbeat (independent, BR-ASR-008), advisory feed |

**Change-impact map (BR-VUL-008).**

- *What is tagged.* Each module is tagged with its trust boundaries and controls. The tags come from the asset and boundary inventory (BR-VUL-001).
- *How suites are selected.* A change selects every suite tagged on any touched module or its reverse dependencies.
- *Fallback.* Unknown impact selects the full set.
- *Catching misses.* The nightly full run catches selection misses. A miss found that way (a failure caught nightly but not on the change) is recorded as a selector defect, and S13 mutates the selector.

## 3. Findings lifecycle (BR-VUL-009)

1. **Normalize.** Every tool and external report is normalized into one finding record: an evidence envelope plus restricted payload. Records are deduplicated by rule, location and fingerprint.
2. **Prioritize.** Findings are ranked by exploitability, impact (tenant data, money, release integrity) and exposure.
3. **Remediate.** Target windows are set by severity. The numeric windows are an owner decision recorded in the obligation register (BR-GOV-009); they are not set here.
4. **Close.** A material finding closes only with a linked permanent regression test, or a recorded reason that none is practical.
5. **External findings.** Findings from external tests follow the same path and become fixtures where safe.
6. **Restriction.** Finding details live only in the restricted tracker. This repository references them by opaque ID and severity class (`SECURITY-AND-CONFIDENTIALITY.md`).

## 4. Environments and authorization (BR-VUL-007)

- **Isolated targets.** Active testing (S7, S10, S11) runs only against ephemeral, production-equivalent isolated deployments seeded with synthetic data, with no real payment rails or provider credentials.
- **Run records.** Each run records its target allowlist, rate limits and stop conditions, and the authorization that covers it.
- **Recorded differences.** Differences from production are recorded with each report.
- **Provider rules.** Cloud provider testing rules are checked before the first active run (OQ-306).
- **No destructive production testing.** Nothing destructive runs against production.

## 5. Internal security assessment report (generated)

Generated per material release and monthly (BR-VUL-011). Fixed sections:

1. scope and target versions (artifact and configuration digests);
2. suites run, with tool versions and selection rationale;
3. coverage (handlers, boundaries, invariants) and known gaps;
4. findings by severity, with disposition counts only (no exploit detail);
5. mutation and sentinel results (S13);
6. environment differences from production;
7. limitations, including the statement: *"Internal automated assessment by Echelon-controlled systems. Not an independent penetration test or attestation."*

The public variant omits sections 4 and 6 detail beyond counts and passes the
classification filter (BR-CUS-002).

## 6. External testing strategy (BR-VUL-012, BR-VAL-003)

**Owner direction DF-ASR-2026-0D0F (2026-11-25).** External engagement decisions are deferred. Comprehensive continuous internal security testing applies. The need for independent testing is reassessed against risk and actual obligations before material production launch (WI-EXT-04). The strategy below applies when testing is engaged.

- **Purpose.** External tests challenge our controls; they are not our discovery mechanism.
- **What the engagement package contains.** Architecture, inventory, threat model, latest internal report, known gaps and proposed rules of engagement, all generated.
- **Scope priority:**
  1. business logic and financial workflows;
  2. authorization and tenant isolation;
  3. infrastructure trust boundaries and identity;
  4. architectural assumptions (Strata boundary, Limen boundary, AI authority gateway);
  5. areas where internal suites are weak (selector defects, mutation survivors).
- **What internal testing may change.** It may reduce remediation volume and retest effort, and may justify focusing scope. It never justifies excluding scope that an obligation requires. No claim is made that internal automation satisfies an independent-testing obligation unless the authority accepts it in writing (OQ-302, OQ-303, OQ-309).
- **Legal frequency.** Under the FTC Safeguards Rule, 16 CFR 314.4(d)(2) allows effective continuous monitoring as an alternative to annual penetration testing plus six-monthly vulnerability assessment. Applicability to Echelon or Unbound is unresolved (OQ-300), and contractual and auditor expectations may require annual tests regardless.

## 7. Cost controls (BR-GOV-015)

Each run records runtime, compute, AI tokens, findings yield and selector misses.
The economics model is recalibrated from these actuals (WI-SCALE-01). Hypothetical
prevented incidents are never monetized as savings.

---
id: ASR-AUTO-001
title: Control automation matrix and automation-level reasoning
status: draft
version: 0.1.0
created: 2026-11-25
---

# Control automation matrix

Per-control target and current levels live in `controls/control-catalog.csv`
(`automation_target`, `automation_current`, `failure_mode`). This document
gives the reasoning behind those choices.

## 1. What each level buys, and what it costs

| Level | Risk reduced | New risks introduced | Independence implication | Economics |
|---|---|---|---|---|
| L0 Manual documentation | Little; depends on diligence | None new | Evidence is self-produced; auditor relies on sampling | Cheapest to set up, most expensive per period |
| L1 Reminders/tasks | Missed periodic activities | Alert fatigue | None | Very cheap; common in compliance platforms |
| L2 Evidence collection | Evidence loss; prep effort | Collector can miss data silently; collected ≠ effective | Evidence is system-generated, which auditors generally weigh above manually assembled evidence, subject to IPE testing (`auditor-interpretation`) | Main saving in audit prep |
| L3 Automated validation | Late detection | False positives/negatives; the validator becomes a control that itself needs validation | Validation by the same team is still self-assessment, not independent | Replaces periodic manual review |
| L4 Preventive enforcement | The prohibited event occurs | Blocks legitimate work; outages if the enforcer fails closed; enforcer misconfiguration is a single point of failure | Strongest evidence: auditor tests the configuration over the period plus override paths, rather than samples of outcomes (`auditor-interpretation`, confidence `medium`) | Higher build cost; very low run cost |
| L5 Continuous enforcement + bounded remediation | Dwell time of drift | Automation can make wrong changes at scale; can mask a recurring root cause; may destroy evidence | Remediation must be logged and reviewed by someone other than its author | Worth it only for reversible, high-frequency drift |

**Rule:** aim for the lowest level that brings residual risk within appetite
at acceptable cost, and stop at L4 unless the drift is frequent, the fix is
reversible, and the blast radius is bounded. Judgment and accountability
activities (risk acceptance, exception approval, management review, policy
approval) stay at L1–L2 by design: automating their *decision* would remove
the human accountability that is the point of the control.

## 2. Fail-closed versus fail-open

Fail closed when the protected action is **irreversible or high-impact** and
a delay is tolerable: money movement, production deployment, privileged
access, policy changes, evidence deletion, cross-tenant access.

Fail open (with an alert and an exception record) is justified only when
blocking would cause harm greater than the risk, **and** a detective control
catches the event afterwards: e.g. an evidence collector being down must not
stop production deployments, but the gap is itself a control failure
(`CTL-ASR` heartbeat controls); a vulnerability scanner outage should not
block an emergency security fix.

## 3. Examples from the charter

| # | Control example | Target | Mechanism | Failure mode | Escalation | Recovery | Evidence |
|---|---|---|---|---|---|---|---|
| 1 | Prevent unauthorized code merges | L4 | Repository rulesets as code: required review by non-author, required checks, no admin bypass; ruleset drift check (L3) | fail-closed | Bypass or ruleset change → alert to owner + second person | Revert ruleset from code; retrospective review of merges during gap | Ruleset state snapshots over period; PR review events; bypass log |
| 2 | Verify required test execution | L4 | Required status check from CI; CI fails on empty/skipped run; test results attached to build provenance | fail-closed | Repeated flakiness → engineering lead | Re-run; flaky-test quarantine requires approval (never silent skip) | CI run records, test reports, provenance attestation |
| 3 | Detect unapproved production changes | L3 (L4 where possible) | Deploy-only-signed-artifacts admission (L4); runtime inventory compared with approved release manifest; cloud audit log diff against IaC (L3) | fail-closed at admission; detective for console changes | Unexplained change = incident (POL-08) | Reconcile to IaC; investigate actor | Deploy records, admission decisions, drift reports |
| 4 | Validate access permissions | L3 + L5 for removal of unapproved grants in non-production; L3 in production | Entitlements as code; continuous diff of actual vs declared | detective-only (diff) | Unapproved privileged grant → page | Revoke; review actions taken by grant | Entitlement snapshots, diff results, approvals |
| 5 | Detect dormant accounts | L5 | Disable after 45 days inactivity (reversible) | fail-open-justified (disable is reversible; owner can re-enable via request) | Notify owner and sponsor | Re-enable via approval | Disable events, inactivity source data |
| 6 | Verify MFA coverage | L4 | IdP policy requiring phishing-resistant MFA; conditional access; coverage report | fail-closed | Any user without MFA → block | Enrollment flow | IdP policy config over period, coverage reports |
| 7 | Detect exposed secrets | L4 (push protection) + L3 (history and runtime scanning) + L5 (auto-revoke where provider supports) | Push protection, scanning, provider revocation hooks | fail-closed at push; detective otherwise | Detected secret → incident if valid | Rotate, revoke, review use | Scan results, revocation events |
| 8 | Validate backup restoration | L3 | Scheduled restore to isolated environment with integrity assertions | detective-only | Failed restore → incident (availability risk) | Fix backup config; re-test | Restore test records with hashes and assertions |
| 9 | Detect configuration drift | L5 for low-risk reversible settings; L3 + human for the rest | Policy-as-code evaluation (cloud-native policy/OPA), IaC reconciliation | detective with bounded auto-revert | Drift on security-critical setting → page | Reconcile; root-cause | Evaluation results, remediation log |
| 10 | Verify encryption settings | L4 | Org guardrails preventing unencrypted storage; continuous evaluation | fail-closed | Violation → engineering lead | Re-encrypt / migrate | Guardrail config, evaluations |
| 11 | Validate dependency integrity | L4 | Lockfiles with hashes, signed artifacts, provenance verification at deploy | fail-closed | Verification failure → stop release | Investigate supply-chain event | SBOM, provenance, verification logs |
| 12 | Detect overdue vulnerability remediation | L3 | SLA clock per finding; overdue without exception = control failure | detective-only | Overdue → owner, then management review | Remediate or approved exception | Finding lifecycle events |
| 13 | Verify incident response deadlines | L3 | Incident record computes deadlines from discovery time | detective-only | Approaching deadline → incident commander, then owner | — | Incident timeline, notification records |
| 14 | Enforce financial approval thresholds | L4 | Domain authorization in application (pure function over authority matrix); dual control | fail-closed | Denials logged; repeated denials → fraud review | — | Approval and denial events with authority version |
| 15 | Detect segregation-of-duties violations | L4 for defined conflicts (approver ≠ initiator, reviewer ≠ author); L3 for cross-system role combinations | Application and repository rules; periodic role-combination analysis | fail-closed for L4 cases | Detected conflict → owner | Remove role or exception | Rule config, analysis results |
| 16 | Validate tenant isolation | L4 (tenant-scoped data access by construction) + L3 (negative tests per change, adversarial tests periodically) | Type-level tenant scoping in Strata-style persistence, row-level security | fail-closed | Any cross-tenant read in tests → release blocked; in production → incident | — | Test results, access logs |
| 17 | Enforce data retention | L5 | Lifecycle rules and scheduled crypto-shredding; legal hold override | fail-open-justified (delay deletion rather than delete under hold uncertainty) | Deletion failure → owner | Re-run | Deletion manifests, hold register |
| 18 | Verify agent permissions | L4 + L3 | Agent identities with platform-enforced scopes; continuous diff of agent grants vs agent definitions | fail-closed | Excess grant → revoke + review agent actions | — | Agent definitions, grant snapshots, execution records |
| 19 | Detect unauthorized policy changes | L4 | Policy model in protected repository; changes require approval authority's review (CODEOWNERS/rulesets); published policy hash compared with approved hash | fail-closed | Mismatch → owner | Restore approved version | Approval events, policy version hashes |

## 4. Where automation should *not* go higher

| Activity | Ceiling | Why |
|---|---|---|
| Risk acceptance and exception approval | L1–L2 for the decision | Accountability must be human; agents may evaluate and recommend (POL-11.02). The catalog's L4 for CTL-GOV-007 and CTL-GOV-002 applies to *enforcing who may decide* (approver ≠ requester, signed approval required before effect), not to automating the decision |
| Policy approval | L2 | Same |
| Vendor due diligence judgment | L2 | Report review can be assisted; reliance decision is human |
| Incident severity and notification decisions | L2–L3 | Deadlines automated; legal judgment human |
| Production financial adjustments | L4 enforcement of approval, never L5 remediation | Automated "fixing" of ledgers destroys auditability |
| Evidence store integrity remediation | Detect (L3) only | Automated repair of evidence is indistinguishable from tampering |

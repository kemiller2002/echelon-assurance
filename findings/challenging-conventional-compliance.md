---
id: ASR-FIND-CHAL-001
title: Challenging conventional compliance practices
status: draft
version: 0.1.0
created: 2026-11-25
---

# Challenging conventional compliance

For each practice: the risk it targets; whether the practice is required
(and by what); whether a stronger mechanism exists; what new risk the
alternative brings; and whether an auditor would accept it. "Auditor
acceptance" is our expectation, not a confirmed fact, unless cited; every
row's acceptance question rolls up into OQ-500, OQ-601, OQ-759 for the first
CPA conversation.

| # | Conventional practice | Risk actually addressed | Required? | Stronger alternative | New risks | Likely auditor view | Controls |
|---|---|---|---|---|---|---|---|
| 1 | Quarterly manual user access review (export, spreadsheet, sign-off) | Accumulated or unapproved access | TSC requires access be authorized and removed (CC6.2/6.3); periodic review is a **common means**, not a mandated form (`auditor-interpretation`) | Access-as-code with continuous drift detection; quarterly attestation only for privileged/agent identities on a generated diff | Drift tool misses a system → silent gap; mitigated by inventory reconciliation | Expected to accept automated detection if the tool is tested (ITGC over the tool) and populations are complete (`assumption`) | CTL-IAM-002, 003, 009, CFG-002 |
| 2 | Change ticket + CAB approval | Unauthorized or untested change | CC8.1 requires authorized, tested, approved changes; a CAB is convention | Protected branches, required checks, non-author review, signed provenance, deploy admission; evidence is the PR itself | Ruleset changes by org owners; mitigated by snapshots and anchoring | Generally accepted (`auditor-interpretation`); auditor tests ruleset config over period + bypasses | CTL-SDL-001, 002, 004, 006, 007 |
| 3 | Screenshots as evidence | Proving configuration state | Evidence must be sufficient and appropriate; screenshots are convention | Machine-readable config snapshots with hashes, continuously; reproducible queries | Snapshot tool wrong; integrity ≠ accuracy | IPE testing still required; generally preferred to screenshots | CTL-ASR-001, 003 |
| 4 | Annual policy re-signing by all staff | Staff unaware of policy | CC1.4/CC2.2 require communication; annual re-signing is convention | Acknowledge on hire and on material change; training gates elevated access | Less frequent touchpoint | Likely acceptable if communication evidence exists (`assumption`) | CTL-HR-001, 002 |
| 5 | 20–35 separate policy documents | Coverage of subject matter | Written program required (Safeguards 314.4); number of documents is not | 13 policies with addressable sections generated from one model | Customer forms ask for named documents; render on demand | Accepts any structure that covers criteria (`auditor-interpretation`) | CTL-GOV-002 |
| 6 | Manual transaction sampling (25–60 items) for financial controls | Misstatement in population | Auditor's choice of procedure (AT-C 205); not ours to remove | Write-time invariants + continuous independent checks + two-way reconciliation to bank/custodian (100% coverage) | Invariants can encode the wrong rule; inputs may be fraudulent but valid | Auditor still samples, but tests the controls rather than re-performing arithmetic; reduced extent is the auditor's judgment (OQ) | CTL-FIN-001..005, 008, 012 |
| 7 | Annual pen test as the security test | Unknown exploitable flaws | Required only by specific regimes (PCI 11.4, NYDFS 500.5, Safeguards 314.4(d)(2) alternative); otherwise customer expectation | Continuous internal adversarial testing + narrowly scoped independent test + retest + VDP | Over-trusting internal tests; never present them as independent | Internal testing supports CC4.1/CC7.1; independent test still expected by banks | CTL-VUL-001..004 |
| 8 | Password rotation every 90 days | Credential compromise | Contradicted by NIST SP 800-63B (rotate on compromise) | Phishing-resistant MFA + breached-password screening | Some customer questionnaires still ask; answer with rationale | Accepted with 800-63B rationale (`auditor-interpretation`) | CTL-IAM-004 |
| 9 | Vendor questionnaires for every vendor | Vendor risk | CC9.2 and Safeguards 314.4(f) require oversight proportional to risk | Risk-tier vendors; rely on SOC reports + CUEC mapping for critical ones; no questionnaire where a report exists | Over-reliance on vendor report scope | Expected to be accepted; CUEC mapping is what auditors are expected to look for (`assumption`) | CTL-VEN-001, 002 |
| 10 | Separate compliance program per framework | Each framework's requirements | No framework requires its own program | One catalog; requirements map to controls | Mapping errors; licensing limits on reusing mappings | Auditor examines controls against its criteria regardless of structure | CTL-GOV-005 |
| 11 | Pre-audit evidence scramble | Missing evidence at audit time | Not required — symptom | Evidence emitted at source continuously; heartbeat on missing evidence | Collector outages; mitigated by dead-man monitor | Expected positive, if completeness is demonstrable (`assumption`) | CTL-ASR-001, 003, 004 |
| 12 | Human reviewer signs off agent work by reading it | Agent error | AI RMF and OWASP recommend human oversight; form not mandated | Platform-enforced permissions + independent validation + human approval for consequential actions only | Automation bias in approvals (RSK-019) | Untested; AICPA guidance evolving (TQA 9561, proposed 2027 TSC) | CTL-AI-002, 003, 005, 007 |
| 13 | Physical security policy for a cloud-only company | Physical access to systems | CC6.4 applies but is carved out to providers | Inherit via carve-out; device rules in operations policy | Carve-out depends on provider reports | Standard carve-out treatment | CTL-VEN-005 |

## Policies that become procedure-free

Technical enforcement removes procedures (and the burden of proving people
followed them), never the policy objective or accountability. See
`policies/policy-inventory.md`, "Policies whose manual procedures disappear".

## Where the conventional practice should stay

- **Human risk acceptance, policy approval and exception approval.**
  Automating the decision would remove the accountability those controls
  exist to create.
- **Independent external testing and attestation.** No internal mechanism,
  however strong, substitutes for independence in the eyes of banks,
  sponsors' auditors and the AICPA standards.
- **Management review.** It is cheap, and it is where compliance theater is
  caught (RSK-033).

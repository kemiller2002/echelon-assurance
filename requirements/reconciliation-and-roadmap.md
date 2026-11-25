# Assurance baseline challenge and reconciliation
2026-11-25. Proposed reconciliation direction, not a completed catalog migration or operational assessment.

## Precedence
Owner-approved foundations govern. Subsequent batch specifications are proposed refinements, not automatic overrides. Existing stable control IDs remain canonical; map new requirement IDs as child requirements, merge semantic duplicates with traceability, retain distinct testable obligations. Never delete a legal/contractual requirement solely for cost. Conflicts require explicit decision and evidence.

## Challenge findings and dispositions
1. SQL: prohibit dynamic executable SQL, including raw escape paths; parameterization alone does not prove authorization.
2. HTML: owner rule is no HTML input; strengthen SBC-002 from untrusted rendering only to no user-supplied HTML accepted as markup. Plain text encoded safely; trusted templates controlled.
3. JS: application-authored JS prohibited outside Limen; generated/runtime/vendor artifacts inventoried and reviewed, not blanket exemption.
4. Financial IDs: use FIN-BC and AIG-BC canonical document IDs rather than abbreviated conversation labels.
5. Financial authority: A3 is execution under independently enforced prior authority, not independent agent authorization; prefer deterministic payment workers over model execution.
6. Independence: separate internal testing, protected technical verification, organizational independence and external attestation; risk-tier requirements replace blanket human review only after owner approval and catalog reconciliation.
7. Evidence: immutability and hashes do not prove completeness or truth; independent populations and heartbeat required. Separate sensitive payloads and retention.
8. Automation L5: bounded reversible remediation; irreversible disposal under approved lifecycle policy with hold checks and elevated safeguards, not blanket L5.
9. Availability: no global queue ordering; per-aggregate ordering, durable acceptance, idempotency, compatible migrations and recovery. Queue alone does not guarantee zero downtime.
10. Policy model: structured F# decisions where executable; prose/legal judgment linked, not forced into code.
11. Tooling: no full commercial feature parity, no custom scanner/ledger/witness infrastructure absent demonstrated gap; evaluate reuse and managed services.
12. SOC: design all five categories, scope actual examination by commitments; no claim of legal requirement or auditor acceptance.
13. Economics: baseline engineering versus incremental assurance; comparable reviewer costs; staged external spend; original estimates preserved until recalculated.
14. Transparency: public standards and sanitized evidence, restricted operational details; review public branch contents before merge.
15. Vigila, Arca, Strata, Fides, Praxis, Dokimos and Folio responsibilities are targets until verified; resolve actual capability gaps before assigning implementation.
16. Regulatory: no assumption that hosted payments eliminate PCI scope, that screening guarantees legality, or that crypto-shredding satisfies erasure.
17. Missing OQ-216: sponsor-controlled funds and no platform custody/transmission as initial design constraint; map actual funds flows and assess licensing before enabling alternative models. Legal conclusion open.
18. Missing OQ-217: maintain framework-neutral evidence mapping; obtain actual bank framework requirements during diligence; no additional certification by default.

## Question crosswalk
All 112 original question IDs enumerated below by disposition workstream; this is routing, not closure. Multiple dependencies may apply. Preserve original question/status and add separate design disposition, source, external dependency, owner, next action and closure evidence. OQ-807 previously marked answered; verify.
- Authoritative sources and future standards: 100,107,113,214,305,605,613.
- Commercial, legal and regulatory boundaries: 101,102,103,104,105,106,110,112,114,115,116,200,201,202,203,204,205,206,207,208,209,210,211,212,213,215,216,217,300,301,303,304,307,313,600,602,603,606,609,610,750,753,754,756,757,762,766.
- External assurance and reliance: 108,109,111,302,308,309,310,311,312,500,501,601,604,612,701,705,755,758,759,763,765.
- Technical design and actual configuration: 306,502,503,504,505,506,507,508,509,510,611,614,615,616,700,702,703,704,706,707,708,709,710,751,752,760,761,764.
- Governance and contradictions: 607,608,800,801,802,803,804,805,806,807.

## Roadmap and exit criteria
P0 reconciliation: read all existing policies, catalog, architecture and batch specs; build bidirectional requirement/control/test/evidence crosswalk; resolve contradictions with decisions; update question register without falsely closing external dependencies; validate references and counts. Produce revised economic model with common scope and actual assumptions. Owner approves baseline.
P1 foundations: verify actual repo settings and tool capabilities; define identity, authorization, policy, evidence and test interfaces; threat models and synthetic fixtures; establish protected release gates. Exit: reproducible negative tests and authenticated receipts in reference environment.
P2 Unbound reference: financial invariants, tenant isolation, durable processing, reconciliation, continuous security tests and evidence; integrate existing systems where verified. Exit: adversarial, recovery and concurrency tests with traceable evidence; no production claims from synthetic tests.
P3 operational readiness: actual workforce/admin controls, monitoring, retention, incident response, continuity, customer commitments, provider contracts and regulatory applicability. Exit: verified deployed controls, accountable roles, risk-tier validation and no unresolved mandatory launch blockers.
P4 external readiness: synthetic and actual evidence packages as appropriate, scope with qualified auditor/customer, obtain quotes and required independent assessments, observation period only after controls operate. Exit: external requirements satisfied, no unsupported attestation.
P5 scale: reuse shared controls, measure incremental cost and questionnaire effort, expand only with justified demand.

## Reconciliation deliverables
Canonical control catalog with stable IDs and child requirements; traceability matrix; updated policies and decision register; question dispositions with explicit external dependencies; capability verification matrix; revised economics; prioritized roadmap; validation report and change log. Use existing register checks and Praxis governance; migrate Python tooling to F# when authorized, not by silently bypassing checks. No other repository changes, purchases or implementation authorized. This document records challenge findings and routing only; full artifact-level reconciliation and tests remain outstanding.

# Executable policy and continuous evidence
Date: 2026-11-25
Status: Proposed detailed requirements under approved assurance foundations. No implementation or audit acceptance claim.

## Policy requirements
| ID | Requirement | Verification |
|---|---|---|
| PEC-001 | One authoritative versioned policy model in F#, with stable policy/control IDs, scope, owner, rationale, source, applicability, effective date, obligations and exceptions | Schema and referential integrity tests |
| PEC-002 | Distinguish executable decisions, configuration constraints, scheduled obligations, human judgment and external obligations; never pretend prose is executable | Classification and coverage review |
| PEC-003 | Deterministic pure typed policy decisions with explicit inputs, outcomes, reasons and policy version; separate effects | Property tests, replay, boundary cases |
| PEC-004 | Generate human-readable policy, AI guidance, enforcement specifications, tests and evidence obligations from authoritative definitions where feasible; explicitly link non-generated content | Drift detection and semantic review |
| PEC-005 | Signed authenticated approval binds exact policy digest, scope and effective date; preserve prior versions | Reject forged, stale and mismatched approvals |
| PEC-006 | Changes require impact analysis, regression tests, approval and controlled activation; no agent self-approval | Attempt unauthorized activation and rollback |
| PEC-007 | Define precedence: mandatory platform/legal constraints cannot be weakened by tenant policy; unresolved conflicts deny or route to review | Conflict and tenant override tests |
| PEC-008 | Versioned, bounded exceptions with reason, risk, compensating controls, approver and expiry; no self-approved material exception | Expiry and bypass tests |
| PEC-009 | Explicit enforcement point and failure mode per policy; fail closed for authorization and financial release; avoid unsafe blanket fail-closed for availability | Dependency outage tests |
| PEC-010 | Human obligations have accountable role, due date, escalation and completion evidence; reminders are not proof | Missed obligation simulation |
| PEC-011 | Historical decisions reproducible using effective policy and input references; corrections preserve history | Replay and version transition tests |
| PEC-012 | Policy changes and validation must remain functional without AI services | Disable model provider in test |

## Evidence requirements
| ID | Requirement | Verification |
|---|---|---|
| PEC-013 | Emit evidence at authoritative enforcement point, not from agent narrative | Compare source events and receipts |
| PEC-014 | Standard envelope: evidence/control/policy IDs, event and collection timestamps, source identity, subject scope, artifact/config digest, outcome, provenance, classification, retention, limitations | Schema, signature and replay tests |
| PEC-015 | Distinguish design, implementation, internal verification, independent assessment and attestation; never promote status from mere document existence | Invalid status transition tests |
| PEC-016 | Evidence completeness reconciled against independent expected populations and sequence/checkpoint records; detect gaps and duplicates | Drop/reorder/duplicate event tests |
| PEC-017 | Freshness and expiry per evidence type; stale, missing or revoked evidence cannot be reported as current passing assurance | Clock and expiry tests |
| PEC-018 | Separate evidence generation, collection, storage and verification privileges; authenticated collectors; no ordinary application or agent overwrite | Access and tampering tests |
| PEC-019 | Managed durable storage with appropriate immutability, retention and isolated trust domain; GitHub not sole production evidence store | Restore, delete-denial and integrity tests |
| PEC-020 | Minimize sensitive payloads; separate deletable personal data from minimal integrity metadata; no secrets or raw customer records in public evidence | Redaction, disclosure and retention tests |
| PEC-021 | Legal holds override ordinary expiration; approved bounded disposal with evidence; no assumption crypto-shredding legally satisfies erasure | Hold and deletion conflict tests |
| PEC-022 | Independent heartbeat and reconciliation detect collector silence; evidence failure alerts and risk-based operational response | Stop collector and verify external detection |
| PEC-023 | Reproducible auditor packages include population definition, query/version, completeness and accuracy checks, evidence manifest, exceptions and limitations | Independent regeneration and digest comparison |
| PEC-024 | Public assurance outputs disclose scope, date, method, result and limitations; no confidential evidence or misleading certification claims | Disclosure review |
| PEC-025 | Evidence capture must not silently lose accepted financial events; transactional outbox or equivalent durable linkage, bounded retries and backpressure | Crash and recovery tests |
| PEC-026 | Verify the assurance system itself: deliberately disable controls in safe environments, test privilege bypass, collector gaps and compromised inputs | Mutation and adversarial tests |
| PEC-027 | Track control effectiveness separately from evidence availability; missing evidence is unknown, not pass | Dashboard and status tests |
| PEC-028 | Maintain portable evidence export and documented schemas; defer specialized formats until demand | Round-trip and schema compatibility tests |

## Question dispositions
- OQ-108, 312, 500, 510, 705, 759, 765: design source-authenticated, reproducible evidence with independent population reconciliation and documented IT general controls. Auditor reliance remains open; no commercial platform presumed necessary.
- OQ-501: signed policy digest plus rendered version and approval record is proposed internal policy of record; support separately signed document if required. Auditor acceptance open.
- OQ-502, 503, 762: record-class retention and holds; separate personal payloads from minimal metadata. Research legal erasure requirements; do not assert crypto-shredding is sufficient.
- OQ-504, 505, 507, 805: provider-neutral managed immutable storage interface; evaluate cost, retention, key custody and independent anchoring. Defer public witness network and qualified timestamp purchases until justified; do not publish sensitive metadata.
- OQ-506: canonical structured export first; OSCAL adapter only when justified by customer/regulatory demand.
- OQ-508: verify current GitHub plan capabilities and compare incremental cost against evidence coverage; do not purchase or assume enterprise features. Document gaps explicitly.
- OQ-509: prefer F# pure typed authorization decisions; compare Cedar only if demonstrable assurance or interoperability benefit outweighs integration cost. No new runtime dependency approved.
- OQ-703, 804: managed keys, separated principals, protected deletion, alerts and recovery; actual custodian/provider and human independence remain open.
- OQ-751, 758: Folio renders packages; evidence population and validation belong to authoritative source and verification layers.
- OQ-802, 803: risk-based automation and validation; align catalog, roadmap and economics; do not equate automation with independence.

## Integration and next steps
Inspect existing policy-as-code specification and evidence architecture before reconciliation. Map PEC IDs to existing controls and requirements rather than duplicating catalog entries. Praxis governs changes; Dokimos verifies; Vigila observes; Arca supports evidence integrity; Folio renders; Strata controls database access. Verify capabilities and distinguish proposed from implemented. Prefer managed services and existing systems, no new platform by default. F# implementation and tests required when authorized. Define interfaces and synthetic evidence fixtures first. Reconcile policies, catalog, questions, risks, roadmap and economics; run governance and register checks before merge. No other repository changes or purchases authorized.

---
policy_id: POL-08
title: Incident Response and Notification Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Owner
approval_authority: Board or sole owner
review: annual and after every major incident
scope: security, privacy, availability, financial-integrity and AI incidents affecting Echelon or its customers
authoritative_references: [TSC CC7.3-CC7.5, 16 CFR 314.4(h) and (j), 16 CFR 314.2 (notification event), state breach notification statutes, customer and bank partner contracts, NIST SP 800-61r3]
---

# POL-08 Incident Response and Notification Policy

## 01 Incident response
1. Anyone (person, monitor or agent) may declare a suspected incident. Declaration creates an incident record with a timeline that is append-only.
2. Severity is assigned using defined criteria (data exposure, financial impact, customer impact, availability). Each severity has an incident commander, response deadlines and communication rules.
3. Phases: detect, triage, contain, eradicate, recover, review. Containment actions that are reversible may be automated; irreversible actions require the incident commander.
4. Every high or critical incident receives a written post-incident review with corrective actions tracked to closure.
5. The plan is exercised at least annually (tabletop) and the exercise produces evidence.

## 02 AI incidents
1. AI incidents include: an agent acting outside its authority, prompt injection with effect, data sent to an unapproved provider, fabricated or altered evidence, model or provider compromise, and unsafe autonomous remediation.
2. Every agent has a kill switch (credential revocation plus execution halt) operable by on-call humans without the agent's cooperation.
3. Agent execution records relevant to an incident are preserved under legal hold.

## 03 Notification
1. A notification register lists each obligation: who must be notified (customers, bank partners, regulators, individuals, law enforcement), trigger, deadline and owner. It is populated from laws (e.g. FTC Safeguards Rule: notice to the FTC within 30 days of discovering a notification event involving at least 500 consumers, where the rule applies; state breach statutes) and from contracts (bank and customer notice periods are often much shorter).
2. The incident record computes notification deadlines from the discovery time; approaching deadlines escalate automatically.
3. Decisions not to notify are recorded with reasoning. Where legal judgment is involved, the record cites the primary sources relied on, states that internal research is not legal advice, and records whether external advice was obtained; obtaining it in an actual incident is a recorded trigger, not a standing engagement (DF-ASR-2026-0D0F, ODP-12).
4. Customers acting as controllers are notified so they can meet their own obligations.

## Change history
| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft | Not approved |

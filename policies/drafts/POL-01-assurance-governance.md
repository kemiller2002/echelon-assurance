---
policy_id: POL-01
title: Assurance Governance Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Owner (acting Qualified Individual / security lead)
approval_authority: Board or sole owner
effective_date: null
review: annual, and on any material change in business, law, or a selected framework
scope: Echelon Foundry, all products (Unbound first), all personnel, contractors and AI agents
applicable_systems: all
authoritative_references: [TSC CC1-CC5, 16 CFR 314.4(a)-(b) and (i), NIST CSF 2.0 GV, ISO/IEC 27001 cl. 4-10]
---

# POL-01 Assurance Governance Policy

## Purpose

Establish one accountable program under which security, compliance, internal
control and risk are governed, so that every obligation Echelon Foundry
accepts is traceable to a control, an owner and evidence.

## 01 Objectives, roles and responsibilities

1. Echelon Foundry maintains a written information security program
   consisting of the policies POL-01 to POL-13, the control catalog and the
   registers in `echelon-assurance`.
2. One accountable role (held by the owner) is responsible for the program
   (the "Qualified Individual" in FTC Safeguards terms where that rule
   applies). An emergency delegate role is defined so that absence does not
   stop urgent action; the individual is designated when operationally
   necessary (owner direction DF-ASR-2026-0D10, ODP-04).
3. Every control has an accountable human role. AI agents may operate controls
   but are never owners.

## 02 Risk management

1. A risk register is maintained in machine-readable form and reviewed at
   least annually and on material change.
2. Risk assessment covers security, privacy, financial integrity, fraud,
   availability, vendor, AI and regulatory change risks, and the failure of
   the assurance system itself.
3. Risk treatment decisions (mitigate, accept, transfer, avoid) are recorded
   with the approver; acceptance above the defined threshold requires the
   approval authority of this policy.

## 03 Compliance management

1. Obligations are recorded as requirements with an applicability
   classification (legal, contractual, framework-selected, customer
   expectation, voluntary, not applicable, undetermined) and the reasoning.
2. Undetermined applicability is not resolved by assumption. It is researched
   from primary sources with multiple-agent analysis, recorded as an open
   question with a prepared question package, and the affected capability is
   gated from activation. External advice is reassessed when legally required, commercially
   necessary, or when material uncertainty cannot otherwise be resolved (owner direction
   DF-ASR-2026-0D0F, ODP-12). Internal research is never represented as a legal
   opinion.
3. Changes to authoritative sources are monitored; a change creates a review
   item against affected requirements, controls and policies.
4. No person or agent represents a compliance status, certification or
   attestation that is not supported by an issued report or recorded
   evidence.

## 04 Internal control

1. Controls are recorded in the unified control catalog with the fields
   defined there; one control may satisfy many requirements.
2. Control status follows the fixed vocabulary; no status is inferred from
   another.
3. Preventive technical enforcement is preferred over detective and manual
   controls where practical and where the failure mode is acceptable.
4. Every control has a validation procedure with a stated independence level.
5. Deficiencies are recorded, rated and remediated with a deadline.

## 05 Exceptions and risk acceptance

1. An exception records: the requirement or control affected, scope, reason,
   risk, compensating controls, expiry (maximum 12 months), requester and
   approver.
2. The approver may not be the requester or anyone reporting solely to the
   requester (CTL-GOV-007); no agent may approve an exception, and exceptions
   requested by agents are approved by the owner. In particular, no material
   exception is self-approved: an owner-originated material exception is
   blocked, or the action redesigned, until a separate qualified approver
   exists (BR-GOV-004, DF-ASR-2026-0D0F).
3. Expired exceptions automatically become control failures.

## 06 Delegation of authority

1. Approval authorities are kept in a machine-readable authority matrix that
   enforcement mechanisms consume directly.
2. Changes to the authority matrix are themselves approved changes under
   POL-06.

## 07 Segregation of duties

1. Incompatible duties are defined in a matrix covering humans, service
   identities and agents, including: authoring and approving the same change;
   initiating and approving the same financial transaction; operating a
   control and performing its independent validation; administering the
   evidence store and being subject to the evidence it holds.
2. Routine low-risk work proceeds under a standing human authorization with
   a separate implementing agent, a technical verifier and an enforced release
   gate. Material policy changes, privilege changes, financial exceptions and
   changes to the verifier require elevated human approval, as do the further
   elevated classes of BR-SDL-003 listed in POL-06.02. Where an
   independent human review is required and no second person exists, the
   action is blocked or redesigned; self-approval of material exceptions is not
   authorized, and no contracted reviewer is engaged at the development stage
   (owner direction DF-ASR-2026-0D0F; reassessed when required). Technical independence of a
   verifier is not organizational independence and is never described as such
   (owner decision DF-ASR-2026-0D04).

## 08 Policy lifecycle and records

1. Policies are maintained as structured, version-controlled definitions;
   human-readable documents are generated from them.
2. Policy changes require approval by the policy's approval authority; the
   approval is recorded as an attributable event.
3. Records required by law or contract are retained per POL-05.03.

## 09 Management review and reporting

On material change, and monthly while operations are active (minimum annually; `governance/operating-model.md`), the owner reviews: control health metrics, failed and
overdue controls, open exceptions, risk register changes, incidents, vendor
changes and regulatory changes. The review and its decisions are recorded.

## Enforcement and validation

Enforcement: authority matrix consumed by repository rules and application
authorization; exception register with expiry automation. Validation: multiple-agent review of the program (VAL-041), with internal-independent review only when an organizationally separate performer exists (ODP-02); external attestation per the
roadmap.

## Change history

| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft by research program | Not approved |

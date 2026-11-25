---
policy_id: POL-10
title: Third-Party and Vendor Risk Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Owner
approval_authority: Board or sole owner
review: annual
scope: vendors, subservice organizations, AI/model providers, open-source critical dependencies, bank and payment partners
authoritative_references: [TSC CC9.2, 16 CFR 314.4(f), Interagency Guidance on Third-Party Relationships: Risk Management (June 2023) — applies to banks and flows down contractually, DC 200 subservice organization disclosures]
---

# POL-10 Third-Party and Vendor Risk Policy

## 01 Vendor and subservice organization management
1. A vendor inventory records: service, data classes accessed, criticality, whether it is a subservice organization for any Echelon system description, contract, owner and review date.
2. Before onboarding a vendor with Confidential or Restricted data access: security due diligence proportionate to criticality (SOC 2/ISO certificate review, questionnaire only where no report exists), contractual security and notification terms, data processing terms.
3. For each vendor SOC report relied on: review the opinion, exceptions, period, and map its complementary user entity controls (CUECs) to Echelon controls. Unmapped CUECs are gaps.
4. Critical vendors are reviewed at least annually; reports with expired periods require a bridge letter or follow-up.
5. Offboarding a vendor removes its access and confirms data return or deletion.

## 02 AI model and provider risk
1. AI providers are approved per data class, with recorded terms on training use, retention, residency, sub-processors and security attestations.
2. No single AI provider is a dependency for a control that blocks or approves actions; agent workflows have a documented degraded mode.
3. Provider or model changes for agents that operate controls are treated as changes under POL-06 and re-evaluated under POL-11.07.

## 03 Bank and financial institution partners
1. Obligations imposed by bank and payment partners (security requirements, audits, notification deadlines, right-to-audit, business continuity, complaint handling) are recorded as contractual requirements and mapped to controls.
2. Echelon anticipates that bank partners will assess Echelon as a third party under their own risk management programs and prepares evidence accordingly.

## Change history
| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft | Not approved |

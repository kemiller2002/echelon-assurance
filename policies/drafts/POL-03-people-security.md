---
policy_id: POL-03
title: People Security Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Owner
approval_authority: Board or sole owner
review: annual
scope: employees, contractors, and anyone granted access to nonpublic systems
authoritative_references: [TSC CC1.4, CC2.2, 16 CFR 314.4(e), NIST CSF 2.0 PR.AT]
---

# POL-03 People Security Policy

## 01 Onboarding and screening
1. Access is granted only after: identity verification, signed confidentiality terms, POL-02 acknowledgement and role assignment.
2. Background screening is proportionate to role risk and permitted by law. Roles with production, financial-approval or evidence-administration access require screening before access is granted.
3. Access is granted by role, through the identity provider; ad-hoc grants follow POL-04.01.

## 02 Security awareness and training
1. Everyone completes security awareness training within 30 days of start and on material change. Roles with elevated access complete role-specific training (secure development, financial controls, incident response) before receiving that access.
2. Training content is updated when incidents, threats or rules change. The FTC Safeguards Rule requires security awareness training for personnel where it applies (16 CFR 314.4(e)).
3. The training system records completion; training completion gates elevated access automatically where feasible, rather than relying on reminders.

## 03 Offboarding
1. A separation event in the HR source of record triggers revocation of all access, including service tokens and agent identities that person owns or sponsors.
2. Privileged access is revoked the same day as separation; all other access within one business day.
3. Revocation is verified automatically against every in-scope system's entitlement list; discrepancies are control failures.
4. Company devices and data are returned or remotely wiped.

## 04 Role changes
A role change is processed as a revocation of the old role and grant of the new role, so accumulated access does not persist.

## Change history
| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft | Not approved |

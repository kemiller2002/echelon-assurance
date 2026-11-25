---
policy_id: POL-04
title: Identity and Access Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Engineering lead
approval_authority: Board or sole owner
review: annual
scope: human, service (workload) and AI agent identities on all systems holding nonpublic data or able to change production
authoritative_references: [TSC CC6.1-CC6.3, 16 CFR 314.4(c)(1) and (c)(5), NIST SP 800-63B, NIST SP 800-53 AC/IA, CIS Controls 5-6]
---

# POL-04 Identity and Access Policy

## 01 Identity lifecycle and provisioning
1. Every actor has a unique identity: one per human, per workload, per AI agent. Shared accounts are prohibited except documented break-glass accounts.
2. Access is granted by role with least privilege, approved by the resource owner, and recorded as an event. Access definitions are kept as code where the platform supports it.
3. Every service and agent identity has a named human sponsor and an expiry or review date.

## 02 Privileged access
1. No standing human administrative access to production. Privileged access is just-in-time, time-bound (default ≤ 4 hours), approved by a second person, and logged.
2. Break-glass accounts are sealed, alert on use, and every use is reviewed within one business day.
3. Privileged sessions on financial data or the evidence store are recorded.

## 03 Authentication and MFA
1. Human access to any system containing nonpublic information requires MFA; phishing-resistant authenticators (FIDO2/WebAuthn passkeys) are required for privileged access and for the identity provider itself.
2. Single sign-on through the corporate identity provider is required wherever the system supports it. Systems that cannot use SSO are recorded as exceptions with compensating controls.

## 04 Passwords and credentials
1. Where passwords remain, the identity provider enforces NIST SP 800-63B rules: minimum length, breached-password screening, no composition rules or periodic forced rotation absent evidence of compromise.
2. Credentials are never shared, written in code, tickets, chat or documents.

## 05 Secrets
1. Secrets are stored only in the approved secret manager, with access restricted per workload.
2. Workload identity federation (short-lived tokens) is preferred over long-lived static secrets; static secrets have an owner and rotation schedule.
3. Repositories use secret scanning with push protection; a detected secret is treated as compromised and rotated.

## 06 Access review
1. Entitlements are compared continuously against the approved access definitions; differences raise alerts and are either approved or removed.
2. Dormant accounts (no use in 45 days) are disabled automatically, unless excepted.
3. Privileged roles, service identities and agent identities are additionally attested by their owners each quarter. The attestation is not a manual export review: owners confirm the automatically generated difference report.

## Failure behavior
Authentication and authorization fail closed. If the identity provider is unavailable, no new sessions are issued except via break-glass.

## Change history
| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft | Not approved |

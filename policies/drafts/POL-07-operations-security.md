---
policy_id: POL-07
title: Operations Security Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Engineering lead
approval_authority: Board or sole owner
review: annual
scope: cloud accounts, networks, endpoints, monitoring and security testing for all environments
authoritative_references: [TSC CC6.6-CC6.8, CC7.1-CC7.2, 16 CFR 314.4(c)(8) and (d), NIST SP 800-53 CM/RA/SI/AU/CA-8, CIS Controls 4, 7, 8, 12, 13, 18, PCI DSS 11.4 (only if in scope)]
---

# POL-07 Operations Security Policy

## 01 Configuration management
Secure baselines for cloud services, identity provider, repositories and endpoints are expressed as policy-as-code and evaluated continuously. Deviations are remediated automatically only where remediation is reversible and bounded (see `automation/`); otherwise raised for human action.

## 02 Network and cloud
1. Prefer managed services with private connectivity; no public administrative endpoints.
2. Organization-level guardrails prevent disabling audit logging, creating static access keys, making storage public, or deploying outside approved regions.
3. Ingress to applications passes through managed TLS termination and, where justified by risk, a web application firewall.

## 03 Endpoints
Devices accessing nonpublic data are managed: full-disk encryption, screen lock, automatic OS updates, and endpoint protection proportionate to risk. Device posture is a condition of access where the identity provider supports it.

## 04 Vulnerability management
1. Code, dependencies, containers and cloud configuration are scanned continuously.
2. Remediation deadlines by severity (initial targets, to be confirmed against customer contracts): critical 7 days, high 30 days, medium 90 days, low best effort. Overdue items without an approved exception are control failures.
3. Reachability and exploitability may lower priority only through a recorded decision.

## 05 Penetration and security testing
1. Internal security testing (automated and manual) runs continuously.
2. Independent external penetration testing is performed when reassessment against risk and actual customer, contractual, insurer or regulatory obligations requires it, and then at the frequency those obligations set (commonly annually and after significant change), by testers independent of the development team. At the development stage external testing is deferred and comprehensive continuous internal testing applies (owner direction DF-ASR-2026-0D0F).
3. Automated scanning is never represented as a penetration test. External reports, if any test is performed, are restricted; customers then receive attestation letters.
4. A public vulnerability disclosure policy and security.txt are maintained.

## 06 Logging, audit trails and monitoring
1. Security-relevant events are logged: authentication, authorization denials, administrative and privileged actions, data export, configuration change, agent actions, financial approvals.
2. Logs exclude secrets and minimize personal data. Logs are protected from alteration and retained per POL-05.03.
3. Expected log sources emit heartbeats; a missing source or gap is itself an alert.
4. Alerts route to a responder with an acknowledgement deadline.

## 07 Physical security responsibilities
Echelon operates no datacenters or offices in scope. Physical security of infrastructure is inherited from cloud providers and evidenced by their SOC reports (carve-out). Personnel protect devices in remote work locations per section 03.

## Change history
| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft | Not approved |

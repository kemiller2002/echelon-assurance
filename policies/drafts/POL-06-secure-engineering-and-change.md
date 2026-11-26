---
policy_id: POL-06
title: Secure Engineering and Change Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Engineering lead
approval_authority: Board or sole owner
review: annual
scope: all source code, infrastructure definitions, configuration, policy definitions and AI agent definitions that affect production or assurance systems
authoritative_references: [TSC CC8.1, CC7.1, NIST SP 800-218 (SSDF), SLSA v1.0, OpenSSF Scorecard, 16 CFR 314.4(c)(4) and (c)(7), OWASP ASVS, OWASP SAMM]
---

# POL-06 Secure Engineering and Change Policy

## 01 Secure development lifecycle
1. Material changes (new data categories, external integrations, authorization changes, financial logic, autonomous agent permissions) require a recorded threat model or design review before implementation.
2. Security requirements are expressed as tests where possible (OWASP ASVS level chosen per system risk).
3. Application code meets the security-by-construction requirements SBC-001..018 (`requirements/security-by-construction.md`), implemented by controls CTL-APP-001..010 and mapped existing controls. Each by-construction claim is valid only within its verified boundary.
4. Evidence by construction: each new system or material change records, at
   design review, the evidence its operation emits for the controls it
   affects, how that evidence protects privacy, and how failure to emit is
   detected (TNT-17, BR-ASR-012).

## 02 Change, code review and release
1. All changes to protected branches go through a pull request with at least one approving review by someone other than the author, and all required status checks passing. Administrators cannot bypass these rules; repository rules are themselves defined as code.
2. AI-authored changes are marked with provenance and require human approval; an agent may not approve a change it authored or that its sponsor authored on its behalf.
3. Releases are produced only by the CI pipeline from reviewed commits; the deployed artifact digest is recorded and traceable to the commit and pull request.
4. Production state that does not trace to an approved release is an incident.

## 03 Emergency changes
1. Emergency changes may bypass pre-merge review only through a defined break-glass path that alerts a second person.
2. Every emergency change receives retrospective review within two business days. Missing review is a control failure.

## 04 Dependencies, supply chain, signing and provenance
1. Dependencies are pinned and locked; new direct dependencies are reviewed (license, maintenance, security posture).
2. An SBOM is generated per release; known-vulnerable dependencies follow POL-07.04 deadlines.
3. Build artifacts are signed and carry build provenance (target SLSA Build Level 2 initially, Level 3 for financial systems as the roadmap allows).
4. Deployment verifies signature and provenance before release (fail closed).

## 05 Testing
1. Every change includes tests. Financial and authorization logic requires negative tests and property-based tests of stated invariants.
2. CI fails on an empty or skipped test run.

## 06 Environments and production access
1. Development, test and production are separate accounts/projects. Production data is not copied to non-production environments; synthetic data is used.
2. Developers have no write access to production data; changes reach production only through the pipeline.

## 07 Architecture governance
New infrastructure, runtime dependencies, data stores or external services require an architecture decision record. Echelon principles apply: F#-first, minimal infrastructure, managed services, monolith where appropriate.

## 08 Infrastructure configuration
All infrastructure is defined as code and reviewed like application code. Drift between declared and actual state is detected continuously and either reconciled or raised as an unapproved change.

## 09 Third-party software and services
New SaaS or software with access to Confidential or Restricted data is approved under POL-10 before use.

## Change history
| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft | Not approved |

---
policy_id: POL-11
title: AI and Autonomous Systems Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Owner
approval_authority: Board or sole owner
review: semiannual
scope: every AI agent, model integration and automated decision mechanism operated by or on behalf of Echelon Foundry
authoritative_references: [NIST AI RMF 1.0 (GOVERN, MAP, MEASURE, MANAGE), NIST AI 600-1, ISO/IEC 42001, OWASP Top 10 for LLM Applications 2025, OWASP Agentic AI guidance]
---

# POL-11 AI and Autonomous Systems Policy

## Principle
Agents are powerful operators, never accountable owners and never independent assurers. Their permissions are enforced by mechanisms outside the agent, not by instructions given to the agent.

## 01 Identity, authorization and credentials
1. Each agent has a distinct identity, a named human sponsor and a declared purpose.
2. Agent permissions are least privilege, scoped to the purpose, and enforced by the platform (identity provider, repository rules, application authorization), not by prompts.
3. Agent credentials are short-lived and issued per execution where possible; agents never hold human credentials or long-lived administrator keys.

## 02 Execution boundaries and human approval
Agents may never:
- approve their own work, exceptions, or changes they authored;
- create, alter or delete historical evidence;
- disable, weaken or bypass a control or monitoring;
- change a policy, authority matrix or control definition without human approval;
- release funds, approve grants, or change financial records outside authority explicitly delegated in POL-12;
- send Confidential or Restricted data to a provider not approved for that class;
- represent internal or automated validation as external audit or attestation.

Each prohibition is backed by a technical mechanism listed in `ai-governance/` (permission absent, rule enforced outside the agent, or detection with alert). Actions not listed may be delegated per agent in its definition; consequential actions require human approval.

## 02a Authority tiers
Agents operate within owner tiers A0–A4 (`requirements/ai-governance-independent-validation.md`): A0 read authorized data; A1 propose; A2 bounded low-risk execution; A3 material operations only through independently enforced human authorization and deterministic gates; A4 reserved human authority (policy weakening, material exceptions, risk acceptance, privileged access, management assertions). No agent independently authorizes movement of funds or material exceptions. If required independent approval is unavailable, the action is blocked or redesigned.

## 03 AI-generated code
AI-generated changes follow POL-06 in full and carry provenance bound to the agent's authenticated platform identity and signed execution records (owner decision DF-ASR-2026-0D08). Routine low-risk changes proceed under a standing human authorization with a separate technical verifier and an enforced release gate; material policy, privilege, financial-exception and verifier changes require elevated human approval (DF-ASR-2026-0D04). An agent never approves its own work.

## 04 Untrusted input and prompt injection
Content retrieved from outside the trust boundary (web pages, documents, tickets, customer input, repository files) is treated as data. Agents processing untrusted content do not hold permissions to take consequential actions or to send sensitive data outside the boundary in the same execution; consequential actions require a separate, authorized step. Tool authorization is enforced outside the model and containment is tested adversarially and continuously (ODP-08, DF-ASR-2026-0D10; BR-AI-004). The design approval is not acceptance of residual risk (OQ-616).

## 05 AI data handling
Data sent to providers follows POL-05.01 and POL-10.02. Prompts and outputs containing Restricted data are retained only as required for evidence and are access-controlled.

## 06 AI-generated evidence and autonomous remediation
1. Evidence is produced by the system where the event occurred, not written by an agent. Agent summaries are labeled as analysis and link to source evidence.
2. Autonomous remediation is permitted only for actions that are bounded in blast radius, pre-approved by type, checked before execution, and logged; irreversible actions are permitted only for approved routine classes (such as retention expiry with legal-hold checks), never for evidence, keys or financial records; remediation is disabled automatically after repeated failures (owner decision DF-ASR-2026-0D09).

## 07 Accountability, evaluation and monitoring
1. Each agent's sponsor is accountable for its behavior.
2. Agents operating controls are evaluated before deployment and after model/provider changes against a test suite including adversarial cases.
3. Agent actions are logged with identity, model/provider and version, inputs by reference, tools used and outcomes; anomalies are monitored.
4. If AI providers are unavailable, controls continue to operate deterministically; agent-assisted tasks queue for humans.

## Change history
| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft | Not approved |

---
id: ASR-CUS-SPEC-001
title: Customer assurance knowledge base — requirements specification
status: proposed
created: 2026-11-25
requirements: [BR-CUS-001, BR-CUS-002, BR-CUS-003, BR-ASR-002, BR-ASR-004, BR-AI-008]
related: trust-center/customer-trust-center-specification.md (public interface detail), customer-assurance/findings.md (research)
---

# Customer assurance knowledge base

## Register

`question-bank.csv` holds 97 canonical, deduplicated questions. They are drawn
from:

- financial-institution third-party risk guidance (2023 Interagency Guidance, SR 24-7);
- DAF sponsor diligence (inferred: no sponsor publishes a questionnaire);
- enterprise procurement (CAIQ, SIG and VSA, public summaries only);
- cyber-insurer applications;
- framework report requests.

Sources are in `sources/fragments/CAQ.csv`. The validator checks every reference.

Each record carries:

- applicability;
- proposed answer and answer basis;
- related policy, control and evidence type;
- implementation status;
- evidence freshness rule;
- disclosure class;
- approval requirement.

## Requirements

| ID | Requirement |
|---|---|
| CA-KB-1 | Answers are generated from the control catalog, baseline and evidence status wherever feasible (`answer_basis = generated-*`). Manual answers name their source decision or open question. |
| CA-KB-2 | No answer claims a status above the controls it cites. The validator rejects `operating` answers whose controls are below `Operating` (BR-CUS-001). As of 2026-11-25 every answer is `designed`, `not-started` or `unknown`. |
| CA-KB-3 | Unknown is not yes. Answers that depend on open questions say "Not yet determined" and cite the question. |
| CA-KB-4 | Each answer has a freshness rule. An answer whose evidence is stale or revoked reverts to unknown (BR-ASR-004). |
| CA-KB-5 | Disclosure classes are `public`, `customer-nda` and `restricted`. Answers about planned or missing preventive controls are never public (due-diligence rule TC-RED-003). |
| CA-KB-6 | Release requires approval: owner for all answers; owner after recorded legal research (primary sources and multiple-agent review; external advice only on a recorded trigger, DF-ASR-2026-0D0F) for legal, notification, insurance, data-location and contractual answers. No answer is approved as of 2026-11-25. |
| CA-KB-7 | Customer-specific questions not in the bank are answered manually, then considered for addition after two occurrences. Volume and effort are tracked (BR-GOV-015). |
| CA-KB-8 | Material AI involvement in generated answers is disclosed to reviewers (BR-AI-008). Generated answers cite sources (BR-ASR-011). |
| CA-KB-9 | The due-diligence repository's controls (ECTL, 642) and evidence requirements (EVR, 617) are crosswalked to this catalog before answers from either are reused (WI-ASR-04). The broken references BND-06 and DDT-04 in `requirements/continuous-assurance-requirements.md` are corrected through that crosswalk. |

## Public assurance interface

**Public:**

- standards and tenets;
- methodology;
- control objectives;
- policy summaries;
- assurance status ladder definitions;
- sanitized, dated results with scope and limitations (for example, internal assessment report summaries with counts only);
- the vulnerability disclosure policy;
- the AI use statement.

**Restricted:**

- production evidence;
- configurations;
- credentials;
- customer information;
- finding details and exploitable vulnerability information;
- SOC reports (restricted-use by design);
- penetration test reports;
- contract terms.

Because no control operates yet, only one question-bank answer is public; the public page carries program-level content (standards, methodology, status definitions) from documentation rather than from control answers. Every public artifact passes the classification filter and the independence lint
(BR-CUS-002). Internal automation is never described as certification,
attestation or audit.

## Known gaps (from research)

- **No control or policy yet for:**
  - insurance coverage;
  - an incident-response retainer;
  - government requests for customer data;
  - staff and AI agent location;
  - accessibility;
  - customer single sign-on.
- **Numeric commitments not yet set:**
  - deprovisioning window;
  - remediation windows;
  - breach notice window;
  - RTO/RPO;
  - availability SLA;
  - retention periods.

  These are owner decisions recorded through the obligation register (BR-GOV-009) once customer contracts exist; they are not invented here.
- **Timing.** Unbound XID-INV-008 resolves that no SOC 1 type 2 is universally required before the first outsourced sponsor (its "Type I at launch" text is only the original recommendation). Here, decision 0D02 sets the design target at SOC 1 and SOC 2 type 2; examination timing remains open (OQ-101, OQ-110) and is not part of WI-UNB-08.

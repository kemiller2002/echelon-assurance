---
id: ASR-TRUST-SPEC-001
title: Customer trust center specification
status: draft
version: 0.1.0
created: 2026-11-25
---

> **Consolidation note (2026-11-25).** The customer assurance knowledge base requirements and the question bank are in `customer-assurance/`. BR-CUS-001 and BR-CUS-002 govern. Disclosure tiers are `public`, `customer-nda` and `restricted`. The due-diligence repository uses six tiers; the crosswalk is WI-ASR-04.


# Customer trust center specification

## 1. Purpose

Answer enterprise and financial-institution due diligence directly from the
authoritative control and evidence model, with tiered disclosure, so that
security questionnaires become a lookup rather than a project.

## 2. Disclosure tiers

| Tier | Audience | Content | Gate |
|---|---|---|---|
| Public | Anyone | Security overview, policy summaries (not full text), subprocessor list, status/uptime, security.txt and vulnerability disclosure policy, SOC 3 report if obtained (SOC 3 is designed for general use) | None |
| Prospect | Verified prospects | Full policy texts, control summaries by domain, pen-test **attestation letter** (only if an external test is obtained; deferred under ODP-11) (scope, dates, tester, severity counts after remediation — not the report), insurance certificate, standard questionnaire answers (SIG Lite / CAIQ) | NDA click-through + identity verification |
| Customer | Contracted customers and their auditors | SOC 1/SOC 2 reports (only once an examination has been performed; none is planned now, DF-ASR-2026-0D0F) (restricted-use: distribution limited to user entities and their auditors per the report's use restriction), bridge letters, CUEC list, data processing terms, incident notifications | Contract + named recipients; watermarked, logged downloads |
| Bank partner | Financial-institution partners | Customer tier plus TPRM-specific artifacts: BCP/DR test summaries, vendor inventory for critical subservices, financial statements if required by contract | Contract; per-request approval |

`authoritative` basis for restricted SOC 1/SOC 2 distribution: AICPA report
use restrictions (see `soc/soc-reporting-landscape.md`).

## 3. Never disclosed through the trust center

Vulnerability details, unremediated findings, internal control deficiency
specifics beyond what an issued report states, network diagrams with
addressing, credentials, other customers' information, raw logs.

## 4. Questionnaire automation

1. Maintain a mapping from common questionnaire standards (SIG/SIG Lite,
   CAIQ for CSA CCM, bank-specific forms, HECVAT if relevant) to control IDs.
   Licensing of SIG content (Shared Assessments) must be checked before
   storing question text (`open-question`).
2. An answer is generated from: control objective + mechanism + current
   status + latest validation result + evidence class. Answers carry the as-of
   date and never claim a status the catalog does not hold.
3. A human approves each outbound questionnaire. Agents draft; they do not
   send (POL-11.02).
4. Novel questions create a gap item: either a new requirement mapped to an
   existing control or a new control proposal.

Expected outcome (`assumption`, to be measured): most questions on standard
questionnaires map to existing controls once the catalog is complete;
residual manual effort is for bespoke contractual and legal questions.

## 5. Build or buy

Trust-center products are commonly bundled with compliance platforms
(`vendors/compliance-platform-analysis.md`). A static, tiered site generated
from the catalog plus an access-controlled document store is likely
sufficient at Echelon's scale (`recommendation`, confidence `medium`); the
deciding factor is NDA/access workflow effort, revisited in `economics/`.

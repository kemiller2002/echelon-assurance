---
id: RS-ASR-2026-SOC1
title: SOC reporting landscape (SOC 1, SOC 2, SOC 3, SOC for Cybersecurity, SOC for Supply Chain)
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
research_key: SOC
confidence: medium
---

# SOC reporting landscape

Research batch key `SOC`. Retrieval date for every fetched source: 2026-11-25.
Sources are `SRC-SOC-NNN` rows in `sources/fragments/SOC.csv`; requirements
are `REQ-TSC-*`, `REQ-SOC1-*` and `REQ-SOCDC-*` in
`regulatory/fragments/SOC.csv`; open questions are `OQ-100`..`OQ-116` in
`registers/fragments/SOC-open-questions.csv`.

Each significant conclusion carries an evidence class in brackets, e.g.
`[authoritative]`. Under the methodology, a conclusion drawn from a source
that was **not read in full** is capped at `auditor-interpretation`.

## 0. What was read, and what was not

| Material | Status in this batch | Effect on conclusions |
|---|---|---|
| AT-C 105, 205, 315, 320 (AICPA codification of SSAEs as of Aug 2026, `ps-at-c-sections.pdf`) | **Read** from the primary text (SRC-SOC-001..004) | SOC 1 and general attestation conclusions can be `authoritative` |
| AU-C 402 and the interpretation in AU-C 9402 (AICPA codification of SASs) | **Read** from the primary text (SRC-SOC-005) | User-auditor conclusions can be `authoritative` |
| TSP section 100, 2017 Trust Services Criteria (revised points of focus 2022) | **Not read.** The AICPA download needs a free account login (SRC-SOC-006). Criterion structure corroborated through secondary sources and AU-C 9402 (which cites CC8.1) | TSC rows are capped at `auditor-interpretation`, summarised in our own words. See OQ-100 |
| DC section 200, 2018 Description Criteria (revised implementation guidance 2022) | **Not read**, same login gate (SRC-SOC-007) | SOC 2 description conclusions are capped at `auditor-interpretation`. See OQ-100 |
| AICPA SOC 2 Guide, SOC 1 Guide, Audit Sampling Guide, SOC for Cybersecurity Guide | **Not read** (paid publications) | Guide content (for example illustrative tests or sample sizes) is not asserted |
| Proposed 2027 TSC and Description Criteria exposure drafts | AICPA news releases read (SRC-SOC-009, -010). Exposure draft PDFs need a login (SRC-SOC-011) | Only the scope and dates of the proposal are stated |
| ISAE 3402 / ISAE 3000 (Revised) | Seen through IAASB/IFAC search summaries only (SRC-SOC-018, -019) | `auditor-interpretation` at most |

## 1. The SOC suite at a glance

The AICPA describes SOC as a suite of services that CPAs may provide, with
reports that help users assess and address the risks of outsourced services
(SRC-SOC-008) `[authoritative]`.

| Report | Standard / criteria | Subject matter | Intended users and distribution |
|---|---|---|---|
| **SOC 1** Type 1 / Type 2 | AT-C 320 (with AT-C 105 and 205). The minimum description criteria are in AT-C 320.15, design criteria in .16, operating-effectiveness criteria in .17 (SRC-SOC-003) | Controls at a service organization likely to be relevant to user entities' internal control over financial reporting (ICFR), organised around **control objectives that management specifies** | **Restricted use**: service organization management, user entities (during the period for Type 2, as of the date for Type 1) and their financial statement auditors (AT-C 320.40n/.41n) `[authoritative]` |
| **SOC 2** Type 1 / Type 2 | AT-C 105 and 205, using DC 200 description criteria and TSP 100 trust services criteria (SRC-SOC-012, SRC-SOC-005 AU-C 9402) | Controls that give reasonable assurance that **service commitments and system requirements** were achieved, based on the applicable trust services criteria for security, availability, processing integrity, confidentiality or privacy | **Restricted** to the service organization and parties with sufficient knowledge of the system: user entities, business partners, practitioners serving them, prospective customers and regulators (SRC-SOC-012) `[auditor-interpretation]` (AICPA comparison table read through a secondary host) |
| **SOC 3** | Same trust services criteria as SOC 2 | Summary opinion; no detailed description of tests and results (SRC-SOC-023) | **General use**, so it can be posted publicly (SRC-SOC-023, a 2016 archived article) `[auditor-interpretation]` |
| **SOC for Cybersecurity** | DC section 100 description criteria plus the trust services criteria for security, availability and confidentiality as control criteria (SRC-SOC-012) | An entity-wide cybersecurity risk management program. Management is responsible for all controls in the program, including those performed by third parties, so no carve-out exists (SRC-SOC-012) | **General use**, though the practitioner may restrict it (SRC-SOC-012) `[auditor-interpretation]` |
| **SOC for Supply Chain** | AICPA Guide (Sep 30 2023) using the 2017 TSC (SRC-SOC-024) | Controls in a **production, manufacturing or distribution** system | Customers and business partners. The page does not state whether use is restricted (SRC-SOC-024) `[auditor-interpretation]` |

**Unbound relevance in one line:** SOC for Supply Chain covers production,
manufacturing and distribution systems, so it does not fit a financial
administration platform `[recommendation]`. SOC for Cybersecurity is
entity-wide and cannot carve out third parties, so it is a poor first choice
for a cloud-hosted platform `[recommendation]`.

## 2. SOC 1 (AT-C 320): what the standard requires

All statements in this section come from the AT-C 320 text read on 2026-11-25
(SRC-SOC-003) `[authoritative]` unless tagged otherwise.

- **Scope.** AT-C 320 covers controls at organizations that provide services
  to user entities, where those controls are likely to be relevant to the user
  entities' ICFR. It complements AU-C 402, so a SOC 1 report may give user
  auditors appropriate evidence (.01). The engagement is an assertion-based
  examination: AT-C 105 and 205 apply in full (.02). AT-C 320 has applied to
  reports dated on or after 1 May 2017 (.06). Its source is still SSAE 18,
  later amended by SSAE 21 and SSAE 23. Secondary sources describing "SSAE 18
  recodified into SSAE 21" are imprecise (see §14).
- **Type 1 vs Type 2.** A Type 1 report covers fair presentation of the
  description and suitability of design **as of a specified date**. A Type 2
  report covers fair presentation, suitability of design and **operating
  effectiveness throughout a specified period**, and includes a description of
  the tests and their results (.08).
- **Preconditions (.10).** The engagement is accepted only if:
  - the scope and description will not be so limited that they are useless to
    user entities and their auditors; and
  - management accepts responsibility for:
    - the description and assertion;
    - having a reasonable basis for the assertion;
    - selecting the criteria;
    - specifying the control objectives, and naming the party that specified
      them if it is not management;
    - identifying risks and designing, implementing and documenting controls;
    - providing a written assertion to user entities.
- **Management assertion is mandatory.** If management refuses to give one,
  the auditor withdraws where possible (.13). If management, or an inclusive
  subservice organization's management, refuses the required written
  representations, that is a scope limitation. It is often enough to preclude
  an unmodified opinion and may lead to a disclaimer or withdrawal (.38).
  Management must also represent that it has disclosed:
  - noncompliance with laws and regulations;
  - uncorrected misstatements; and
  - known or suspected fraud (.36).
- **Minimum description criteria (.15).** The description must cover:
  - the types of services and classes of transactions;
  - the automated and manual procedures that initiate, authorize, record,
    process, correct and report transactions;
  - the information and records used;
  - how significant events other than transactions are captured;
  - how reports for user entities are prepared;
  - subservice organizations, and whether the carve-out or inclusive method
    is used;
  - the control objectives and controls, **including CUECs and CSOCs**;
  - the other COSO components relevant to the services;
  - for Type 2, relevant changes during the period.

  The description must not omit or distort relevant information. It serves
  the common needs of a broad range of users.
- **Design and operating effectiveness criteria.** Design (.16) means
  management identified the risks, and the controls would give reasonable
  assurance that those risks do not prevent the objectives. Operating
  effectiveness (.17) means the controls were applied consistently as
  designed throughout the period. Manual controls must be applied by
  competent people with the appropriate authority.
- **Materiality** covers the fair presentation of the description, the design
  of controls and, for Type 2, their operating effectiveness (.19). A service
  auditor cannot judge materiality from the viewpoint of any single user
  entity (.A46).
- **Internal audit and regulators.** The auditor must understand the internal
  audit function, if one exists (.21). The auditor must also read relevant
  internal audit and regulatory examination reports and consider their
  findings (.23).
- **Testing.**
  - Prior-period evidence gives **no basis for reducing** current testing
    (.28, .A48).
  - Inquiry alone is insufficient: it must be combined with inspection,
    observation or reperformance (.31a).
  - The auditor must consider dependencies on other controls (.31b) and choose
    an effective selection method (.31c).
  - The auditor must investigate deviations and decide whether they fall
    within the expected rate (.32).
  - Deviations caused by fraud trigger a reassessment (.33).
- **Automated controls.** Because IT processing is inherently consistent,
  testing the design and implementation of an **automated application
  control** may also serve as evidence of its operating effectiveness. This
  depends on the auditor's assessment and testing of IT general controls such
  as program-change controls (.A47). This matters for an automation-heavy
  platform.
- **Changes during the period.** Significant changes must appear in the
  description. If they do not, the auditor describes them. Superseded controls
  are tested where possible (.29).
- **Subsequent events** that would mislead users if omitted must be disclosed,
  by the auditor if management does not (.35).
- **Opinion modification (.42–.44).** The opinion is modified (qualified or
  adverse) for any of the following:
  - the description is not fairly presented;
  - the controls are not suitably designed;
  - for Type 2, the controls did not operate effectively;
  - sufficient appropriate evidence could not be obtained.

  A disclaimer is the outcome when the evidence cannot be obtained. Even
  under a disclaimer, known findings are identified, but the procedures are
  not described.
- **Report content (.40/.41).** The report:
  - has a title containing "independent";
  - identifies whether the carve-out or inclusive method was used and,
    for carve-out, states that CSOCs are needed and were not tested;
  - states the need for CUECs, which the auditor did not evaluate;
  - states the independence requirement;
  - states inherent limitations;
  - for Type 2, describes the tests and results;
  - includes the restricted-use alert;
  - is signed by the **firm** and dated no earlier than when sufficient
    evidence was obtained.
- **Duty to communicate.** If the auditor finds noncompliance, fraud or
  uncorrected misstatements that may affect user entities, and management
  refuses to tell those entities, the auditor takes appropriate action (.45).
- **Neighbouring standards.**
  - AT-C 320 guidance can help with controls over **compliance** (for example
    contracts or grants), but those engagements fall under AT-C 205 (.04a).
  - Examinations of **compliance** itself fall under AT-C 315. AT-C 315 does
    not cover reporting on internal control over compliance (AT-C 315.02b,
    SRC-SOC-004).
  - Examining a user entity's transactions or balances, or agreed-upon
    procedures under AT-C 215, are separate engagements (.05).

## 3. The user-auditor side (AU-C 402): why SOC 1 demand exists

The following is drawn from the AU-C 402 and AU-C 9402 text read on
2026-11-25 (SRC-SOC-005) `[authoritative]`.

- **When a service is relevant to the audit (402.03).** It is relevant when it
  affects any of the following:
  - how significant classes of transactions are initiated, recorded,
    processed, corrected and posted to the general ledger;
  - how non-transaction events are captured;
  - the accounting records and specific accounts;
  - the financial reporting process, including disclosures and estimates;
  - the related IT environment.
- **Exclusion (402.05).** AU-C 402 does not apply to processing limited to
  transactions **the entity itself specifically authorizes**, such as a bank
  processing checking-account transactions or a broker processing securities
  trades. In those cases the user entity keeps authorization and
  accountability. This matters for how banks are treated in Unbound's scope.
- **How the user auditor gains understanding (402.12).** If the understanding
  cannot come from the user entity, the user auditor may use:
  - a Type 1 or Type 2 report;
  - information requested from the service organization;
  - a **visit to the service organization and procedures performed there**;
  - another auditor.
  
  **A SOC report is therefore one route, not a legal requirement**
  `[authoritative]`.
- **Testing controls at the service organization (402.16).** If the user
  auditor expects those controls to operate effectively, evidence comes from a
  **Type 2 report**, its own tests at the service organization, or another
  auditor. A Type 1 report gives no evidence of operating effectiveness.
- **Using a Type 2 report (402.17, .A34–.A35).** The user auditor evaluates:
  - whether the period suits its purposes;
  - CUECs at the user entity;
  - the time elapsed since testing;
  - the relevance of the tests.
  
  Little overlap with the user entity's fiscal year reduces the evidence and
  may require another report or direct testing. Evidence about changes
  **outside** the report period may be needed. This is the standards' version
  of the problem that bridge letters address.
- **Carve-out on the user side (402.18).** If a report carves out a relevant
  subservice organization, the user auditor applies AU-C 402 to that
  subservice organization too.
- **SOC 2 does not substitute for SOC 1 (AU-C 9402, issued December 2022,
  revised March 2024).**
  - A SOC 1 report, or ISAE 3402 equivalent, is "the preferred report" for a
    user entity's audit.
  - A SOC 2 report "is not specifically designed" for ICFR and "therefore is
    unlikely to achieve the intent" of AU-C 402.
  - Reasons given:
    - SOC 2 change-management testing addresses security and availability,
      not completeness and accuracy of transaction processing (citing CC8.1);
    - SOC 2 descriptions may omit ICFR-relevant services and reports;
    - SOC 2 CUECs differ from SOC 1 CUECs.

  `[authoritative]`. **This is the strongest primary-source argument that a
  financial-processing platform cannot rely on SOC 2 alone if user auditors
  plan to rely on its controls.**

## 4. SOC 2: criteria structure

### 4.1 Trust services categories and criteria

The TSC (TSP section 100, 2017, with revised points of focus 2022) have five
categories: Security, Availability, Processing Integrity, Confidentiality
and Privacy (SRC-SOC-006, SRC-SOC-008). The criterion structure is
`[auditor-interpretation]`; the primary text was not read (OQ-100).

| Series | Content (own-words summary) | Count |
|---|---|---|
| CC1 Control environment | integrity and ethics; board oversight; structure and authority; competence; accountability | 5 |
| CC2 Information and communication | quality information; internal communication; external communication | 3 |
| CC3 Risk assessment | objectives; risk identification; fraud risk; change identification | 4 |
| CC4 Monitoring | ongoing and separate evaluations; deficiency communication | 2 |
| CC5 Control activities | selection of controls; technology general controls; policies and procedures | 3 |
| CC6 Logical and physical access | logical access architecture; registration; role-based access and least privilege; physical access; disposal; boundary protection; transmission and movement; malware | 8 |
| CC7 System operations | vulnerability and configuration detection; anomaly monitoring; event evaluation; incident response; recovery | 5 |
| CC8 Change management | change lifecycle | 1 |
| CC9 Risk mitigation | business disruption; vendors and business partners | 2 |
| A1 Availability | capacity; environmental protection and recovery infrastructure; recovery testing | 3 |
| C1 Confidentiality | identify and protect; dispose | 2 |
| PI1 Processing integrity | processing specifications; inputs; processing; outputs; storage | 5 |
| P1–P8 Privacy | notice; choice and consent; collection; use, retention and disposal; access; disclosure and notification; quality; monitoring and enforcement | 18 |

- **Common criteria.** CC1–CC5 follow the 17 COSO 2013 principles. CC6–CC9
  are additional security-specific criteria. The "common criteria" apply to
  every category, and **Security (the common criteria) is always in scope** in
  a SOC 2 `[auditor-interpretation]`. This is consistent with the Unbound
  directive (SRC-SOC-028).
- **Points of focus are not requirements.** They are characteristics to
  consider, and not every one must be addressed. The 2022 revision changed
  points of focus and implementation guidance only, not the criteria
  (SRC-SOC-006 via secondary summaries) `[auditor-interpretation]`.

### 4.2 Proposed 2027 revision

The AICPA proposed a 2027 revision on 28 October 2026 (SRC-SOC-009)
`[authoritative]` for the facts below.

- **Scope of the proposal.** The criteria would be restructured: duplicative
  concepts consolidated, some criteria removed, others added, and wording
  revised.
- **New AI-related points of focus:**
  - AI governance;
  - model monitoring;
  - prompt manipulation;
  - data poisoning;
  - transparency of AI-generated outputs;
  - training data;
  - automated decision-making.
- **Dates.** Comments are due 15 January 2027. If approved, the criteria
  would be effective for engagements beginning on or after **31 December
  2027**, with early adoption permitted.
- **Description criteria.** A companion description-criteria exposure draft
  was issued on 17 November 2026 (SRC-SOC-010). It keeps nine areas, stresses
  **materiality** to users, and adds implementation guidance on cloud,
  AI/ML, APIs and modern architectures. It cautions against detail that would
  expose vulnerabilities.
- **Unverified detail.** Secondary sources say the common criteria drop from
  33 to 20 and the COSO-based structure goes. This is unverified; the
  exposure drafts are login-gated (`open-question`, OQ-107).

**Implication.** Any Unbound SOC 2 examination would likely start after the
2027 criteria take effect, or during the early-adoption window. Control
design should therefore map to **both** the 2017 and the proposed 2027
structures. Final mapping should wait until the criteria are issued
`[recommendation]`.

### 4.3 SOC 2 description criteria (DC 200)

DC 200 (2018, with revised implementation guidance 2022) requires a
description of the following nine areas (SRC-SOC-007, SRC-SOC-010)
`[auditor-interpretation]` (not read):

1. types of services;
2. principal service commitments and system requirements;
3. system components (infrastructure, software, people, procedures and
   data);
4. significant system incidents;
5. the applicable criteria and related controls;
6. CUECs;
7. CSOCs and subservice organizations;
8. criteria that are not relevant, and why;
9. significant changes during the period.

The 2026 exposure draft says it "keeps nine existing areas" (SRC-SOC-010),
which corroborates the count.

## 5. Inclusive vs carve-out; CUECs and CSOCs

The definitions come from AT-C 320.08 (SRC-SOC-003) `[authoritative]`.

- **Carve-out method.** The description identifies the nature of the
  subservice organization's services and **excludes** its control objectives
  and controls from the description and from the scope. **CSOCs** are the
  controls that management assumes the subservice organization performs.
- **Inclusive method.** The description includes the subservice
  organization's objectives and controls. The auditor applies the standards to
  them, and **subservice management must give the assertion and written
  representations** (.11, .37).
- **CUECs** are controls that management assumes user entities will
  implement and that are necessary to achieve the objectives.
- **SOC for Cybersecurity** has no carve-out option (§1).

**In practice:**

- Carve-out is the norm for hyperscale cloud providers, which do not sign
  other organizations' assertions `[industry-convention]`.
- Inclusive treatment needs the subservice organization's cooperation and
  representations, which is rarely practical except for affiliates
  `[auditor-interpretation]`, derived from .11/.37.
- A carved-out subservice organization still shifts work to the user auditor
  (AU-C 402.18). So the service organization should monitor its subservice
  organizations' own SOC reports, under CC9.2 and the SOC 1 monitoring
  objective `[auditor-interpretation]`.

## 6. System boundaries and materiality

The system boundary is management's choice, but it is constrained:

- **SOC 1.** The scope must not be so limited that the report is useless
  (AT-C 320.10a). The description must not omit or distort information
  (.15c). The report states that the description covers only aspects likely
  to be relevant to ICFR (.40d) `[authoritative]`.
- **SOC 2.** The boundary follows the service commitments and system
  requirements (DC 200) `[auditor-interpretation]`. The 2026 description
  criteria exposure draft stresses materiality to intended users
  (SRC-SOC-010) `[authoritative]` for the fact of the proposal.
- **Materiality.** It is judged on the description, design and operation
  (AT-C 320.19), not on dollar amounts at any single user entity (.A46)
  `[authoritative]`.

## 7. Evidence, IPE, automation and continuous monitoring

**What the standards require of evidence:**

- **Information produced by the entity (IPE).** When using IPE, the
  practitioner must evaluate whether it is reliable enough. That means
  obtaining evidence of its **accuracy and completeness** and judging whether
  it is precise and detailed enough (AT-C 205.36; AT-C 320.30)
  `[authoritative]`.
  - Examples in AT-C 320.A52: population lists for sampling, exception
    reports, reconciliations, user access lists and **system-generated
    reports**.
  - Ways to get that evidence (AT-C 205.A38): as part of the procedure
    itself, or by **testing controls over how the information is prepared
    and maintained** `[authoritative]`.
- **Automated controls** can be evidenced by testing design and
  implementation once IT general controls are effective (AT-C 320.A47)
  `[authoritative]`.
- **Inquiry is never enough on its own** (AT-C 320.31a) `[authoritative]`.

**What this means for automated and continuous evidence
`[auditor-interpretation]`:**

- Management's monitoring tools, compliance platforms and continuous evidence
  are **not prohibited** by the standards.
- They are treated as IPE. The service auditor must still be satisfied about
  their completeness and accuracy, and **must perform and evaluate the tests
  itself**.
- A tool's dashboard is management's evidence, not the auditor's conclusion.
- Automated, tamper-evident, complete-population evidence can lower the cost
  of IPE testing. It cannot replace the auditor's independent procedures.

**Contradicting signals, recorded as required:**

- The AICPA is investigating allegations about a SOC compliance vendor
  (SRC-SOC-008) `[authoritative]` for the fact of the notice.
- A February 2026 JofA article warns that "a rushed or overly automated
  report may rely too heavily on inquiry" (SRC-SOC-013)
  `[auditor-interpretation]`.
- A May 2026 Peer Review Board alert directs reviewers to look at a firm's
  use of SOC platforms, at **identical** risk assessments, sample sizes and
  procedures across clients, and at whether timelines are reasonable
  (SRC-SOC-014) `[auditor-interpretation]`.

**Implication.** Platform-generated evidence is acceptable **as input**.
Platform-driven *audits* are under regulatory scrutiny. Echelon should design
evidence for **auditor re-performance and independent population
extraction**, not for one-click report generation `[recommendation]`.

Whether a specific service auditor will accept Echelon/Folio evidence
packages, and on what IPE testing basis, is pending (OQ-108; internal design
approved: source-authenticated reproducible evidence with completeness and
accuracy checks, DF-ASR-2026-0D11).

## 8. Sampling

**What the standard requires (AT-C 205.32).** If sampling is used, the
practitioner must:

- size the sample to reduce sampling risk to an acceptably low level;
- select items representatively;
- treat items that cannot be tested as deviations;
- investigate the nature and cause of deviations;
- evaluate the results, including sampling risk;
- conclude whether sampling gave an appropriate basis.

The AICPA Audit Guide *Audit Sampling* "may be useful" (205.A32). These are
`[authoritative]`.

**What the standards do not set.** No AICPA standard read in this batch sets
sample-size tables for tests of controls.

**Firm practice.** Firms publish tables keyed to control frequency (annual,
quarterly, monthly, weekly, daily, many-times-daily), often with an "expected
zero deviations" assumption. These tables are **firm methodology**
`[industry-convention]`. Specific figures were not verified in this batch and
are deliberately not reproduced.

**Implications `[recommendation]`:**

- Automated controls with effective IT general controls can be tested with
  very small samples, or a single instance (AT-C 320.A47).
- Complete, machine-verifiable populations make sampling cheaper.
- Neither removes the auditor's sampling judgment.

## 9. Observation periods and bridge letters

**Requirement versus convention:**

- **Requirement.** AT-C 320 sets **no minimum period** `[authoritative]`. It
  says only that a Type 2 is most useful when it covers a **substantial
  portion** of the user entity's financial statement period (.A49). The user
  auditor evaluates the overlap with its fiscal year and the time elapsed
  since testing (AU-C 402.17, .A34).
- **Convention.** Practitioner sources say no minimum exists for SOC 2 and
  that periods of **3–12 months** are usual. First reports are often three
  months, followed by annual twelve-month reports (SRC-SOC-022)
  `[industry-convention]`.
- **Contested figure.** Claims of an "AICPA six-month minimum" for SOC 2 could
  not be sourced and appear to conflate SOC 1 practice with SOC 2. This is
  recorded as a contradiction `[open-question]`, low priority.
- **Customer expectation.** User auditors of calendar-year sponsors will
  prefer periods that overlap substantially with the sponsor's fiscal year,
  and often a 12-month, rolling, contiguous coverage
  `[customer-expectation]` (inferred from AU-C 402.A34; OQ-110).

**Bridge (gap) letters:**

- They are **not defined in AT-C 105, 205 or 320 or in AU-C 402**. A text
  search of both codifications found no "bridge" or "gap letter"
  `[authoritative]`, an absence verified in the read text.
- In practice they are **management-signed statements** that no significant
  changes have occurred since the report period end. They are not
  attestation and carry no auditor assurance (SRC-SOC-027)
  `[industry-convention]`.
- The standards' own mechanism is the user auditor's evaluation of changes
  outside the period (AU-C 402.A35).

## 10. Exceptions and opinions

- **Exceptions.** Deviations are described in the Type 2 test results. Where
  identified, describing causative factors helps readers (AT-C 320.A63)
  `[authoritative]`. A deviation does not automatically modify the opinion:
  the auditor judges whether it falls within the expected rate (.32)
  `[authoritative]`.
- **Modified opinions.**
  - *Qualified* or *adverse*: when a description, design or operating failure
    is material.
  - *Disclaimer*: when sufficient appropriate evidence cannot be obtained,
    with known findings still identified (.42–.44) `[authoritative]`.
  - The split between qualified and adverse follows the AT-C 205
    pervasiveness concepts `[auditor-interpretation]`; that section of AT-C
    205 was not extracted here.
- **Market reception.** A qualified SOC report is common in practice, and
  user entities evaluate the affected objectives. Bank third-party risk
  management (TPRM) may escalate it `[industry-convention]`.

## 11. Auditor independence, qualification and quality

- **Independence.** The practitioner **must be independent** unless law
  requires acceptance of the engagement (AT-C 105.28). The report must state
  the independence requirement (AT-C 320.40j) `[authoritative]`. The user
  auditor must be satisfied with the service auditor's competence and
  independence (AU-C 402.13a) `[authoritative]`.
- **Ethics changes.** PEEC adopted clarifying ethics revisions for SSAE
  engagements, effective for engagements beginning on or after **15 June
  2026**. They include the new term "period covered by the attest report"
  (SRC-SOC-016) `[auditor-interpretation]`; the Code text was not read.
- **Independence and readiness work.** A CPA firm that designs or operates
  the controls it later examines faces self-review threats under the AICPA
  Code `[auditor-interpretation]`; the Code was not read (OQ-109 adjacent).
  The Echelon assurance repository's own rule is that internal agents are
  never independent (AGENTS.md rule 4).
- **Licensing.** Under the 2014 Uniform Accountancy Act (UAA) revision,
  examinations, reviews and agreed-upon procedures under the SSAEs are
  **attest** services that only CPAs in a CPA firm may perform. Adoption is
  by state statute and varies (SRC-SOC-015) `[auditor-interpretation]`; the
  model act and state laws were not read. The AICPA refers unlicensed firms to
  state boards (SRC-SOC-008) `[authoritative]` for the fact of the notice.
- **Peer review.** It is a firm-level requirement, mainly through state
  boards and AICPA membership. Results are a selection signal
  (SRC-SOC-013, -014) `[auditor-interpretation]`.
- **Quality management.** SSAE 23 aligned the attestation standards with the
  quality management standards. It is effective for engagements beginning on
  or after 15 December 2025, as noted in the AT-C 105/205 text
  (SRC-SOC-001/002) `[authoritative]`.
- **Pending change.** The ASB proposed further attestation-standard changes in
  February 2026, effective no earlier than 15 June 2029 (SRC-SOC-017)
  `[auditor-interpretation]`.

## 12. Using internal audit and other parties

- **Internal audit (AT-C 205.40–.44).** The auditor may use the internal
  audit function's work, or internal auditors as direct assistance, only after
  evaluating their competence, objectivity and systematic approach (including
  quality management). The auditor must reperform some of that work. The
  auditor uses less of it when judgment or risk is higher. Direct assistance
  requires written acknowledgment from management and auditor supervision
  `[authoritative]`.
- **Reporting the use.** In SOC 1, use of internal audit work in tests is
  described in the test section. Direct assistance is not referenced
  (AT-C 320.A64–.A66) `[authoritative]`.
- **Specialists and other practitioners** have their own requirements (AT-C
  205.37–.39) `[authoritative]`.
- **Echelon relevance.** An "internal audit function" made of Echelon-controlled
  agents is **not** objective in the AT-C sense unless its organizational
  status supports objectivity. Whether a service auditor would use such work
  at all is pending (OQ-109; internal design approved as management
  monitoring, not independent internal audit, DF-ASR-2026-0D11).

## 13. Restricted use and distribution

- **SOC 1.** Restricted to service organization management, user entities and
  their auditors (AT-C 320.40n) `[authoritative]`.
- **SOC 2.** Restricted to knowledgeable specified parties (SRC-SOC-012)
  `[auditor-interpretation]`.
- **General use.** Only SOC 3 and SOC for Cybersecurity are general use.
- **Distribution practice.** SOC 1 and SOC 2 reports are typically shared
  under NDA through a trust portal `[industry-convention]`
  (vendor example SRC-SOC-025, `vendor-claim`).
- **Repository rule.** SOC reports are restricted-use and must never be
  committed to this repository (SECURITY-AND-CONFIDENTIALITY.md).

## 14. International equivalents

- **ISAE 3402** is the IAASB standard for assurance reports on controls at a
  service organization. It applies with **ISAE 3000 (Revised)**, defines
  Type 1 and Type 2 reports, and covers controls relevant to financial
  reporting (SRC-SOC-018, -019) `[auditor-interpretation]`; read through
  summaries only.
- AU-C 9402 names an ISAE 3402 report as equivalent to SOC 1 for user auditors
  (SRC-SOC-005) `[authoritative]`.
- ISAE 3000 is commonly used for SOC 2-like reports outside the United States
  `[industry-convention]`.
- US-only operations make ISAE relevant only if non-US sponsors or auditors
  appear `[recommendation]`.

## 15. Bank and regulator expectations

- **2023 Interagency TPRM guidance (88 FR 37920).** Banking organizations
  "may consider reviewing" SOC reports during due diligence when relevant and
  available. Contracts often entitle the bank to SOC reports and preserve
  audit rights (SRC-SOC-020) `[authoritative]` for the guidance text.
  - This is **supervisory guidance, not a rule requiring SOC reports**.
  - Demand from bank partners is therefore `customer-expectation`.
- **2026 proposed replacement.** The OCC, Fed, FDIC and NCUA proposed
  replacement TPRM guidance on 15 September 2026, with comments due 16
  November 2026 (extended to 16 December 2026, per the question register). It emphasises risk-based tailoring and says credible
  summaries, certifications or assessments "may be adequate" for due
  diligence depending on facts (SRC-SOC-021) `[authoritative]` for the
  proposal text. Final status is unknown (OQ-105).

## 16. Contradictions and cautions recorded

1. **"Six-month minimum" claims** conflict with the absence of any minimum in
   AT-C 320 and with practitioner sources (§9).
2. **"SSAE 18 recodified into SSAE 21"** (a secondary source) conflicts with
   the AT-C 320 text, whose source is still SSAE 18 as amended by SSAE 21/23.
3. **2027 TSC "33→20 criteria"** is secondary only (§4.2).
4. **Scale of the 2022 revisions.** Secureframe calls them minor; BDO calls
   them significant. Both agree the criteria themselves did not change.
5. **Misuse of "certification".** One DAF-platform vendor markets a "SOC 2
   Type I certification" (SRC-SOC-026, `vendor-claim`). SOC 2 is an
   attestation report, not a certification. The Unbound directive already
   forbids this wording.
6. **Wording of the 2022 description criteria change.** BDO says "significant
   updates"; EY says the criteria were unchanged and only the implementation
   guidance changed. Both are consistent with criteria text being unchanged.

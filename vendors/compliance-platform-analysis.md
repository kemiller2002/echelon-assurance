---
title: Commercial and open-source compliance platforms - analysis and build/buy boundary
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
research_batch: B4 (agent key PTV)
sources: sources/fragments/PTV.csv
---

# Compliance platforms: analysis and the build/buy boundary

Evidence-class note: almost everything about functionality is
**[vendor-claim]**, taken from vendor pricing and product pages read on
2026-11-25. Prices come from Vendr buyer data seen only through search
summaries, because Vendr returned 403 to the agent. Complaint patterns come
from review aggregators seen through search summaries. None of these platforms
was trialled.

## 1. What these platforms actually do

These platforms share a common core **[vendor-claim, consistent across
SRC-PTV-038/-039/-040/-048]**:

1. **Framework/control library.** Prebuilt SOC 2, ISO 27001 and HIPAA
   controls, cross-mapped.
2. **Integrations.** API connectors using read-only credentials to:
   - cloud (an AWS cross-account role with the `SecurityAudit` policy plus a
     vendor custom policy, in Vanta's case; SRC-PTV-049)
   - identity provider
   - HRIS
   - code hosting (GitHub)
   - ticketing
   - MDM

   They pull configuration *state*.
3. **Automated tests.** Checks over that state, such as MFA enabled, storage
   encrypted, branch protection on, every employee has completed training.
   Failures appear as dashboard alerts.
4. **Endpoint agent.** A lightweight desktop agent, or an MDM integration,
   that checks disk encryption, screen lock, OS updates and antivirus.
5. **Policy templates and acknowledgement tracking.**
6. **Personnel workflows.** Onboarding and offboarding checklists, training,
   background-check evidence.
7. **Vendor risk** registers and questionnaires.
8. **Auditor collaboration.** An auditor portal or API (Vanta "Auditor API",
   Drata "Audit Hub"), plus auditor marketplaces or networks (Vanta auditor
   network, Drata Audit Alliance, Secureframe Audit Partner Network) and
   negotiated audit pricing.
9. **Trust center and questionnaire automation**, increasingly AI-assisted.

## 2. Comparison table

| Platform | Positioning | Evidence mechanism | Integrations (claimed) | Auditor model | Pricing evidence (US$/yr) | Notable concerns |
|---|---|---|---|---|---|---|
| Vanta (SRC-PTV-038, -029, -049) | SMB to mid-market automation | API connectors; Vanta Agent; AI agent | "400+" | Auditor API; auditor network (third-party CPA firms) | Vendr median ~20,000; observed ~7.5k-57k **[vendor-claim/summary-only]** | Broad read access; reported false positives and integration gaps (SRC-PTV-050) |
| Drata (SRC-PTV-039, -030) | SMB to mid-market automation | API connectors; agent (help-center docs); MDM integrations | Not stated on pricing page | Audit Hub; Audit Alliance | Vendr median ~25,000; observed ~9.4k-68k | Custom tests only from the Advanced tier; integration complaints (SRC-PTV-050) |
| Secureframe (SRC-PTV-040, -031) | SMB to mid-market; CMMC "Defense" tier | API connectors; Secureframe Agent and device agent | "300+" | Audit Partner Network | Vendr median ~20,000 | Agent and partner network only in upper tiers per pricing page |
| Sprinto | SMB automation | Not verified | Not verified | Not verified | No public rate card (search summary) | Not researched in depth; pricing page truncated |
| Hyperproof (SRC-PTV-053) | Mid-market/enterprise GRC | "Hypersyncs" (connectors), not detailed | Not stated | Not stated | Vendr median ~44k reported by an aggregator citing Vendr (n=39) **[low confidence]** | GRC workflow more than automation |
| AuditBoard, now **Optro** (SRC-PTV-051, -032) | Enterprise audit, SOX and GRC; Hg acquired for >$3bn in 2024; renamed Optro 2026-03-09 | Workflow-centric; connectors | n/a | Internal-audit oriented | Vendr median ~45,900; range ~21k-111k | Overkill for Unbound |
| Thoropass, formerly Laika (SRC-PTV-041) | Platform **plus affiliated audit firm** (Laika Compliance LLC dba Thoropass Assurance) | Connectors | n/a | **Bundled audit** by an affiliated licensed CPA firm; AICPA peer review "pass" (2025) per vendor | No credible public figure found | Independence optics (section 4) |
| Scrut | SMB automation | Not verified | Not verified | Not verified | Not found | Not researched in depth |
| Anecdotes (SRC-PTV-048) | Enterprise "GRC data engine" | Plugins collecting timestamped, source-attributed raw data | "230+" | Audit partners | Not found | Enterprise pricing likely |
| Conveyor / SecurityPal type | Questionnaire and trust-center automation only | Document/knowledge base | n/a | n/a | Not researched | Solves only the questionnaire burden |
| RegScale (SRC-PTV-047) | OSCAL-native compliance as code; FedRAMP/CMMC | Connectors plus OSCAL artifacts | "60+ frameworks" | Federal assessors | Not found | Federal focus; useful model for OSCAL export |
| Comp AI (SRC-PTV-043) | Open source (AGPL-3.0) plus proprietary cloud | Integrations catalog; self-host needs Postgres/Redis/etc. | Not listed | None | Community free | AGPL; young project |
| Probo (SRC-PTV-044) | Open source (MIT) GRC | Evidence as files/URLs; CLI | Few | Audit programs | Free | Manual evidence; young project |
| CISO Assistant (SRC-PTV-045) | Open source (AGPL-3.0) plus commercial editions | Manual/API evidence; 200+ frameworks with mapping; REST API, MCP | API-first | Audit module | Community free | Mapping library is the main value; not an automated collector |
| eramba (SRC-PTV-046) | Self-hosted GRC; Community and Enterprise | Manual workflows | Few | Audit workflows | Enterprise reported EUR 2,500/yr flat (EUR 5,000 also shown) | GRC toolkit, not an evidence collector |
| Delve (SRC-PTV-042) | AI compliance startup | n/a | n/a | Alleged rubber-stamp affiliated auditors | n/a | **Allegations (Mar 2026)** of fabricated evidence and pre-generated auditor conclusions; denied; unproven |

## 3. Security implications of buying

Buying one of these platforms carries several security costs:

- **A concentrated read path.** A typical deployment grants the vendor
  read-only access to:
  - the cloud control plane (SecurityAudit can read configuration across
    nearly all services)
  - the identity provider directory
  - HR data (names, start and end dates, sometimes background-check status)
  - GitHub organization metadata
  - endpoint posture

  A compromise of the vendor exposes the whole map of the environment.
  **[recommendation]** based on SRC-PTV-049. The Delve reporting includes
  claims of exposed customer data and an attack-surface weakness at the
  vendor (SRC-PTV-042, unproven).
- **The vendor becomes a subservice organization** that must be in Unbound's
  vendor-risk program (its SOC 2 report, access scoping, offboarding).
- **Agents installed on endpoints** add software supply-chain exposure.
- **Mitigations if bought:**
  - least-privilege custom roles instead of broad managed policies where the
    vendor supports it
  - separate read-only identities per integration
  - logging of vendor access (e.g., CloudTrail on the role)
  - no write scopes
  - contractual deletion on exit

## 4. Bundled auditors and independence

- **Two layers.** Platforms that sell or broker audits fall into two groups.
  Some broker independent CPA firms (marketplace or network). Others own an
  affiliated audit entity (Thoropass). Thoropass states that it separates
  personnel and process and that its peer review passed (SRC-PTV-041)
  **[vendor-claim]**. Industry commentary flags "bundled-auditor independence
  optics" for enterprise sales **[industry-convention, low confidence]**.
- **Delve case (2026)** **[news; allegations unproven]**.
  - An anonymous whistleblower alleged that a compliance-automation vendor:
    - supplied fabricated evidence
    - generated auditor conclusions before independent review
    - routed nearly all clients to two related audit firms
  - The company denies this and says reports are issued only by independent
    licensed auditors (SRC-PTV-042).
  - Whatever the outcome, the case shows the failure mode: when the platform
    selects, templates and pre-populates evidence, *and* the auditor depends
    on the platform for deal flow, the attestation can become a rubber stamp.
- **Recommendation for Echelon** **[recommendation]**:
  - Select the CPA firm independently of any software vendor.
  - Give the auditor direct read access to source evidence, not only
    vendor-rendered dashboards.
  - Confirm acceptability with the first bank or sponsor partner (OQ-310).

## 5. Limits of automated evidence

These limits hold for any platform, and for anything Echelon builds:

| Limitation | Why it matters to a SOC 2 Type 2 auditor | Echelon design response |
|---|---|---|
| Point-in-time configuration snapshots | Type 2 tests operation over a period. Daily snapshots show state but not every change in between | Event-sourced evidence: record every configuration change, e.g., audit log and IaC commit history, not only daily state |
| Population vs. sample | Auditors sample from a *complete* population (all changes, all terminations). Platforms show pass/fail per item but often do not prove completeness of the population | Generate complete populations from the system of record with counts and hashes; reconcile against independent counts |
| Configuration ≠ operating effectiveness | "Branch protection enabled" does not prove that reviews were meaningful or that approvers were authorized | Combine configuration evidence with per-transaction evidence (per-PR review records, approver authorization) |
| False positives and negatives | Reported by users (SRC-PTV-050); manual justification is needed | Tests as code with documented logic and test fixtures |
| Non-integrated systems | Custom apps (such as Unbound itself), F# services and financial processes have no connectors | Unbound must emit its own evidence for financial and tenant controls (Unbound CCM requirements) |
| Templates ≠ implemented controls | Template policies can describe controls that do not exist (the core of the Delve allegations) | Policies drafted from the control catalog; status vocabulary forbids claims without evidence |

**Manual work that remains with any platform** **[industry-convention]**:

- risk assessment
- scoping and the system description
- policy tailoring and approval
- vendor reviews
- access-review decisions
- incident tabletop exercises
- BC/DR tests
- the pen test itself
- remediation
- exceptions
- management assertion
- auditor walkthroughs and sample requests (PBC lists)
- evidence for anything without a connector

A platform automates perhaps the collection half of the work. The judgment
half remains. (No credible quantitative source was found; this is an
estimate.)

## 6. Capabilities Echelon must have to not need a platform

The minimal capability set is listed below. Each item says which existing
asset could provide it (systems are assessed separately in B1, so maturity is
not claimed here) and the target automation level.

| # | Capability | Minimum viable form | Candidate existing asset | Target level |
|---|---|---|---|---|
| 1 | Control register with framework crosswalk | CSV registers with validator | This repository (`controls/`, `regulatory/`); CISO Assistant's open mapping library as reference data (AGPL; reference only) | L0-L1 |
| 2 | Cloud configuration evidence collector | Scheduled job with a read-only role that exports configuration and change logs, hashed and stored | New small collector, or open-source cloud posture scanners; the IaC repository as the primary source | L2-L3 |
| 3 | Identity evidence | Export of users, MFA status and admin roles from the IdP (Fides/GitHub) and cloud IAM, with periodic diff | Fides (assessed in B1) | L2-L3 |
| 4 | Change-management evidence | Per-PR review, check runs and merge receipts | Praxis/Dokimos/Specula (B1 assesses) | L3-L4 |
| 5 | Personnel lifecycle and training | Roster with join/leave dates, acknowledgements, training completions, background-check attestation | A spreadsheet or register is enough at Echelon's headcount | L1 |
| 6 | Endpoint posture | MDM report export (encryption, screen lock, OS updates) | Whatever MDM is used; no platform agent needed if MDM exists | L2 |
| 7 | Vulnerability and pen-test evidence | Scanner outputs, pen-test summary and retest status in a restricted store | CI artifacts plus a restricted tracker | L2-L3 |
| 8 | Evidence store with integrity | Append-only store, content hashes, timestamps, retention, access control; auditor read access | Evidence architecture (B5/B7) | L2 |
| 9 | Population generation for sampling | Query or export producing complete, counted populations per control and period | Unbound emits; collectors export | L2-L3 |
| 10 | Control-failure alerting | Tests as code with alerts to the task system | Tutela/Vigila candidates (B1) | L3 |
| 11 | Policy lifecycle | Versioned Markdown policies, approval record, acknowledgement log | This repository (`policies/`) | L1 |
| 12 | Vendor register | CSV register with review dates and report collection | `registers/` | L1 |
| 13 | Auditor interface | PBC-request-to-evidence mapping and an export package per request | Scripted export from items 8-9 | L2 |
| 14 | Trust center / questionnaires | Static page (artifact) plus a reusable answer bank | Static page | L0-L1 |

Estimated difficulty **[assumption]**:

- Items 1, 5, 11, 12 and 14 are documents and registers and are cheap.
- Items 2-4, 6-7 and 9 are collectors, each a small scheduled script.
- Items 8, 10 and 13 are the genuine engineering, and they overlap with the
  evidence architecture Echelon intends to build anyway for Unbound's own
  transaction evidence. Building them is not mainly a compliance-platform
  cost; it is needed regardless because platforms do not cover Unbound's
  financial controls.

## 7. When buying is cheaper

Buy (Vanta, Drata or Secureframe class, at about US$20-25k/yr median
**[vendor-claim]**) if any of the following hold **[recommendation]**:

1. A customer or bank deal requires a SOC 2 report sooner than Echelon can
   build items 2-4 and 8-13, and engineering time is the binding constraint.
2. Multiple frameworks (SOC 2 plus ISO 27001 plus HIPAA or others) are needed
   at once. Cross-mapping and auditor familiarity then pay off.
3. Headcount grows enough that personnel and endpoint workflows dominate.
   Agents, HRIS and MDM integrations save real recurring effort.
4. The chosen auditor does not accept self-built evidence packages (OQ-312)
   and discounts platform-integrated engagements.
5. Fully loaded engineering cost to build and maintain items 2-4, 8-10 and 13
   is above roughly US$20-25k/yr. This is likely if they are built only for
   compliance and not reused by Unbound's evidence architecture
   **[assumption]**.

Do not buy if all of the following hold **[recommendation]**:

- SOC 2 is the only framework.
- Headcount is small.
- The evidence architecture is being built for Unbound anyway.
- The auditor accepts direct evidence (OQ-312).

Under those conditions a platform adds a broad read-access path, lock-in
(control mappings, evidence history and policies in a proprietary format,
with export quality unverified) and an annual fee, without covering the
financial and tenant controls that matter most.

**Middle path** **[recommendation]**:

- Use open-source CISO Assistant or Probo only for the framework library and
  register UI if spreadsheets prove insufficient. Self-host, read-only.
- Keep the collectors and evidence store Echelon-owned.
- Revisit at the first bank or sponsor contract (OQ-303, OQ-310).

## 8. Lock-in notes

- Lock-in risk is in the evidence history needed for Type 2 continuity, in
  custom tests, and in policy text written inside the platform. Export
  formats were not verified for any vendor **[open-question]**.
- Vendr-reported negotiated savings (about 23-30%) suggest list prices are
  softer at renewal than at first purchase. This is from search summaries
  only **[vendor-claim]**.

# Counsel, auditor, CPA, bank and insurer question packages

> **Prepared questions only (owner direction DF-ASR-2026-0D0F, ODP-12).** No
> legal or professional engagement exists or is authorized. These packages are
> held ready; sending any of them requires an owner reassessment recorded under
> 0D0F (legally required, commercially necessary, or material uncertainty that
> internal research cannot resolve). Until then each question is worked by
> primary-source research and multiple-agent review, and the affected capability
> stays gated from production activation.

Prepared 2026-11-25 from `registers/open-questions.csv` and the dispositions in
`question-dispositions.csv`. No paid engagement is authorized by this document. Everything here is **proposed
interpretation**, not a legal conclusion and not a statement of auditor acceptance.
Many questions below now have owner-approved internal design resolutions
(DF-ASR-2026-0D11); that record does not resolve their legal classification or
external acceptance, which these packages still ask about.

Common facts for every package:

- Unbound is a donor-advised-fund (DAF) administration platform. Under owner decision 1 (DF-ASR-2026-0D01), Echelon runs it as a commercial technology and servicing platform for sponsoring charities and financial institutions. Donors and advisors use it only through those organizations. Echelon does not act as sponsor and does not hold custody of assets.
- In the initial design, funds stay in bank accounts owned by the sponsor, and every flow is authorized by the sponsor. The design avoids platform custody and money transmission (REC item 17; the legal conclusion is open, OQ-216), and no direct Echelon–bank contract is assumed in the initial design; the approved internal design supports direct bank relationships behind activation gates, and the actual contracting structure is pending (OQ-205; DF-ASR-2026-0D11), and decision 1 includes financial-institution customers (F12, CQ-5). Card payments are deferred.
- Operations are US-first (owner direction ODP-10); the proposed design does not deliberately target the EU/UK (OQ-208).
- Controls are designed for SOC 1 type 2 and SOC 2 type 2 across all five trust categories (decision 2). GLBA Safeguards Rule elements are the design baseline, adopted without claiming they legally apply (decision 3).
- AI agents act within delegated tiers; in financial domains AI agents are capped at A2; they never prepare, alter or release payment instructions and never authorize, approve or release money movement in any scope (BR-AI-001, BR-FIN-006, DF-ASR-2026-C004); financial execution is deterministic under established human authority, which for money movement is the sponsor's: a deterministic worker acts only under a recorded sponsor (board) delegation or sponsor-user authorization, or a recurring authority set by the sponsor and revalidated at execution (BR-FIN-006, BR-FIN-015, DF-ASR-2026-C004); and enforced technical gates apply (decision 4; ODP-10).

Preconditions: the owner has decided US-first operations, the card deferral and
the sponsor-controlled-funds constraint (ODP-05, ODP-10). Remaining: the actual
contracting entity (OQ-102; for planning the operator is the Echelon-side
service organization, DF-ASR-2026-0D11), and an owner reassessment under 0D0F before any
package is sent.

---

## 1. Privacy and financial-services counsel (one scoped memo)

Blocking tier for all items: **production-launch**. Requirements are not blocked
because decision 3 already adopts the Safeguards elements. Bundle all items into a
single question set. As a no-cost first step, ask prospective sponsors what position
their own advisers have taken.

**C1. GLBA status of DAF sponsors and donors (OQ-200, OQ-203).**
Question: Is a 501(c)(3) DAF sponsoring organization a "financial institution" under
15 U.S.C. 6809(3) / 16 CFR 314.2(h)? Is a donor a "consumer" obtaining a financial
product or service for personal, family or household purposes? If so, does Regulation P
(12 CFR 1016) apply to donor information?
Facts: contributions are irrevocable and the sponsor owns the assets (26 U.S.C. 4966(d)(2)). The
sponsor invests the assets and makes grants on the donor's advice. We found no FTC or CFPB guidance,
enforcement action or case on DAF sponsors.
Proposed interpretation: an irrevocable charitable gift is not obtaining a consumer
financial product, so GLBA privacy obligations probably do not attach to the sponsor
by statute. We are uncertain because of the investment-advisory features. We
will design privacy notices so they can be enabled per sponsor.
Unblocks: whether the sponsor privacy-notice templates are mandatory (REQ-GLBA-014/015)
and whether REQ-GLBA rows are classified as legal or contractual.

**C2. FTC jurisdiction over nonprofits under GLBA (OQ-201).**
Question: Does 15 U.S.C. 6805(a)(7) give the FTC Safeguards Rule authority over a nonprofit
financial institution, even though 15 U.S.C. 44 defines "corporation" as an entity organized
for profit? Does any other agency have GLBA safeguards authority over a DAF sponsor?
Facts: we read 15 U.S.C. 44 on 2026-11-25 (SRC-OQ2-003). The district-court rulings in FTC v.
Grand Canyon Education were made at the pleading stage, and the case was later dismissed.
Proposed interpretation: the question is unsettled. Treat the Safeguards Rule as contractual
for sponsors, and as a design baseline in any case.
Unblocks: how the Safeguards obligation is labeled in sponsor contracts.

**C3. Is the operating entity itself a financial institution? (OQ-202, OQ-300)**
Question: Is the entity named under the preconditions a "financial institution" under
16 CFR 314.2(h)? Examples would be data processing under 12 CFR 225.28(b)(14), or acting as
an account servicer. If it is, (a) does 314.1(b) bring in customer information received from
sponsors, and (b) how are "fewer than five thousand consumers" counted for the 314.6
exemption? Specifically: are the counts aggregated across sponsors, and do donors count at all?
Facts: 314.4(d)(2) (read) lets an institution use effective continuous monitoring in place of
annual penetration testing plus vulnerability assessments every six months.
314.6 (read) exempts paragraphs (b)(1), (d)(2), (h) and (i) for institutions under the 5,000 threshold.
Proposed interpretation: a white-label technology provider that does not hold custody is
probably a service provider to financial institutions, not a financial institution itself.
Regardless of the answer, continuous internal monitoring and testing are designed (ODP-11; CTL-VUL-004, Designed, not yet operating); whether an independent penetration test is needed is reassessed against risk and actual obligations before material production launch (BR-VUL-012, PG-10).
Unblocks: whether the annual penetration test is a legal minimum or voluntary
(REQ-PENTEST-010..012), and legal vs contractual classification for Unbound.

**C4. State privacy laws and breach notification (OQ-209, OQ-210, OQ-104, OQ-503).**
Question: Please review our internally drafted per-state applicability table, covering sponsors
as controllers and Unbound as processor, including whether any nonprofit or GLBA exemption
applies. Also review our notification matrix, which covers statutory notice to data owners and
configurable contract windows. In addition: (a) Under the laws that apply, is destroying
per-subject encryption keys ("crypto-shredding") an acceptable form of deletion while the
ciphertext remains in WORM storage? (b) In which deployment models, if any, does Unbound act
toward donors in its own right rather than as a processor?
Approved design: configurable privacy roles, with sponsor instructions as the default and own-purpose data kept separate (OQ-104, DF-ASR-2026-0D11). Proposed interpretation of the legal role, still the question: Unbound acts as a processor in every planned model. Crypto-shredding is not
assumed to be sufficient on its own; personal data is kept separate from minimal evidence metadata.
Unblocks: processor contract terms, the incident-response matrix and the privacy design of the evidence store.

**C5. Retention schedule (OQ-502, OQ-614, OQ-762; tax portions with T1).**
Question: Please confirm or correct our draft minimum retention period for each record class:
financial records, control evidence, security logs, AI agent prompts and traces, and personal
data payloads. Please also give the precedence rule for conflicting holds across sponsors.
Proposed interpretation: retention is configurable per record class with legal holds. The
default is the longest applicable period found in primary sources, and there is no universal
period.
Unblocks: final compliance-mode retention periods. Until then a compliance-mode lock at a
conservative minimum period is applied from the first production evidence: retention can be
extended later but not shortened, so locking early carries no irreversibility risk
(BR-ASR-006; corrected in the consolidation challenge pass).

**C6. Sanctions allocation (OQ-213).**
Question: Is screening by Unbound on the sponsor's behalf, as specified in SPD-GRT-003, sufficient?
Does Unbound, as a US person that may "facilitate" transactions, need its own sanctions
compliance program under OFAC's 2019 Framework? Please provide contract allocation language.
Proposed interpretation: the sponsor is the grantor and is accountable. Unbound runs
risk-based screening and rescreens at release, records each disposition, and keeps a
lightweight program of its own. Screening does not guarantee that a transaction is lawful.
Unblocks: REQ-OFAC rows and contract clauses.

**C7. Vulnerability disclosure safe harbor (OQ-313).** Please review the draft VDP, adapted
from the CISA template, for authorization and safe-harbor wording. Blocks publishing the VDP.

**C8. Entity structure and conditional items (OQ-102, OQ-756, OQ-205, OQ-216, OQ-301).**
Question: With the named entity and sponsor-owned accounts, please confirm three points:
(a) sponsor and operator are legally separate, with no related-party issue; (b) whether
12 U.S.C. 1867(c) examination authority can reach Echelon for services performed for a bank
"by contract or otherwise", including indirectly through a sponsor's bank relationship (see
CQ-5), and which bank-service-provider incident-notification rule applies by the bank's
regulator (OCC 12 CFR 53.4, FDIC 12 CFR 304.24, Federal Reserve 12 CFR 225.303); (c) no
money-transmission licence (including limb (i)(B) of 31 CFR 1010.100(ff)(5)) or NYDFS Part 500
covered-entity status is triggered.
Proposed interpretation: the answer to each is no, provided funds stay in accounts the sponsor
controls. Any alternative funds flow comes back for internal research and owner reassessment before it is enabled.

## 2. Tax advisor (sponsor's or Unbound's)

**T1. Generated tax documents and retention (OQ-212).** Blocking tier: **implementation**.
Question: Please confirm our catalog of documents the platform generates for the sponsor:
contemporaneous written acknowledgments under 170(f)(8) and 170(f)(18), Schedule D Part I data
under 6033(k), Form 8282 triggers for noncash dispositions, and any 1099 obligations. Please
also state a concrete retention period for each under Treas. Reg. 1.6001-1(e).
Proposed interpretation: the sponsor issues the documents, and the platform generates them as
templates the sponsor approves. We propose 7 years after the year the record is filed, and
longer under a legal hold. That is a proposal, not advice. Internal research reads the primary
IRS text first.

## 3. Service auditor (candidate CPA firms): single readiness scoping agenda

Blocking tier: **external-examination**, except A1, which is **implementation**.
Cost: hold free pre-engagement scoping calls with 2-3 licensed, peer-reviewed firms. Bring
the synthetic evidence package, the system register, the authority matrix and a
GitHub dependency diagram.

**A1. Evidence own-collection vs compliance platform (OQ-312, OQ-108, OQ-765, OQ-500, OQ-510, OQ-601).**
Question: Will you accept evidence from our own collectors as information produced by the
entity (IPE)? The design is: source-authenticated populations with hashes, independent
population reconciliation, managed immutable storage separate from generation (decision 10),
and read-only access for the auditor. Which ITGCs over the collectors, reconciler and store would
you test? How would you test completeness and accuracy? How would you classify agent-produced
records: primary, derived or AI-interpreted?
Proposed interpretation: you test ITGCs over the evidence system plus independent
re-extraction samples. AI-interpreted material is never used as primary evidence.
Unblocks: auditor acceptance of evidence produced independently of commercial compliance platforms (OQ-312; design approved, DF-ASR-2026-0D11).

**A2. Segregation of duties with one human owner (OQ-701, OQ-763, OQ-702).**
Question: Our proposed arrangement is a standing human authorization, a separate implementing
agent, a technical verifier with its own key, and an enforced release gate for routine
low-risk changes. Material policy, privilege, financial-exception and verifier changes need
elevated human approval. No external reviewer is contracted at the development stage (owner direction DF-ASR-2026-0D0F); an action that needs organizational independence is blocked or redesigned, and material exceptions are never self-approved. Agent identities
are authenticated and bound to platform identities. Is this acceptable for change-approval and
grant/payment SoD testing? If not, what is the minimum human arrangement?
Proposed interpretation: technical independence is not organizational independence, so we
expect at least sampled review by an independent human for elevated classes.

**A3. Scope (OQ-103, OQ-106, OQ-602, OQ-707, OQ-750).** Which TSC categories follow from our draft
service commitments? Should banks and processors, AI model providers and GitHub be carved out
or treated as inclusive, and which CSOCs would you expect? Does the proposed system boundary hold?
Note: today the repositories are on a personal account and default branches are unprotected.

**A4. SOC 1 objectives and IPE (OQ-116, OQ-758).** Please comment on the management-drafted
control objectives for DAF administration and the IPE register for the top 10 SOC 1 controls.

**A5. Policy of record (OQ-501).** Is a signed policy digest, with a rendered version and an
approval record, acceptable as the approved policy? Or do you require a separately signed document?

**A6. Monitoring and validation (OQ-109, OQ-604, OQ-612).** We assume you will place no
internal-audit reliance on our monitoring. Would our mutation tests and synthetic transactions
inform the nature and extent of your procedures? Would you use, or perform yourselves, an
external technical validation of the evidence machinery?

**A7. Penetration testing (OQ-302, OQ-308, OQ-309).** Do you expect an independent penetration
test within or near the examination period? Would a grey-box application/API test of a
production-equivalent environment built from the same infrastructure-as-code be acceptable?
Is autonomous or AI testing only a vulnerability assessment?
Proposed interpretation: SOC 2 does not require a penetration test (SRC-PTV-015). Independent
testing is deferred under owner direction ODP-11 and reassessed against risk
and actual obligations before material production launch (BR-VUL-012, PG-10);
none is planned now.

**A8. Engagement structure and cost (OQ-111, OQ-755).** Please give an indicative fee for separate
and for combined SOC 1 and SOC 2 type 2 engagements over the same period on a shared control set.

**A9. 2027 criteria (OQ-107, OQ-113, OQ-605).** Final criteria are expected after the comment
period closes on 2027-01-15. The proposed effective date is engagements starting on or after
2027-12-31, with early adoption permitted. Would you recommend early adoption? How would AI
be disclosed without exposing vulnerabilities?

## 4. Sponsor's external auditor (via the first sponsor)

**U1. Planned approach (OQ-101, OQ-110, OQ-754).** Blocking tier: **external-examination**.
Question: Unbound would run the sponsor's DAF program ledger and export summarized journals
to the sponsor's ERP. Would you treat this processing as relevant to the sponsor's ICFR?
Would you plan to rely on a SOC 1 type 2 report, test controls directly, or audit
substantively? What period and end date would you need, given the sponsor's fiscal year?
Attach a one-page service description and the convergence crosswalk.
Proposed interpretation: the processing is relevant, a SOC 1 type 2 is expected, and the
first period is aligned to the sponsor's fiscal year end.

## 5. Bank partner / ODFI (one consolidated diligence request)

Blocking tier: **production-launch**, before onboarding a financial institution. Ask the bank's
TPRM team for its standard due-diligence request list, its model security addendum
(often shared under NDA) and its TPSP onboarding questionnaire. Then ask:

- B1 (OQ-204, OQ-105): Which assurance do you require: SOC 1 or SOC 2, which type, scope and period? What are your bridge-letter, right-to-audit, incident-notice, subcontractor-notice, examination-access and insurance terms? Note that the 2026 interagency TPRM proposal is not final; its comment period was extended to 2026-12-16.
- B2 (OQ-206, OQ-757): Please confirm in writing that Unbound would be a Third-Party Service Provider and not a Third-Party Sender. Which audit, risk-assessment, data-security and fraud-monitoring obligations apply to Unbound now (both fraud-monitoring phases are reported in force before 2026-11-25; we have read only Nacha summary pages)? Please share the relevant Nacha rule excerpts.
- B3 (OQ-217): Do you require self-assessment against a particular framework, such as CSF 2.0, the CRI Profile or CISA CPGs? SR 24-7 endorses none.
- B4 (OQ-303): What penetration-test terms do you require: frequency, tester accreditation, sharing of the report or a summary letter, and remediation SLAs?
- B5 (OQ-610): What do you require on a vendor's use of AI: disclosure, model risk management and human oversight?
- B6 (OQ-310, OQ-506, OQ-606): Will you accept a SOC report from a firm affiliated with a compliance platform? Do you need OSCAL output or ISO 42001?

Our proposed position: controls are designed for SOC 1 type 2 and SOC 2 type 2 across all five
trust services categories (decision 0D02); whether and when an examination happens
is not yet decided. Contract notice windows are configurable. We do not claim the bank
has accepted any of this.

## 6. Insurance broker / cyber insurer

**I1 (OQ-307, OQ-609).** Blocking tier: **production-launch**. At the first quote, ask: Do you
require, or give a discount for, an annual third-party penetration test, a VDP or tester
accreditation? Does coverage extend to losses from autonomous AI agent actions, such as
unauthorized actions or disclosure to AI providers? Are there AI-specific control conditions?
Attach the AI governance summary. Free; this is part of ordinary underwriting.

## 7. Not professional advice (handled through ordinary diligence; listed for completeness)

- Licensors (OQ-214): email CIS to request a Supporter license, which is available to companies with under USD 200k in revenue. Email the SCF Council for written confirmation of the intended use.
- Acquirer/processor (OQ-207): ask about SAQ type only if card payments are enabled.
- AI providers (OQ-603): request enterprise data-processing terms.
- Penetration-test firms (OQ-311): none now; the scoping package is kept current for the reassessment in WI-EXT-04 (DF-ASR-2026-0D0F, ODP-11). No quotes are requested.

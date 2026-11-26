# Funds-flow regulatory review: Unbound as operated by Echelon

Date: 2026-11-25. Status: internal primary-source research with multiple-agent review (owner direction DF-ASR-2026-0D0F, ODP-12; no counsel engaged). **Not a legal opinion.** No characterization below is a conclusion; each is our proposed working interpretation, and the open ones are prepared as questions in `findings/counsel-and-auditor-questions.md`.
Inputs: `unbound` @ 9b38de4 (requirements only, nothing built), `daf-landscape` (research), `echelon-assurance` FIN-BC-001..024, owner decision 1 (Echelon operates Unbound as a commercial technology and servicing platform for sponsors and financial institutions; white-label; no Echelon sponsorship or custody; legal structure unconfirmed).
Evidence classes: **A** authoritative (statute, regulation, regulator ruling or guidance, network-rule publisher), **AI** auditor-interpretation, **IC** industry-convention, **VC** vendor-claim, **AS** assumption, **OQ** open-question. Sources: `sources.csv` (SRC-FIN2-*), all retrieved 2026-11-25. Earlier daf-landscape sources (REG-0xx) are cited as recorded there, not re-verified unless listed.

## 1. What the repos actually say about money

Unbound is designed as an instruction-only platform. Its requirements put the sponsor in control of the money and keep Unbound out of the flow of funds:

- **CNV-LEG-001**: Unbound SHALL NOT receive or hold donor or grant money (no operator, FBO or pooled accounts). It SHALL NOT be able to execute a payment, and it SHALL NOT prevent one except through a sponsor-adopted protective hold. Donor payments settle directly into sponsor-owned accounts through regulated processors. A funds-flow conformance check blocks activation when these tests fail.
- **CNV-PAY-001 / UNB-XIN-BNK-001**: the sponsor is the Originator on sponsor-titled accounts under its own ODFI agreement. Unbound is a Third-Party Service Provider (TPSP), never a Third-Party Sender (TPS). A configuration naming Unbound as Originator is rejected.
- **The release-1 default rail** is a networked payment partner where the sponsor has an executed partner agreement. Otherwise grants go by sponsor-originated ACH, with check as a fallback.

Everything here is design. No bank, processor, partner or custodian agreement exists, and every activation gate is open. [observed]

| # | Flow | Account holder / custody | Who initiates | Unbound role (per requirements) | Release-1 status | Repo refs |
|---|---|---|---|---|---|---|
| F1 | Cash contribution by donor push (ACH credit, wire) | Sponsor-titled contribution or clearing account at sponsor's bank | Donor via own bank | Match, recognize (4 dates), availability, acknowledgment | In scope | CNV-FIN-003; RR-TVR-CON-001-r1; CNV-PAY-005 |
| F2 | Paper check contribution | Sponsor account; lockbox if sponsor contracts one | Donor mails; sponsor's bank deposits | Match remittance data; acknowledgment | In scope (check named in CNV-PAY-005) | SPW-020 (simplification inventory) |
| F3 | Processor-settled contribution (sponsor's existing processor; card later) | Processor, then sponsor contribution clearing account; sponsor is merchant of record | Donor via processor page | Record gross, fee, net; payout identity; acknowledgment after settlement | Processor in scope; card deferred (CNV-PAY-008) | UNB-XIN-PAY-006; CNV-PAY-006/008; XID-BNK-008 |
| F4 | Donor ACH debit (one-time or recurring) | Sponsor account | Sponsor as Originator, under donor authorization | Would prepare entries as TPSP | **Prohibited in R1** (CNV-PAY-005 AC-3) | CNV-PAY-005; daf-landscape recurring record CON-REC-05 |
| F5 | Securities and noncash contributions (DTC, certificates, complex assets) | Sponsor's custodian or external manager platform | Donor's broker delivers | Match deliveries (unmatched DTC queues), completed-gift rule, 8283/8282 records, UBTI refusal | Public securities in scope; UBTI assets refused | CNV-FIN-010, CNV-FIN-013; UNB-INV-012 |
| F6 | Investment of fund assets, liquidation to fund grants | Sponsor accounts at custodian; external RIA under sponsor agreement | Sponsor or its manager | Ingest positions and units, reconcile, relay donor allocation preferences among the manager's options | In scope; no sponsor-maintained pools | CNV-LEG-001; CNV-FIN-005; UNB-NAV-* |
| F7 | Grant by sponsor-originated ACH or wire | Sponsor disbursement account (debit block) | Sponsor under its ODFI agreement | Prepare instruction after approval, destination proof, sanctions fence; reconcile returns and NOCs | In scope | CNV-PAY-001/002/004/007; CNV-ARC-003 |
| F8 | Grant by check | Sponsor account with positive pay | Sponsor's bank prints/issues (or sponsor) | Issue file, void, stale-dating, unclaimed property records | Fallback rail | CNV-PAY-002/003; UNB-RCN-GRT-005 |
| F9 | Grant via networked payment partner (Chariot named first) | Partner holds funds in transit after debiting sponsor's disbursement account | Unbound sends instruction to partner API under sponsor's partner agreement | Orchestrate; exposure cap; status queries | **R1 default where agreement exists** | CNV-PAY-001; CNV-PAY-007; CNV-PLT-012 |
| F10 | Fund-level fees and Echelon's own platform fee | Fees: internal transfer fund to sponsor operating. Echelon fee: invoice to sponsor/partner (assumed) | Sponsor ledger posting; invoice | Compute, post, recompute | In scope | CNV-FIN-002/006; UNB-XIN-ERP-002 (PartnerInvoice) |
| F11 | Returns, chargebacks, voids, refunds, unclaimed property, OFAC blocked property | Sponsor accounts (blocked property in a sponsor blocked account) | Banks, processors, sponsor | Match returns, reverse availability, VoidDisposition, report data | In scope | CNV-PAY-003/005; CNV-GRT-006 |
| F12 | White-label for a financial institution (decision 1) | Bank or bank-affiliated sponsor accounts | Bank / affiliated sponsor | Technology and "servicing" for the bank | Supported as technical functionality and may be built and tested in non-production (ODP-05); production activation gated on CQ-5 and OQ-205 (BR-FIN-020, PG-02) | Decision 1; OQ-105, 204, 205, 211, 753 |

## 2. Regulatory considerations per flow

### 2.1 Federal money transmission (FinCEN), all flows
- **Definition [A, SRC-FIN2-001].** 31 CFR 1010.100(ff)(5)(i) has two limbs. Limb (B) covers "any other person engaged in the transfer of funds"; it is the limb most likely to reach a platform that originates or transmits payment instructions and files (F7, F9), and this review gives it no answer. It is open and is worked by internal primary-source research, with external advice only after an owner reassessment under DF-ASR-2026-0D0F (CQ-1, CQ-2). Limb (A) defines money transmission services as "the acceptance of currency, funds, or other value that substitutes for currency from one person and the transmission of ... to another location or person by any means". Status is "a matter of facts and circumstances" under (ff)(5)(ii). The listed exclusions cover:
  - (A) delivery, communication or network access services that support a money transmitter;
  - (B) payment processor: "to facilitate the purchase of, or payment of a bill for, a good or service through a clearance and settlement system by agreement with the creditor or seller";
  - (F) funds transfers "only integral to the sale of goods or the provision of services".
- **Payment-processor conditions [A, SRC-FIN2-005].** FIN-2014-R012 states four conditions for the (ii)(B) exclusion:
  - (a) the payment facilitates a purchase of goods or services, or the payment of bills for them;
  - (b) it runs through clearance and settlement systems that admit only BSA-regulated institutions;
  - (c) there is a formal agreement;
  - (d) that agreement is at least with the seller or creditor.
- **Proposed interpretation [AS].** Under F1-F8, Unbound never accepts value (CNV-LEG-001), so the definition's first element ("acceptance") is not met. The exclusions are therefore not needed. That matches daf-landscape Model A (REG-042/043, inference [I]). (ii)(A) excludes delivery, communication or network access services *supporting a money transmitter*; it may cover services to the ODFI or payment partner where those are themselves transmitters, but does not by itself establish that sending instructions is not transmission. Limb (i)(B) remains open.
- **Where the exclusions do not help.** A charitable grant is not the purchase of a good or service or the payment of a bill. So (ii)(B) and (ii)(F) fit poorly wherever Unbound or Echelon *would* be in the flow (daf-landscape §14, [I]).
- **Facts that change this:**
  - any Echelon- or Unbound-titled, FBO or pooled account;
  - Echelon credentials able to release funds without a sponsor step;
  - Echelon netting its fee from settlement;
  - "faster grant" float or advances funded by Echelon;
  - Echelon contracting with the payment partner as a party to the transfer.

### 2.2 State money transmission (MTMA and non-MTMA states), F3, F9, F12
- **Model definitions [A for model text only, SRC-FIN2-006].** The CSBS model defines money transmission to include "receiving money for transmission". Section 3.01(b) exempts an **agent of a payee** collecting payment "for goods or services ... provided to the payor by the payee", subject to three conditions:
  1. a written agreement with the payee;
  2. the payee holds the agent out to the public as accepting payments on its behalf;
  3. receipt by the agent extinguishes the payor's obligation.
- **Adoption [A, tracker, SRC-FIN2-007].** Adoption is partial and uneven. Examples: Colorado full MTMA effective 2025-07-17, Illinois 2026-01-01, Virginia 2026-07-01; several states adopted only some provisions. State statutes, not the model, govern.
- **Proposed interpretation [AS]:**
  - For F3, the processor's licensing (or its own exemption) is the processor's issue, as long as funds settle directly to the sponsor and Unbound never receives them.
  - The agent-of-payee exemption is drafted for goods or services. A donation is neither, and the payee in F9 is a charity, not the sponsor. So it should not be relied on for Echelon. Whether a payee-agent theory applies to contributions collected *for the sponsor* is a state-by-state question [OQ].
  - For F9, the networked partner holds funds in transit, so the partner is the party whose licensing or bank status matters. CNV-PLT-012 already requires segregated holding and an exposure cap.

### 2.3 Nacha roles (F4, F7, F9, F11)
- **Roles [A, network rules, SRC-FIN2-004].**
  - A TPSP "provides services related to ACH processing on behalf of another party; may or may not transmit entries directly".
  - A TPS is a TPSP acting for an Originator "without a direct agreement between the Originator and the ODFI".
  - The deciding fact is who holds the origination agreement with the ODFI.
- **Fraud monitoring [IC, SRC-FIN2-010: Nacha summary page; Rules text not read].** Rules effective 2026-03-20 (Phase 1) and 2026-06-22 practical date (Phase 2) require risk-based fraud monitoring by:
  - non-consumer Originators, TPSPs and TPSs;
  - ODFIs and RDFIs.

  They must be reviewed at least annually. CNV-PAY-001 already accepts this by contract.
- **Proposed interpretation [AS].** For F7, Unbound is a TPSP if the sponsor holds the ODFI agreement and Unbound only prepares or transmits files or API calls.
- **For F9 [OQ].** The partner is likely the TPS, or the Originator for its own credits. Unbound's role relative to the partner's ODFI must be confirmed from the partner contract.
- **What would make Echelon a TPS:**
  - Echelon signs an origination agreement with any ODFI on sponsors' behalf;
  - Echelon aggregates several sponsors' entries under its own bank relationship.

### 2.4 Bank third-party risk and Bank Service Company Act (F7, F9, F12)
- **Third-party relationships [A, guidance, SRC-FIN2-002].** The 2023 interagency guidance covers "any business arrangement between a banking organization and another entity, by contract or otherwise". It states that "a banking organization's use of third parties does not diminish its responsibility". It binds banks, not Echelon. Its effect reaches Echelon through bank contracts: due diligence, audit rights, incident notice and termination.
- **BSCA examination [A, SRC-FIN2-003].** Under 12 U.S.C. 1867(c)(1), where a regulated depository institution has authorized services performed for it "by contract or otherwise", that performance is "subject to regulation and examination by such agency to the same extent as if such services were being performed by the depository institution itself". Under (c)(2) the institution must notify its agency within 30 days.
- **Proposed interpretation [AS]:**
  - In F7 the bank's customer is the sponsor, not Echelon. Echelon is a fourth party to the bank, and its exposure is contractual through the sponsor's ODFI agreement (OQ-757).
  - In F12, a bank (or bank subsidiary) contracting directly with Echelon for DAF technology and servicing would make Echelon a candidate bank service provider. Its services could then be examined by the bank's regulator.
  - This contradicts the FIN-BC disposition "no assumed ... direct bank service relationship" (OQ-205) and must be escalated.
- **Facts that change this:**
  - which entity signs (bank, bank holding company affiliate, or a separate charity affiliated with the bank);
  - whether the services are of the kind 1867(c) reaches (data processing, recordkeeping);
  - whether the bank is NYDFS-covered (OQ-211).

### 2.5 Custody and investment (F5, F6)
- **Requirement position [observed].** Assets are held by the sponsor's custodian or external manager, and Unbound reconciles (CNV-LEG-001, UNB-INV-026). Unbound holds no custody, so there is no custody characterization for Echelon if this holds.
- **Facts that change this:**
  - Echelon holding custodian credentials able to move assets;
  - Echelon-titled omnibus or sub-accounts;
  - Echelon routing orders for the sponsor's accounts.

  CNV-LEG-001 forbids order routing, model construction and asset-based compensation. Adviser and broker questions are noted but out of scope here [OQ].
- **Omnibus custody.** Omnibus custody with fund-level attribution (UNB-INV-002) is a sponsor account structure, not Echelon custody. Reconciliation (custody to pool to units to funds to GL) remains a key SOC 1 control.

### 2.6 OFAC (F1, F3, F5, F7-F9, F11)
- **Program expectations [A, SRC-FIN2-009].** OFAC expects risk-based programs built on five components: management commitment, risk assessment, internal controls, testing and auditing, and training. daf-landscape records sanctions liability as strict for all U.S. persons and 10-year records (REG-036/037, [F], not re-verified here).
- **Proposed interpretation [AS].** The sponsor (as payor and holder of blocked property) and Echelon (as a U.S. person that could "facilitate") each have their own exposure. Unbound's design screens:
  - at onboarding;
  - on list change;
  - at release, with an atomic dispatch fence (CNV-GRT-006, CNV-ARC-003).

  Blocked property is held in a sponsor account, never by Echelon. The allocation of responsibility is OQ-213. Proposed FIN-BC-028 makes the release fence a financial-integrity invariant.

### 2.7 IRS substantiation (F1-F5, F11)
- **Statute [A, SRC-FIN2-008]:**
  - 170(f)(8) denies a deduction of $250 or more without a contemporaneous written acknowledgment stating the cash amount or a description (not value) of property, and whether goods or services were provided.
  - 170(f)(18) requires, for DAF contributions, a contemporaneous written acknowledgment that the sponsoring organization "has exclusive legal control over the assets contributed".
- **Proposed interpretation [AS].** The donee (sponsor) issues the acknowledgment. Unbound generates it on the sponsor's behalf through the gated template approved under the owner's recorded resolution (UNB-RCN-FIN-001). Echelon must never appear as the donee.
- **Where this links to the funds flow.** Exclusive legal control is undermined by any design where Echelon, a partner or a donor can move contributed assets without the sponsor. daf-landscape §14 notes that "deposit and forward" designs weaken both the exclusive-control and own-funds positions [I].
- **Processor-covered fees.** These, and contributions reversed by return or chargeback, need acknowledgment correction (CNV-FIN-003, CNV-PAY-006).

## 3. Characterization-determining facts (collect before any legal characterization or owner reassessment)
1. Legal entity map: Echelon entity operating Unbound (OQ-102); whether any sponsor is affiliated with Echelon (OQ-756); whether any customer is a bank or bank affiliate (F12).
2. For every account touched: legal title, signatories, who holds bank-channel credentials, whether Echelon personnel or systems can release without a sponsor step.
3. For every rail: who holds the ODFI or partner agreement; who transmits files; who receives return and NOC files.
4. Payment-partner contract: does the partner debit sponsor accounts, hold funds in transit for the sponsor or the charity, and is it a licensed money transmitter or a bank? Is Echelon a party?
5. Echelon compensation: invoice versus netting from funds; any asset-, trade- or transaction-based component (CNV-LEG-001 restricts).
6. Any Echelon-provided "faster grant" liquidity, advances or float.
7. Whether contributions are ever solicited from the public through Echelon-hosted pages (CA AB 488 platform question, REG-046, CNV-LEG-002).
8. Recurring giving design: donor debit authorizations (F4) only when activated, and who is Originator.

## 4. Questions for counsel (with our proposed interpretation)
| Q | Question | Our proposed interpretation [AS] | Blocks |
|---|---|---|---|
| CQ-1 | If Echelon operates Unbound per CNV-LEG-001 (no accounts, no execution capability, instructions under sponsor agreements), is Echelon a money transmitter under 31 CFR 1010.100(ff)(5) or any target state's law? | No under limb (i)(A), because Echelon never accepts value; limb (i)(B) ("any other person engaged in the transfer of funds") needs a recorded legal resolution (internal research; external advice only after an owner reassessment under DF-ASR-2026-0D0F) for instruction and file transmission; (ii)(A) may apply only where the recipient (ODFI, partner) is itself a money transmitter. State laws to be checked for "receiving money for transmission" interpretations. | Production launch; the baseline handles it through the BR-FIN-020 activation gate |
| CQ-2 | In the networked-partner rail (F9), does Echelon's sending of payment instructions to the partner make Echelon an agent or delegate of the partner, or a party to transmission, in any state? Does the answer change if Echelon has its own contract with the partner? | No, provided Echelon contracts only as the sponsor's technology provider and the sponsor's partner agreement authorizes instructions. | Production launch of F9 |
| CQ-3 | Can any exclusion ((ii)(B) payment processor, (ii)(F) integral, state agent-of-payee) be relied on for contribution collection or grant payment if Echelon ever touches funds? | Do not rely on them: grants are not goods/services; design so they are unnecessary. | Production launch (baseline already assumes they are unavailable) |
| CQ-4 | For sponsor-originated ACH (F7), is Echelon a Nacha TPSP only, and what TPSP obligations (fraud monitoring, data security, audit) attach by contract? Would transmitting files directly to the ODFI change status? | TPSP only while the sponsor holds the ODFI agreement; direct transmission does not by itself create TPS status. | Implementation (bank adapter) |
| CQ-5 | If a bank or bank subsidiary is the contracting customer (decision 1, F12), is Echelon subject to 12 U.S.C. 1867(c) examination, and which regulator's third-party and incident-notice expectations apply contractually? | Likely within 1867(c) for data processing and recordkeeping services to the bank; plan for examination access and bank TPRM terms. | Production activation of bank customer onboarding (F12); technical build not blocked (ODP-05) |
| CQ-6 | Does "servicing" by Echelon staff (e.g., an E5 administrator model) risk Echelon holding sponsor authority, contrary to CNV-GOV-004? Which servicing tasks may Echelon perform without becoming an agent with authority over charitable assets? | Echelon may perform A0-A2 tasks under per-sponsor ServiceDelegation; never A3+ or payment release. | Production launch (baseline already encodes the restriction in BR-FIN-006, BR-FIN-010, BR-FIN-018) |
| CQ-7 | Allocation of OFAC screening, blocking and reporting between sponsor and Echelon; is Echelon's own exposure satisfied by screening on the sponsor's behalf? | Sponsor blocks and reports; Echelon screens and fences as contracted and maintains its own risk-based program. | Implementation |
| CQ-8 | Does Unbound-generated acknowledgment content and timing (including the 170(f)(18) statement and corrections after returns) meet substantiation rules when issued in the sponsor's name by a vendor system? | Yes if issued in the sponsor's name from a sponsor-approved template; Echelon never named as donee. | Production launch |
| CQ-9 | Does a donor-covered processing fee, or an Echelon platform fee paid by the donor, change acknowledgment amount or create money-transmission or fundraising-platform issues? | Follow CNV-PAY-006; Echelon fees invoiced, never netted. | Production launch (card/processor) |
| CQ-10 | Is a protective hold placed by Echelon (CNV-PLT-007) an exercise of control over funds that affects custody or transmission analysis? | No: time-bounded, sponsor-adopted, sponsor can override via its own bank channel. | Production launch |
| CQ-11 | State unclaimed-property treatment of voided grant checks and priority rules (CNV-PAY-003). | As Unbound default: hold and report when unknown. | Production launch |

## 5. Blocking summary
- **Correction (consolidation challenge pass, 2026-11-25).** These questions were first labelled as blocking requirements finalization. The baseline now expresses each answer-dependent choice as a gated requirement (BR-FIN-020 activation gate; BR-FIN-006 authority limits), so the requirements can be finalized while the questions stay open; they block production launch or the specific flow. The register (`registers/question-dispositions.csv`) is authoritative for blocking tiers.
- **Originally listed before requirements finalization:**
  - CQ-1, CQ-3, CQ-5, CQ-6 and CQ-10;
  - entity map (fact 1);
  - resolution of the decision 1 contradiction (financial institutions as customers versus "no direct bank service relationship").

  These fix the wording of FIN-BC-013, FIN-BC-026 and the servicing scope.
- **Before implementation of payment adapters:** CQ-4, CQ-7; bank-file layouts (CNV-PAY-005 activation gate).
- **Before production launch (per rail and per sponsor):**
  - CQ-2, CQ-8, CQ-9, CQ-11;
  - executed ODFI, positive-pay, partner and custodian agreements (CNV-PAY-001/002/007 gates);
  - state-by-state licensing confirmation for any flow where a fact in §3 changes.

## 6. Limits
- Primary sources were read through a summarising fetch tool, and several are unofficial compilations (Cornell LII). Verify the official text before citing.
- The Nacha Operating Rules text and enacted state statutes were not read.
- The FinCEN agent-of-payee rulings (e.g., FIN-2009-R004) were found by search but not retrieved, so they are not relied on.
- No conclusion here is legal advice.

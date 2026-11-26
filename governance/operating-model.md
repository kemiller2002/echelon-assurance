---
id: ASR-GOV-OPM-001
title: AI-native organizational operating model
status: proposed (no appointments, staffing or deployed controls asserted)
created: 2026-11-25
requirements: [BR-GOV-004, BR-GOV-006, BR-GOV-007, BR-GOV-008, BR-GOV-009, BR-GOV-011, BR-GOV-012, BR-GOV-013, BR-GOV-016, BR-IAM-002, BR-IAM-003, BR-IAM-004, BR-HR-001, BR-HR-002, BR-IR-001, BR-BCR-001, BR-VEN-001, BR-CUS-001, BR-AI-001, BR-AI-006, BR-VAL-001]
supersedes_detail_of: requirements/organizational-governance.md (GOV-BC-001..026)
---

# Operating model

## Principle

The model is organized around responsibilities, not departments. A responsibility
is an obligation with:

- an accountable human;
- a delegated executor (agent, deterministic worker or person);
- authority limits;
- an evidence obligation;
- an escalation path.

Headcount follows workload, risk and independence obligations (BR-GOV-011
tracks them). Automation executes delegated work. It never carries legal
accountability.

## Responsibility register (BR-GOV-007)

The register belongs in the organization-administration repository as a
machine-readable record (WI-EOA-01). Until then, this table is the proposed seed.
Names are not recorded. As of 2026-11-25 one human, the owner, holds every
accountable role.

| Responsibility | Accountable | Delegated executor | Authority level of executor | Human involvement | Evidence |
|---|---|---|---|---|---|
| Policy and control lifecycle | Owner | Assurance coordinator agent | A1 propose; A2 regenerate derived artifacts | Approves material changes (A4) | Signed approvals (EVT-021) |
| Release integrity | Owner | Implementing agents; Dokimos verifier (separate identity) | A2 within standing authorization | Routine changes: exception only. Elevated changes (authorization, release, ledger, payment adapter, tenant isolation, verifier, required checks): explicit owner approval on the change (BR-SDL-003) | Receipts (EVT-003, EVT-006) |
| Financial operations (per sponsor) | Sponsor (customer) | Deterministic workers | None for agents (BR-FIN-006) | Sponsor approves releases and exceptions | Authorization records (EVT-018) |
| Reconciliation and close (per sponsor) | Sponsor-appointed approver | Reconciliation worker (separately credentialed identity) | Deterministic | Sponsor approver reviews breaks, adjustments and close; Echelon staff operate the platform but never hold reserved (A3+) financial authority | EVT-017, EVT-022 |
| Identity and privileged access | Owner | Access-as-code pipeline | A2 for reversible drift reversion | Approves privilege grants and break-glass review | EVT-009, EVT-030 |
| Continuous security testing | Engineering lead (owner today) | CI suites, triage agent | A1 triage; A2 open findings | Accepts risk on exceptions | EVT-004, EVT-011 |
| Incident response | Owner | Containment playbooks; agents for evidence gathering | A2 containment only where pre-authorized | Declares incident; makes notification decisions | EVT-016 |
| Regulatory monitoring | Owner | Monitoring agent | A1 propose interpretation changes | Approves activation (BR-GOV-012) | Proposals and approvals |
| Vendor and subservice | Owner | Register agent | A1 | Approves new providers and AI data classes | Register versions |
| Customer assurance answers | Owner | Answer generator | A1 | Approves answers by disclosure class | Answer versions |
| Management review | Owner | Package generator | A1 | Reviews material items (BR-GOV-011) | Signed review record |
| Succession and continuity | Owner | None | — | Designates the delegate when operationally necessary (ODP-04; role defined, naming deferred) | Exercise record |

## What is eliminated, automated or exception-based

| Conventional activity | Treatment | Mechanism |
|---|---|---|
| Quarterly user access review spreadsheets | **Eliminated**; replaced by continuous comparison | BR-IAM-004 access as code plus drift detection; humans review exceptions only |
| Evidence screenshots and collection sprints | **Automated** at enforcement points | BR-ASR-001 envelope; BR-ASR-003 populations |
| Standing compliance meetings | **Eliminated**; replaced by generated packages and event triggers | BR-GOV-011; minimum cadence only where an obligation requires one |
| Manual status reporting | **Automated**; status computed from evidence, unknown is not pass | BR-ASR-002 |
| Annual calendar training | **Event-triggered** role-based competency | BR-HR-001 |
| Policy document maintenance | **Generated** from the policy model where duplication is removed | BR-GOV-002 |
| Questionnaire answering | **Generated** with exception review | BR-CUS-001 |
| Change approval boards | **Replaced** by standing authorization, separate verifier and enforced gate for routine changes | Owner decision 0D04; BR-SDL-001 |
| Exception approvals | **Kept human**, bounded and expiring | BR-GOV-004 |
| Risk acceptance, policy weakening, management assertions, legal commitments | **Kept human (A4)** | BR-GOV-007 |

## Separation-of-duties conflicts that cannot be designed away

With one human, these conflicts are recorded, not hidden:

1. **Approver equals requester** for owner-originated material exceptions and policy changes. Policy changes the owner makes are recorded with the reason and pass the multi-agent review protocol and validators; they are not represented as independently approved. For material exceptions: Rule (BR-GOV-004, owner direction DF-ASR-2026-0D0F): self-approval of material exceptions is not authorized. Such an exception is blocked, or the change is redesigned, until a qualified approver exists. Internal development review uses the multi-agent review protocol (BR-VAL-004). That protocol is technical review, not organizational independence. Under owner decision 0D04, routine agent-prepared work is approved through the owner's standing authorization with a separately credentialed technical verifier and an enforced gate; this is technical separation, not organizational independence. Organizational independence is unavailable until a second human or fractional reviewer exists (ODP-02).
2. **Administrator equals monitored party.** The owner can disable rulesets, retention locks, reconciliation, heartbeats and alerts. Separately credentialed drift detection and out-of-band alerts detect system failures and external compromise, **not owner action**: every alert reaches the owner. Owner-proof assurance needs records the owner does not control (sponsor- or bank-side records, a compliance-mode retention lock) and an external reviewer (ODP-02, ODP-03).
3. **Whistleblowing and complaints about the owner** need an external recipient (ODP-04).
4. **Fraud review of the owner's own financial actions:**
   - For sponsor funds: sponsors, not Echelon, authorize movement (BR-FIN-006), which removes most of the conflict for Unbound.
   - For Echelon's own finances: this is outside this program's scope. It is recorded for the organization-administration repository.

Economically viable mitigations, in order:

1. Design so the sponsor is the second party for money.
2. Use the technical verifier for routine work.
3. Use the internal multi-agent review protocol for development review (BR-VAL-004, `methodology/review-protocol.md`); no fractional reviewer is contracted now (owner direction DF-ASR-2026-0D0F). Reassess when an observation period, production risk acceptance or an obligation requires organizational independence.
4. Escalate to a second hire only when workload or obligations justify it (BR-GOV-011 metrics).

No personnel, approvals or reviewers are invented here.

## Defined roles (owner direction DF-ASR-2026-0D10, ODP-04)

Individuals are named when operationally necessary. The proposed interpretation
is no later than production launch; the owner has not set that deadline.

| Role | Authority | Not authorized | Activation and procedure | Evidence |
|---|---|---|---|---|
| Emergency delegate | Suspend agent runs and credentials; freeze releases (pause the release gate); contact sponsors, partners and providers; use sealed recovery credentials to restore access | Approve policy changes, exceptions, risk acceptance or money movement; change controls or retention | Activated when the owner is unreachable for a defined period or on the owner's instruction; follows the human-readable runbook, which works without AI; every action logged out of band | Activation and action log (EVT-034) |
| Protected-reporting recipient | Receive whistleblowing, conflict-of-interest and complaint reports concerning the owner; retain them outside owner-administered systems; decide whether to escalate externally | Act on Echelon systems; access production data | Reports go to a channel the owner does not administer; the recipient acknowledges receipt and records disposition; no retaliation | Report and disposition records (restricted) |

## Owner unavailability (BR-GOV-013)

- **Delegate.** The emergency delegate role (ODP-04; the individual is designated when operationally necessary, DF-ASR-2026-0D10) can:
  - suspend agents (BR-AI-005);
  - freeze releases;
  - contact sponsors and partners;
  - access sealed recovery credentials.
- **No routine powers.** The delegate cannot approve policy changes or exceptions.
- **Works without AI.** Operating instructions are human-readable and function without AI (BR-AI-006).
- **Exercises.** The procedure is exercised by tabletop at least annually and on material change.

## Cadence

| Activity | Trigger |
|---|---|
| Controls, evidence and status | Continuous |
| Exceptions and findings | On event, within severity windows |
| Management review | On material change and monthly while operations are active; minimum annual review of policies and continuity |
| Vendor and obligation review | On onboarding, contract change, or quarterly where vendors hold restricted data |
| Policy review | On material change, incident or obligation change; at least annually where an obligation requires it |

These are proposed defaults, not statutory frequencies. Customer contracts may impose others (BR-GOV-009).

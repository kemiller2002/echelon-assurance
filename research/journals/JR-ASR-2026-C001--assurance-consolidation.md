---
id: JR-ASR-2026-C001
title: Assurance consolidation, challenge and requirements finalization
status: complete pending owner decisions
created: 2026-11-25
branch: claude/echelon-assurance-consolidation-jygy23
---

# JR-ASR-2026-C001: Assurance consolidation

## Inputs verified

- PR #1 is `claude/fervent-tesla-ytnxz6` (head 37cbdc5, 56 commits) and PR #2 is `claude/echelon-transparency-governance-rvt82z` (1e23430). Both were open drafts on 2026-11-25.
- Branch `claude/echelon-availability-architecture-ezhitq` had no commits beyond `main`.
- PR #2 was not an ancestor of PR #1, so both were merged into the consolidation branch, keeping their history.
- All files were read. The structure of every register was read by script. The nine requirement documents, the owner decisions and the requirement narratives were read in full by the coordinator.
- Echelon system repositories were cloned read-only at their 2026-11-25 heads (31 repositories, including the private organization-administration repository at capability level only). Tests were executed for Arca, Dokimos, Strata and Tutela, and partially for Praxis. The rest were source-inspected (`integration/capability-verification.csv`).
- Clarity and EDF were read from their authoritative repositories:
  - EDF v0.3, the frozen baseline, at 710d54c.
  - The Clarity Result template (Candidate status) at 200a9eb.

## Method

1. **Research agents.** Five agents worked in parallel on narrow briefs: capability verification, financial review against Unbound and daf-landscape, customer question bank, question register disposition, and methodology extraction. They wrote only to a scratch area; the coordinator integrated their output into the registers.
2. **Baseline.** The coordinator wrote the baseline (`requirements/baseline.csv`), the tenets, the dispositions, the decisions C001–C006, the economics v2 model and the backlog.
3. **Tooling.** The F# tooling was extended with the Traceability and Economics modules and the `trace` and `econ` commands. Unit tests went from 33 to 63. CI now checks that the generated views are current.
4. **Challenge round 1.** Two adversarial review agents worked from the repository, not from the coordinator's conclusions. One covered structure and consistency (passes 1, 2, 9, 13, 14, 15). The other covered domain correctness (passes 3–8, 10–12). They reported 59 findings, 33 of them material, archived in `findings/challenge-passes/`.
5. **Fixes.** All material findings were addressed in commit 8bec065. New validator rules prevent the mechanically checkable defect classes from recurring.
6. **Challenge rounds 2–4.** Fresh agents verified each round's fixes and looked for new defects. New material findings fell from 33 to 4, then 3, then 1. The last round found only unescalated owner decisions. See `findings/validation-report.md` for the stopping rationale.

## Material corrections made by the challenge passes

- **Semantic link errors.** Policy, validation and evidence links had been assigned per area rather than per requirement. They are now derived from each requirement's controls and checked by the validator.
- **Economics bias.** The first v2 run charged maintenance and non-reliance only to Echelon-built alternatives. The corrected model reverses the hybrid recommendation: minimum viable assurance is the robust choice.
- **FinCEN.** The analysis omitted limb (i)(B), "any other person engaged in the transfer of funds", and stated a legal reading as fact.
- **Evidence storage.** The replacement for the custom ledger was not actually locked in the interim. A compliance-mode lock now applies from the first production evidence.
- **Customer answers.** Some answers overstated independence or described the removed ledger.
- **One-human exception rule.** It was inconsistent across the baseline, the operating model and a superseded decision.
- **Gaps.** 32 requirements had no work item, five controls had no baseline requirement, and the 18 reconciliation findings had no disposition.

## Owner direction reconciliation

The owner's direction of 2026-11-25 (DF-ASR-2026-0D0F) was reconciled after round 4. See `findings/validation-report.md`, section 3a.

## Independence statement

Every step was performed by AI agents controlled by Echelon. The review agents
had separate contexts and were not given the coordinator's conclusions. That is
a structured self-review with technical separation. It is **not** organizational
independence and not external assurance (BR-VAL-001, BR-ASR-002).

## What was not done

- No other repository was modified.
- No purchases, engagements or consultations were made.
- No production implementation was done.
- Branch protection was not changed; it is an owner action.
- Praxis is still not installed in this repository (owner action WI-ASR-03).

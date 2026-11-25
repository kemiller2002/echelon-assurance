---
id: DF-ASR-2026-7B20
title: Small-team approval and independence rule
status: superseded
decided_by: pending owner (superseded by DF-ASR-2026-0D04; one-human exception rule now in BR-GOV-004, awaiting ODP-02)
date: 2026-11-25
resolves: [OQ-800 (pending owner), OQ-803 (partly)]
---

# DF-ASR-2026-7B20

## Context
Three documents conflicted (review pass 4–6): ai-governance §4 P1 excludes
everyone in the preparer's delegation chain from approving, which leaves no
eligible approver when one human sponsors all agents; POL-11.03 lets the
sponsor approve with "independent" sampling nobody performs; CTL-GOV-009
routes gaps to exceptions that themselves need an independent approver.

## Decision (proposed)
While Echelon has fewer than two humans able to approve:
1. **Agent-prepared work** may be approved by the sponsoring human. The agent
   is the preparer; the human is not. Agents can never approve (unchanged).
2. **Work the owner personally authored, and exceptions the owner requests**,
   may be approved by the owner only as a recorded **compensating-control
   exception**, and every such approval enters a review queue examined by a
   **contracted independent reviewer** at least quarterly (100% of financial,
   access, policy and exception approvals; sample of the rest).
3. Exceptions above the high-risk threshold additionally require the
   external adviser's written concurrence before taking effect.
4. The stricter rule in ai-governance §4 P1 (approver outside the preparer's
   delegation chain, Unbound RR-AIN-003-r1) becomes mandatory automatically
   when a second qualified human approver exists.
5. System descriptions and customer statements disclose this limitation
   rather than imply full segregation (CTL-GOV-012).

## Consequences
Restores a workable, honest rule. Cost: a contracted reviewer (assumption
USD 10k–30k/yr, added to economics scenarios 4/5/5b). An auditor may still
treat the owner's concentration of duties as a limitation (OQ-701).

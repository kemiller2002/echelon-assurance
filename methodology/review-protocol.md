---
id: ASR-METH-REVIEW-001
title: Internal multi-agent review protocol
status: canonical
created: 2026-11-25
authority: owner direction DF-ASR-2026-0D0F (docs/decisions/owner-review-direction-2026-11-25.md)
requirement: BR-VAL-004
---

# Internal multi-agent review protocol

Under owner direction DF-ASR-2026-0D0F, this protocol is sufficient for
**internal development review**. It is technical review. It is not
organizational independence, legal advice or external attestation, and agent
consensus alone is not proof.

## When it applies

Use it for requirements, decisions, regulatory interpretations, economic
models and any material change to the assurance baseline. Executable claims are
checked by deterministic tests and the validators (`assurance validate`,
`praxis validate`), not by review alone.

## Steps

1. **Separate initial analyses.** Two or more agents analyse the question in separate contexts, from the repository and primary sources, without being given each other's conclusions.
2. **Adversarial challenge.** Reviewers are told to disprove or weaken the conclusions, not to confirm them. Each finding records:
   - severity (`material` or `minor`);
   - the artifact and identifiers concerned;
   - evidence (paths, quotes, source URLs with retrieval dates);
   - a recommended fix.
3. **Alternative interpretations.** For legal, regulatory and auditor questions, record the competing readings and the facts that would decide between them. Never collapse them into a conclusion.
4. **Citation and date verification.** Each cited source is checked for existence, the exact provision and its effective date. A summary page is never presented as primary text.
5. **Deterministic verification.**
   - Counts, numbers and links are recomputed by script or validator.
   - Executable claims are checked by tests.
   - Derived numbers in prose are recomputed from the generated outputs.
6. **Evidence-based reconciliation.**
   - The coordinator fixes or escalates every material finding.
   - A fresh reviewer verifies the fixes (status `fixed`, `partially-fixed`, `not-fixed` or `new`).
   - Repeat until a round finds no new material defect, or the remaining items need owner decisions, external evidence or implementation.
7. **Record provenance.** Each round's findings are kept in `findings/challenge-passes/` with scope, round and dispositions. The stopping rationale goes in the validation report, and disagreements, assumptions, limitations and unresolved risks go in the relevant register.

## Known limits

- Agents from the same model family share failure modes (RSK-022); prompts and contexts are separated, but this is not independence of judgment.
- The protocol cannot resolve questions that only a qualified professional, an auditor, a regulator or a counterparty can answer. Those stay open with their external dependency (DF-ASR-2026-0D0F boundaries).

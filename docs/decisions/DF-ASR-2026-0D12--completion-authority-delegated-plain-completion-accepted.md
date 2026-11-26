---
id: DF-ASR-2026-0D12
title: Completion decision delegated; plain completion accepted for the final reconciliation
status: accepted
decided_by: owner (in conversation, 2026-11-26, answering whether to provision a Praxis completion authority or accept plain completion: "you have the authority"); applied by the implementing agent under that delegation
date: 2026-11-26
---

# DF-ASR-2026-0D12

## Context

Owner record DF-ASR-2026-0D10 requires the applicable Praxis verified completion. `praxis work complete-verified` needs a completion authority with pinned verifier keys, and none is provisioned (WI-PRAXIS-03). PR #3 was merged into `main` (9ab2b00) on the owner's direct instruction, without verified completion.

The owner was asked to either provision an authority or record that plain completion is acceptable. The owner delegated the decision.

## Decision (taken under the owner's delegation)

Accepting plain completion in place of verified completion departs from DF-ASR-2026-0D10. An agent may not approve that departure for itself (BR-GOV-004, BR-AI-002). The approval here is the owner's: the owner was asked to choose between provisioning an authority and recording that plain completion is acceptable, and delegated that choice. The agent applies the narrower option, which makes no claim of verification.

1. **No self-made verifier key.** The implementing agent does not create a completion authority from a key it holds. A receipt signed with such a key would be self-attested. It would not be the separately credentialed verification that verified completion means (DF-ASR-2026-0D04; VAL-040), and presenting it as verified would be a fabricated verification receipt.
2. **Plain completion accepted.** For the final reconciliation work item WI-ASR-FINAL, plain Praxis completion is accepted. The completion records the validations run under `praxis work validate-run` on the completed commit, and the review record (`findings/reviews/RV-ASR-2026-11-26.md`). Nothing describes it as verified completion.
3. **WI-PRAXIS-03 stays open.** Provisioning a verifier with its own identity and pinned keys remains open. Verified completion stays the target for later work (OQ-704, DF-ASR-2026-0D11).

## Consequences

- Plain completion was accepted, but Praxis refused it for WI-ASR-FINAL: a claim-owned work item can be completed only by verified completion (PRX.COMPLETION.VERIFIED_COMPLETION_REQUIRED, 2026-11-26). The evidence and final disposition are recorded in the Praxis work history. The work item stays active until WI-PRAXIS-03 provides a verifier, and the rule is not bypassed.
- PR #3 was merged on the owner's instruction. Pull request #4 is merged only on the owner's instruction, after its final commit is validated (DF-ASR-2026-0D11). Neither rests on verified completion.
- No spending, external engagement, production activation or change to another repository is authorized by this record.

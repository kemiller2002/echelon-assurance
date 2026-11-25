---
id: JR-ASR-2026-5C02
title: Review passes 4, 5, 6, 8, 9 and 10 - consistency, automation feasibility, independence, security architecture, Unbound fit, auditor-acceptance wording
status: complete
created: 2026-11-25
---

# Review passes 4, 5, 6, 8, 9 and 10

This was an independent, adversarial reviewer pass on branch
`claude/fervent-tesla-ytnxz6`. Another reviewer was checking source accuracy
and economics at the same time, so I kept my edits to exact, targeted
replacements. I did not edit `tools/*.py`, and I made no commits. Each
register edit was followed by `python3 -I tools/validate_registers.py` (OK)
and `python3 -I tools/merge_fragments.py --check` (rc 0).

## Method

- **Catalog statistics.** I recomputed them from
  `controls/control-catalog.csv` with Python: automation targets, failure
  modes, phases, domains, and validation independence per control.
- **ID references.** I cross-checked every CTL/POL/OQ/VAL/RSK/EVT/REQ/SYS/SRC
  ID cited in Markdown and CSV (outside fragments and journals) against the
  registers. Only one ID did not resolve: the illustrative `SRC-AICPA-001` in
  `methodology/identifiers.md`.
- **Roadmap.** I compared each phase's narrative with the catalog's
  `roadmap_phase` values.
- **Policies.** I checked the policy inventory against the catalog
  (control↔policy symmetry) and against the draft files.
- **Unbound.** I spot-checked against
  `/home/user/unbound/requirements/governance/SPECIFIC-EXECUTABLE-CONTROLS.csv`
  (UNB-SCTRL-001..045), the CUEC identifiers across Unbound requirements, and
  the Unbound decision IDs cited (XID-INV-008, SPD-PLT-001, SPD-ADV-003,
  CNV-PAY-001, SOC2-023/027 and others; all exist). I also checked the
  daf-landscape `financial-core-control-spec-v0.5.md` gate list.

## Pass 4 - Internal consistency

Findings:

1. **Policy inventory count was wrong.** The register holds 13 policies and
   70 sections, not 68. Two sections (POL-12.06, POL-13.05) were marked
   `inventory` with no `draft_path`, although both are drafted in the policy
   files.
2. **The roadmap understated the daf-landscape gates.** It said "G1–G7
   before any production financial execution". The spec defines G0–G9: G8 is
   SOC 2 evidence and independent review, G9 is pilot approval, and payment
   and trade execution are gated separately.
3. **VAL-014 was phased too late.** In the validation architecture §7,
   VAL-014 (cross-tenant isolation tests) sat in P2. CTL-TEN-001 is a P1
   control, and daf gate G2 requires the tenant-isolation tests before
   production.
4. **Two documents timed the first pen test differently.** The validation
   architecture §7 put VAL-027 (pen test) "before first SOC 2 period". The
   roadmap and CTL-VUL-002 put the first independent test in P1, before
   launch.
5. **Pen-test stage labels collided with roadmap phases.** The pen-test
   strategy uses its own P0–P5 stages ("P2 first independent test"), which
   read as roadmap phases.
6. **The restore-test frequency conflicted.** CTL-BCR-001 and POL-09 say
   quarterly; VAL-012 said semi-annual.
7. **Two documents disagreed on when `Tested` needs independence.** The
   validation architecture §5 required `internal-independent` "for key
   controls"; `controls/README.md` requires it for every control.
8. **The automation ceilings looked inconsistent with the catalog.** The
   automation matrix §4 caps exception and policy approval at L1–L2, but the
   catalog has CTL-GOV-007 and CTL-GOV-002 at L4. They are reconcilable: L4
   enforces *who* may decide, not the decision itself. The matrix did not say
   so.
9. **Not fixed: the ledger substrate is undecided.** DF-ASR-2026-3A1E extends
   Arca, whose records are GitHub-backed JSON. `evidence-architecture.md` §2
   and §5 specify a transparency log plus a WORM object store and say a
   shared GitHub repo is unacceptable for production evidence. PC-12 is P0,
   CTL-ASR-002 is P3, and economics pass 7 advises deferring the ledger.
   → OQ-805.
10. **Not fixed: the minimum set and the catalog targets differ.** The
    vendor-analysis minimum capability set, which economics scenario 5
    prices, targets personnel at L1, endpoint at L2, evidence store at L2 and
    policy lifecycle at L1. The catalog targets HR-001..003 at L4/L5, END-001
    at L4, ASR-002 at L4 and GOV-002 at L4. So scenario 5 prices a lower
    automation level than the catalog plans. → OQ-802.
11. **Not fixed: the P3 exit criterion cannot be met as catalogued.** The
    roadmap's P3 exit needs every in-scope control `Tested` at
    `internal-independent`. 65 of 104 controls map only to `automated` or
    `self-assessment` VAL procedures, and no economics scenario budgets the
    reviewer (CTL-ASR-007 / VAL-025). → OQ-803.
12. **Checks that passed:**
    - Roadmap phase counts (P0 13, P1 67, P2 13, P3 10, P4 1) match the
      catalog.
    - Economics "80 of 104 shared controls" recomputes correctly.
    - Catalog distribution (L4 49, L3 27, L2 11, L1 6, L0 6, L5 5) matches
      the README.
    - Catalog→policy links are symmetric. The top-level-only policy→control
      links are the documented inheritance.
    - SOC applicability, financial controls, roadmap P4 and economics agree
      on SOC 1 Type 2 + SOC 2 Type 2 (Security, Availability,
      Confidentiality) and on no standalone SOC 1 Type 1.

Edits:

- `policies/policy-inventory.md`: changed "68 sections" to "70 sections".
- `policies/policy-inventory.csv`: set POL-12.06 and POL-13.05 to
  `drafted`, with their draft paths.
- `roadmap/implementation-roadmap.md`:
  - changed G1–G7 to G0–G9 with the G8/G9 meaning;
  - added a P3 exit-criterion gap note citing OQ-803.
- `validation/independent-validation-architecture.md`:
  - moved VAL-014 into P0–P1;
  - noted that VAL-027's first run is in roadmap P1;
  - aligned the `Tested` rule with the README.
- `pentest/unbound-minimum-cost-testing-strategy.md`: added a note that its
  stages are not roadmap phases, and that stage P2 falls in roadmap P1.
- `registers/validation-procedures.csv`: changed VAL-012's frequency to
  quarterly restore plus an annual full exercise.
- `automation/control-automation-matrix.md`: clarified in §4 that the
  ceiling applies to the decision, not to enforcing who decides.

## Pass 5 - Automation feasibility

Findings:

1. **L5 retention is irreversible.** CTL-DAT-002 (retention, crypto-shredding)
   targets L5, which is irreversible. That breaks the matrix L5 rule
   (reversible, bounded) and ai-governance §14 ("reversible and idempotent").
   The README claimed all five L5 controls are reversible. → OQ-801;
   README corrected.
2. **Vigila is mapped as a logging and alerting system it is not.** The
   catalog uses SYS-VIGILA for:
   - CTL-LOG-001 (security event logging);
   - CTL-LOG-003 (alerting);
   - CTL-ASR-004 (heartbeat, "monitor in separate trust domain").

   The system register and the ECH2 assessment say Vigila is a
   personal/business follow-up tracker, at most L1 for control tasks. The
   integration matrix row mislabelled it "Observability". Matrix row
   corrected; catalog mapping left for the owner. → OQ-806.
3. **HR controls are over-automated for the current setup.** CTL-HR-001..003
   are P0 with L4/L5 targets and no Echelon system. They depend on an HR
   system, an IdP (OQ-764 undecided) and a training platform that do not
   exist. The vendor analysis itself calls a spreadsheet sufficient at
   current headcount. This is an over-claim for "immediate" P0. → OQ-802.
4. **Several L4 targets rest on mechanisms that are not switched on:**
   - CTL-SDL-001/002 and CTL-GOV-003 depend on GitHub rulesets (state
     unevidenced, OQ-700) and on Praxis/Dokimos verified completion
     (implemented, enabled nowhere, PC-03).
   - Ruleset-change detection needs org audit-log access (OQ-508, a plan
     dependency).
   - CTL-ASR-002 depends on the Arca hash chain, which is explicitly
     deferred in Arca.
   - CTL-GOV-008 depends on the Ordo authority matrix, which exists only as
     types (PC-06, P2).

   These targets are feasible but not near-term. The catalog's "Designed"
   status is honest; the roadmap's P0 instruction to turn on CTL-SDL-001 "no
   admin bypass" cannot bind the GitHub org owner (already stated in
   known_limitations).
5. **CTL-AI-003 (prompt injection) at L4 is acceptable only as
   architecture.** Separate read and act identities is how the catalog
   states it. The known_limitations entry is honest.

Edits:

- `controls/README.md`: corrected the L5 reversibility sentence and cited
  OQ-801.
- `integration/echelon-integration-matrix.md`: corrected the Vigila purpose.

## Pass 6 - Validation independence

Findings:

1. **VAL-019 counted agent runs as independent.** It was labelled
   `internal-independent` but performed by "Human or validation agent". By
   the validation architecture's own rule, an Echelon agent run is
   `automated`. Fixed in the register.
2. **PC-16 called an Echelon system an external witness.** It described
   Specula, which Echelon runs, as an "External-witness candidate", which
   breaks AGENTS rule 4. Reworded.
3. **The small-team approval rules conflict, and none is workable.**
   - ai-governance §4 P1 forbids approval by anyone in the preparer's
     delegation chain. With one human sponsoring all agents, that leaves no
     eligible approver for agent work.
   - POL-11.03 allows sponsor approval plus post-merge "independent"
     sampling, which no one is available to perform.
   - CTL-GOV-009 sends small-team gaps to POL-01.05 exceptions. Those need
     an approver independent of the requester, which is circular when the
     owner is the requester.

   → OQ-800 (P0). Note added to ai-governance §8.
4. **VAL-011 reconciliation is internal-independent only with a second
   human.** It is labelled `internal-independent` and described as a "job
   owned by someone other than the control owner". That holds only if such a
   person exists (OQ-608).
5. **The pen-test strategy states the independence problem honestly.**
   Echelon has no independent person; tests are labelled accordingly.

Edits:

- `registers/validation-procedures.csv`: VAL-019 performer changed to a
  human, with agent runs counted as `automated`.
- `integration/proposed-changes.md`: PC-16 reason reworded.
- `ai-governance/ai-governance-model.md`: OQ-800 note added in §8.

## Pass 8 - Security architecture

Findings, ranked:

1. **Administrative roles are concentrated in one person.** Evidence
   architecture §5.3, CTL-ASR-006 ("admin role ≠ application operators") and
   R1 ("JIT with second-person approval") all assume a second administrator.
   With one human holding cloud root, KMS admin, GitHub org owner and IdP
   admin:
   - deleting the evidence account defeats S3 compliance-mode WORM;
   - deleting KMS keys silently "crypto-shreds" any payload, so key deletion
     is an evidence-destruction path;
   - rulesets can be changed by the org owner.

   → OQ-804; §5.3 annotated.
2. **The evidence store's design is split.** The architecture specifies a
   transparency log plus WORM; the decision record specifies Arca on GitHub.
   Trust boundaries differ materially (GitHub org owners can rewrite
   history, ECH2 F2). → OQ-805; §2 annotated.
3. **The ingest gateway is a single point of trust.** It assigns
   `recorded_at` and authenticates collectors. A producer or gateway can omit
   records before sequencing (acknowledged as "Producer can omit before
   numbering"). Population reconciliation from the source is the only
   control, and its independence depends on a second person (Pass 6 item 4).
4. **Not fixed: VAL-026 is cited as an R7 collusion detector.** The
   resilience doc's R7 cites VAL-026 (external technical review of the
   assurance machinery) for approval-pattern sampling, which that procedure
   does not cover. Coordinator should re-point it to VAL-025/032.
5. **Two P1 controls have their exercises only in P2.** VAL-012 (restore)
   and VAL-022 (kill switch) are listed under P2 in validation §7, while
   CTL-BCR-001 and CTL-IR-003 are P1 controls whose P1 exit needs a recorded
   negative test.

## Pass 9 - Unbound-specific

Findings:

1. **Charter section 16 cannot be checked from this repository.**
   `PROJECT-CHARTER.md` does not contain charter §16 (or §22), so I checked
   coverage against the financial doc's own control areas. All 20 areas map
   to FIN/TEN controls. Donor restrictions, charity eligibility and
   historical corrections are covered only inside FIN-006 and FIN-001.
   → OQ-807 (owner to add the full charter).
2. **Catalog Unbound refs were incomplete.** Against UNB-SCTRL-001..045 they
   omitted:
   - 012/013/014 (legal rule publication, exemption, sanctions screening);
   - 018 (post-approval eligibility drift);
   - 020 (recipient bank detail changes, which is the payee-change control);
   - 023 (investment authority);
   - 026 (grant payable recognition);
   - 029 (payment instruction authority).

   Added to FIN-006, FIN-002 and FIN-001.
3. **The financial doc cited the wrong payee-change control.** It cited
   029/030 for payee bank change; the matching control is UNB-SCTRL-020.
   Corrected.
4. **Remaining Unbound controls with no catalog home:**
   - UNB-SCTRL-030 (live payment execution prohibition);
   - UNB-SCTRL-033 (tax reporting reconciliation).

   I left these unmapped because FIN-014 and FIN-012 have empty
   `unbound_refs` and the mapping is a design choice. Low severity.
5. **Verified claims:** "22 CUEC identifiers" is correct (22 distinct
   CUEC-CNV-* IDs); the daf-landscape NO-GO status is correct.

Edits:

- `controls/control-catalog.csv`: `unbound_refs` updated for FIN-001,
  FIN-002 and FIN-006.
- `financial/unbound-financial-controls.md`: grant-disbursement refs
  corrected.

## Pass 10 - Auditor-acceptance uncertainty

Most acceptance statements were already tied to OQ-108, OQ-312, OQ-500,
OQ-601, OQ-759 or OQ-765, or marked `auditor-interpretation`. I hedged these
unmarked statements:

- `findings/challenging-conventional-compliance.md`: three "likely auditor
  view" cells stated as fact (rows 1, 9, 11). Now marked `assumption`.
- `pentest/unbound-minimum-cost-testing-strategy.md`: "Best for bank TPRM
  acceptance". Now marked `assumption`, citing OQ-303.
- `roadmap/implementation-roadmap.md`: "no fee" for an audit-firm call. Now
  marked `assumption`.

**Remaining item, acceptable but worth watching:** the automation matrix's
L4 row says the auditor tests configuration over the period rather than
outcome samples. It is tagged `auditor-interpretation`, confidence `medium`.

## Open issues for the coordinator (ranked)

| # | Severity | Issue | Ref |
|---|---|---|---|
| 1 | High | Small-team SoD approval rules conflict (ai-gov P1 vs POL-11.03 vs CTL-GOV-009 exceptions); no eligible approver exists | OQ-800 |
| 2 | High | P3 exit criterion unattainable: 65/104 controls lack internal-independent validation; reviewer unbudgeted | OQ-803 |
| 3 | High | Admin-role concentration and key custody can defeat WORM and crypto-shredding | OQ-804 |
| 4 | High | Evidence-ledger substrate and timing contradict across decision record, architecture, PC-12, roadmap and economics | OQ-805 |
| 5 | Medium | Catalog automation targets exceed the minimum set that scenario 5 prices; HR L4/L5 at P0 with no systems | OQ-802 |
| 6 | Medium | SYS-VIGILA mapped to logging, alerting and heartbeat controls it cannot perform | OQ-806 |
| 7 | Medium | CTL-DAT-002 L5 is irreversible | OQ-801 |
| 8 | Medium | VAL-012 and VAL-022 phased in P2 for P1 controls (BCR-001, IR-003) | validation §7 |
| 9 | Low | R7 cites VAL-026 for collusion pattern sampling | resilience §2 |
| 10 | Low | UNB-SCTRL-030 and 033 unmapped; FIN-010 cites UNB-SCTRL-028 (break handling), which fits FIN-004 better | catalog |
| 11 | Low | Charter §16 and §22 text absent from this repository | OQ-807 |
| 12 | Low | `registers/fragments/AIG-validation-procedures.csv` still holds the pre-edit VAL-012 and VAL-019 text (merge adds rows only, so there is no effect) | fragments |
| 13 | Low | Duplicated auditor-acceptance questions (OQ-108, 312, 500, 601, 759, 765) with mixed resolver (`auditor` vs `cpa-firm`); consider consolidating | registers |

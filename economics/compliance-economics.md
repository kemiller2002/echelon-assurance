---
id: ASR-ECON-001
title: Compliance economics — five-year TCO and marginal cost per product
status: draft
version: 0.1.0
created: 2026-11-25
inputs: [economics/parameters.csv, economics/generated/tco-summary.md, economics/fragments/PTV-cost-evidence.csv, vendors/compliance-platform-analysis.md]
---

# Compliance economics

## 1. Read this first

- Every price is a **vendor claim** or **assumption**: vendor and
  consultancy blogs, buyer-reported medians seen through search summaries,
  and our own hour estimates. No quote has been obtained (OQ-311 and the
  economics questions in the register). Treat outputs as ranges for
  decision structure, not budgets.
- Internal labor is priced at an assumed loaded cost of USD 100–175/hour.
  Replace with Echelon's actual figure; it is the most sensitive input.
- The scope modelled is one product (Unbound) with **SOC 1 Type 2 + SOC 2
  Type 2 covering all five trust services categories** (owner decision 0D02) (per `soc/unbound-soc-applicability.md`) and an annual independent
  pen test. If no SOC 1 is needed, subtract roughly USD 29k–60k per year
  from every scenario equally.

## 2. Result (generated, undiscounted, 5 years)

From `economics/generated/tco-summary.md`:

| Scenario | Unavoidable external | Reducible | Total |
|---|---|---|---|
| 1 Conventional manual | $315k – $745k | $357k – $1,312k | $672k – $2,057k |
| 2 Commercial platform | $315k – $745k | $269k – $904k | $584k – $1,649k |
| 3 Outsourced management | $315k – $745k | $272k – $980k | $587k – $1,725k |
| 4 Echelon fully automated (feature parity build) | $395k – $970k | $287k – $1,052k | $682k – $2,022k |
| 5 Hybrid: minimal automation on existing Echelon systems + targeted external services | $380k – $940k | $205k – $777k | $585k – $1,717k |
| 5b Sensitivity: hybrid, but auditor rejects system-generated evidence | $380k – $940k | $405k – $1,477k | $785k – $2,417k |

With an 8% discount rate, the ordering is unchanged
(`assurance tco --rate 0.08`).

## 3. What the numbers say

1. **The unavoidable external costs dominate.** SOC 1 and SOC 2
   examinations plus independent penetration testing are roughly half of
   every scenario at the low end. No operating model removes them; an AI-native
   model can only shorten fieldwork and reduce findings. (`recommendation`,
   confidence `medium`.)
2. **Building feature parity is not cheaper than buying.** Scenario 4 costs
   more than a commercial platform across most of the range, because the
   build effort and the external validation the assurance machinery itself
   then needs exceed a mid-market subscription. This confirms the charter's
   warning. (`recommendation`, confidence `medium`.)
3. **The hybrid and a commercial platform cost about the same.** After
   adding the contracted independent reviewer that our own `Tested` and
   small-team rules require (owner decision DF-ASR-2026-0D04), the hybrid
   ($585k–$1.72M) and a commercial platform ($584k–$1.65M) are indistinguishable within the
   uncertainty of the inputs. The hybrid's advantage is not price: it covers
   the financial controls platforms do not, avoids subscription lock-in and
   bundled-auditor independence risk, and its independent review is a real
   assurance improvement. Platform scenarios that omit independent review
   are cheaper partly because they assure less. (`recommendation`,
   confidence `low` on cost, `medium` on the qualitative difference.)
4. **Cost is not the deciding factor; fit is.** The decisive differences are
   not in this table:
   - Commercial platforms automate configuration snapshots for common SaaS
     and cloud services. They do not cover Unbound's financial controls
     (ledger invariants, reconciliation, authorization thresholds), which are
     the core of a SOC 1. Those must be built and evidenced in Unbound under
     **every** scenario.
   - The bundled-auditor model carries independence risk (see vendor analysis).
   - The hybrid's evidence is only valuable if the auditor relies on it
     (OQ-500, OQ-601). If the first auditor declines, scenario 5 degrades
     toward scenario 1's labor profile.

**Recommendation:** pursue scenario 5, with an explicit exit: if the
readiness assessment or first auditor will not rely on system-generated
evidence with completeness proofs, buy a platform for the SaaS/cloud
evidence layer and keep the Echelon-built financial controls. Decide after
the auditor conversation (roadmap P3), not before.

### Review pass 7 (economic viability) finding

Scenario 5b shows the hybrid's downside: if the auditor will not rely on
Echelon-generated evidence, the hybrid becomes the **most expensive**
option ($785k–$2.42M), because the build cost is spent and conventional evidence labor
returns. Consequences for the roadmap:

1. Before P3, build only automation that is justified *without* auditor
   reliance: preventive controls (they reduce risk and findings regardless)
   and financial invariants (required for production anyway).
2. Defer the anchored evidence ledger, completeness reconciliation and
   auditor interface until a CPA firm has said, in scoping, how it will use
   them.
3. The model counts SOC 1 and SOC 2 examination fees from year 1. In
   practice year 1 is likely readiness and an observation period, so all
   scenarios are overstated by roughly one year of examination fees, equally.

### Owner by-construction requirements (2026-11-25)

The SBC and FIN-BC requirements are product engineering, required in every
scenario, so they do not change the comparison between scenarios. They do
raise absolute engineering effort for Unbound (not modelled here) and reduce
expected findings and remediation in all scenarios.

## 4. Marginal cost of adding another Echelon product

| Cost element | Hybrid / Echelon model | Commercial platform model | Evidence |
|---|---|---|---|
| Examination scope | Add product to existing SOC 2 system description: assumed +USD 5k–15k/yr rather than a second report; separate SOC 1 only if the product affects user-entity ICFR | Same | `assumption`; auditor-specific (OQ) |
| Penetration test | +USD 8k–25k/yr per distinct application surface; shared platform components tested once | Same | vendor-claim ranges (SRC-PTV-036) |
| Policies | ~0: policies are organization-wide; product adds scope lines | ~0 | design |
| Shared controls (IAM, SDL, LOG, VEN, GOV, ASR) | ~0 incremental: same IdP, rulesets, pipeline, evidence ledger | Usually ~0 if same integrations | design |
| Product-specific controls (e.g. FIN, TEN) | Engineering in the product itself: assumed 80–200 hours | Same work; platform does not supply it | `assumption` |
| Evidence integration | Emit envelopes from new product: assumed 40–120 hours | Integration may exist or need custom evidence upload | `assumption` |
| Subscription | none | Some vendors price per framework or per entity; growth tiers | vendor-claim |

**Economies of scale are real for the shared control layer** (80 of 104 controls, about three
quarters of the catalog: GOV, HR, IAM, CRY, NET, END, CFG, SDL, VUL, LOG, IR,
BCR, VEN, AI, ASR). Each additional product mostly incurs its own pen test,
its own product-specific controls and a scope increment in the examination.
(`recommendation`, confidence `medium`.)

## 5. Opportunity cost

Scenario 4's 800–1,600 build hours and Scenario 5's 300–700 are hours not
spent on Unbound. At the assumed rate they are already priced as labor, but
the real cost is schedule: assurance tooling should not delay the product's
own gates (`financial/unbound-financial-controls.md`). This argues for
building only what is on the critical path to the first observation period.

## 6. Sensitivities (which inputs change the decision)

| Input | Effect |
|---|---|
| Loaded labor rate | Higher rates favor buying (platform, outsourced) |
| Auditor reliance on system evidence | Rejection erases most of scenario 4/5 savings |
| Need for SOC 1 | Adds equally to all; increases value of Echelon-built FIN evidence |
| Number of products | More products favor shared Echelon controls |
| Number of frameworks (ISO 27001, etc.) | Platforms amortize multi-framework mapping; unified catalog does too |
| Platform price negotiation | Buyer-reported medians vary ~2x |

## 7. Questions to resolve with real quotes

Quotes from 2–3 CPA firms for a combined SOC 1 + SOC 2 Type 2 engagement on
the Unbound scope; one grey-box pen-test quote; one platform quote for
comparison. These are in `registers/open-questions.csv` and on the roadmap
as P0/P3 actions.

### Owner record: continuous security testing (2026-11-25)

CST-025 requires recording testing cost, runtime, coverage, finding yield and
escaped defects, so the pen-test line in `parameters.csv` can later be
replaced by measured internal cost plus actual quotes. The record states no
guaranteed savings; external testing remains an unavoidable cost line in
every scenario.

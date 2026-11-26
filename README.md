# Echelon Assurance

Echelon Foundry's authoritative repository for **organizational assurance**:
research, requirements, policy, control design, evidence architecture and
validation methodology for an AI-native compliance operating model.

> Make security and compliance measurable, continuously evidenced properties
> of the organization rather than a periodic administrative exercise.

**Status:** final reconciliation of the owner directions of 2026-11-25 (DF-ASR-2026-0D0F, DF-ASR-2026-0D10) in review; design-stage only, no production implementation (journal `JR-ASR-2026-C001`); see [`docs/decisions/owner-decision-package-2026-11-25.md`](docs/decisions/owner-decision-package-2026-11-25.md). Nothing in this repository asserts that any
Echelon system is compliant, that any control is operating effectively, or
that any audit requirement is satisfied. See
[`context/CURRENT-STATE.md`](context/CURRENT-STATE.md).

**Confidentiality:** read [`SECURITY-AND-CONFIDENTIALITY.md`](SECURITY-AND-CONFIDENTIALITY.md)
before contributing.

## Scope

Assurance, not only SOC compliance. SOC 1 and SOC 2 are the first research
priorities; the unified control model is designed so that further regulatory,
contractual, security, operational and financial requirements attach to the
same controls and evidence instead of creating parallel programs. Unbound is
the first reference implementation; the architecture is organization-wide.

## Map

| Deliverable | Location |
|---|---|
| **Consolidation deliverables (2026-11-25)** | |
| 1. Executive findings | [`findings/executive-findings.md`](findings/executive-findings.md) |
| 2. Unified requirements baseline | [`requirements/baseline.csv`](requirements/baseline.csv), [`requirements/README.md`](requirements/README.md), tenets in [`registers/tenets.csv`](registers/tenets.csv) |
| 3. Canonical control catalog | [`controls/control-catalog.csv`](controls/control-catalog.csv) |
| 4. Consolidated policy inventory | [`policies/policy-inventory.csv`](policies/policy-inventory.csv) |
| 5. Requirements traceability matrix | [`requirements/generated/traceability-matrix.md`](requirements/generated/traceability-matrix.md) (generated) |
| 6. Question register | [`registers/open-questions.csv`](registers/open-questions.csv), [`registers/question-dispositions.csv`](registers/question-dispositions.csv), [`findings/counsel-and-auditor-questions.md`](findings/counsel-and-auditor-questions.md) |
| 7. Decision register | [`registers/decision-register.csv`](registers/decision-register.csv), [`docs/decisions/`](docs/decisions/) |
| 8. Risk register | [`registers/risk-register.csv`](registers/risk-register.csv) |
| 9. Regulatory applicability | [`regulatory/regulatory-applicability-matrix.md`](regulatory/regulatory-applicability-matrix.md), [`findings/financial-integrity-review.md`](findings/financial-integrity-review.md) |
| 10. Capability verification matrix | [`integration/capability-verification.csv`](integration/capability-verification.csv), [`integration/capability-findings.md`](integration/capability-findings.md) |
| 11. Policy-as-code specification | [`policy-as-code/policy-as-code-specification.md`](policy-as-code/policy-as-code-specification.md) |
| 12. Continuous evidence architecture | [`architecture/evidence-architecture.md`](architecture/evidence-architecture.md) |
| 13. Continuous security testing specification | [`validation/continuous-security-testing-specification.md`](validation/continuous-security-testing-specification.md) |
| 14. AI governance and authority model | [`ai-governance/ai-governance-model.md`](ai-governance/ai-governance-model.md) |
| 15. Independent validation methodology | [`validation/independent-validation-architecture.md`](validation/independent-validation-architecture.md) |
| 16. Organizational operating model | [`governance/operating-model.md`](governance/operating-model.md) |
| 17. Customer assurance requirements | [`customer-assurance/`](customer-assurance/) |
| 18. Revised economic model | [`economics/v2/`](economics/v2/) (v1 preserved, superseded, in [`economics/`](economics/)) |
| 19. Prioritized implementation roadmap | [`roadmap/generated/backlog-by-phase.md`](roadmap/generated/backlog-by-phase.md), [`roadmap/implementation-roadmap.md`](roadmap/implementation-roadmap.md) |
| 20. Cross-repository backlog | [`roadmap/backlog.csv`](roadmap/backlog.csv) |
| 21. Consolidation and removal register | [`requirements/source-dispositions.csv`](requirements/source-dispositions.csv) |
| 22. Validation report | [`findings/validation-report.md`](findings/validation-report.md) |
| 23. Clarity and EDF assessment | [`findings/clarity-and-edf-assessment.md`](findings/clarity-and-edf-assessment.md) |
| 24. Owner decision package | [`docs/decisions/owner-decision-package-2026-11-25.md`](docs/decisions/owner-decision-package-2026-11-25.md) |
| 25. Production activation gates | [`registers/production-gates.csv`](registers/production-gates.csv) (PG-01..11; all `not-met`) |
| 26. Owner decision traceability | [`registers/decision-traceability.csv`](registers/decision-traceability.csv) and the generated matrix |
| 27. Final reconciliation review record | [`findings/reviews/RV-ASR-2026-11-26.md`](findings/reviews/RV-ASR-2026-11-26.md) |
| **Research program (B0–B8)** | |
| Executive findings | [`findings/executive-findings.md`](findings/executive-findings.md); challenge of conventional practice and Clarity/EDF application in [`findings/`](findings/) |
| Owner research brief | [`docs/charter/research-brief-2026-11-25.md`](docs/charter/research-brief-2026-11-25.md) |
| Owner by-construction requirements and challenge review | [`requirements/`](requirements/) |
| Research methodology, identifiers | [`methodology/`](methodology/) |
| Authoritative source register | [`sources/source-register.csv`](sources/source-register.csv) |
| SOC reporting landscape | [`soc/`](soc/) |
| Regulatory applicability matrix, framework crosswalk | [`regulatory/`](regulatory/) |
| Unified control catalog and automation matrix | [`controls/`](controls/), [`automation/`](automation/) |
| Policy inventory and draft policies | [`policies/`](policies/) |
| Policy-as-code specification | [`policy-as-code/`](policy-as-code/) |
| Evidence architecture | [`architecture/`](architecture/) |
| Independent validation architecture | [`validation/`](validation/) |
| Penetration testing research | [`pentest/`](pentest/) |
| Compliance vendor analysis | [`vendors/`](vendors/) |
| Echelon integration matrix | [`integration/`](integration/) |
| AI governance model | [`ai-governance/`](ai-governance/) |
| Unbound financial controls | [`financial/`](financial/) |
| Auditor experience specification | [`auditor/`](auditor/) |
| Customer trust center specification | [`trust-center/`](trust-center/) |
| Compliance economics | [`economics/`](economics/) |
| Risk, decision, open-question, evidence-type and validation registers | [`registers/`](registers/) |
| Implementation roadmap | [`roadmap/`](roadmap/) |
| Decision records | [`docs/decisions/`](docs/decisions/) |
| Research journals (batch log) | [`research/journals/`](research/journals/) |

## Verify

```bash
dotnet tool restore
dotnet test Echelon.Assurance.sln -c Release
dotnet run --project src/Echelon.Assurance.Cli -- validate
```

Requires the .NET 10 SDK. `assurance merge`, `assurance roadmap`, `assurance trace`,
`assurance econ` and `assurance tco` regenerate merged registers and generated views;
CI fails when generated views are stale.

## Contributing

See [`AGENTS.md`](AGENTS.md).

# Echelon Assurance

Echelon Foundry's authoritative repository for **organizational assurance**:
research, requirements, policy, control design, evidence architecture and
validation methodology for an AI-native compliance operating model.

> Make security and compliance measurable, continuously evidenced properties
> of the organization rather than a periodic administrative exercise.

**Status:** research phase complete (batches B0–B8, ten review passes); awaiting owner decisions. Nothing in this repository asserts that any
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

Requires the .NET 10 SDK. `assurance merge`, `assurance roadmap` and
`assurance tco` regenerate merged registers and generated views.

## Contributing

See [`AGENTS.md`](AGENTS.md).

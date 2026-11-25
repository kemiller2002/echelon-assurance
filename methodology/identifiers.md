# Identifier scheme

IDs are stable, case-sensitive and never reused. Gaps are valid. A retired
record keeps its ID with `status` set to `retired` or `superseded`.

| Prefix | Record | Format | Register |
|---|---|---|---|
| `SRC-` | Authoritative or secondary source | `SRC-<AREA>-<NNN>` e.g. `SRC-AICPA-001` | `sources/source-register.csv` |
| `REQ-` | Externally derived requirement | `REQ-<FRAMEWORK>-<NNN>` e.g. `REQ-GLBA-003` | `regulatory/requirements.csv` |
| `CTL-` | Unified control | `CTL-<DOMAIN>-<NNN>` e.g. `CTL-IAM-004` | `controls/control-catalog.csv` |
| `POL-` | Policy (or policy section) | `POL-<NN>` / `POL-<NN>.<NN>` | `policies/policy-inventory.csv` |
| `EVT-` | Evidence type | `EVT-<NNN>` | `registers/evidence-types.csv` |
| `VAL-` | Validation procedure | `VAL-<NNN>` | `registers/validation-procedures.csv` |
| `RSK-` | Risk | `RSK-<NNN>` | `registers/risk-register.csv` |
| `OQ-` | Open question | `OQ-<NNN>` | `registers/open-questions.csv` |
| `SYS-` | Echelon system | `SYS-<NAME>` e.g. `SYS-PRAXIS` | `integration/system-register.csv` |
| `DF-ASR-` | Decision record (Praxis) | `DF-ASR-2026-<HEX4>` | `docs/decisions/` + `registers/decision-register.csv` |
| `JR-ASR-` | Research journal (Praxis) | `JR-ASR-2026-<HEX4>` | `research/journals/` |

## Control domains

| Domain | Scope |
|---|---|
| `GOV` | Governance, risk, compliance management, policy lifecycle, oversight |
| `HR` | Personnel, onboarding/offboarding, training, conduct |
| `IAM` | Identity, authentication, authorization, privileged access, access review |
| `CRY` | Encryption, key and secrets management |
| `NET` | Network and cloud perimeter |
| `END` | Endpoints and workstations |
| `CFG` | Configuration and infrastructure-as-code |
| `SDL` | Secure development, change, release, supply chain |
| `VUL` | Vulnerability management, testing, penetration testing |
| `LOG` | Logging, monitoring, detection |
| `IR` | Incident response and notification |
| `DAT` | Data classification, retention, deletion, privacy |
| `BCR` | Backup, continuity, disaster recovery, availability |
| `VEN` | Vendor, subservice and third-party risk |
| `AI` | AI and autonomous agent governance |
| `FIN` | Financial transaction and accounting integrity |
| `TEN` | Tenant/customer isolation and configuration governance |
| `CUS` | Customer lifecycle, communication, complaints |
| `ASR` | Assurance-system integrity (controls over the evidence and validation machinery) |

Unbound's existing identifiers (`UNB-CTRL-*`, `UNB-SCTRL-*`, `SOC2-0xx`) are
preserved in Unbound and referenced from `controls/control-catalog.csv`
(`unbound_refs`), never renamed. Unbound reuses some of our prefixes with
different meanings (for example its own `VAL-001…024`), so every reference to
another repository's identifier is written with a repository prefix:
`UNB:VAL-003`, `UNB:UNB-CTRL-014`. A bare ID always means this repository.

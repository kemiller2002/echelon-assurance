/// Register schemas: the single definition of every machine-readable register.
/// Pure data; documented in registers/SCHEMA.md and methodology/identifiers.md.
module Echelon.Assurance.Registers.Schema

type RegisterSpec =
    {
        Name: string
        Path: string
        IdColumn: string
        IdPattern: string
        Columns: string list
        Enums: Map<string, Set<string>>
        Refs: Map<string, string>
    }

let evidenceClasses =
    set
        [
            "authoritative"
            "auditor-interpretation"
            "industry-convention"
            "customer-expectation"
            "vendor-claim"
            "recommendation"
            "assumption"
            "open-question"
        ]

let confidence = set [ "very-low"; "low"; "medium"; "high"; "very-high" ]

let applicability =
    set
        [
            "legal"
            "contractual"
            "framework-selected"
            "customer-expectation"
            "voluntary"
            "not-applicable"
            "undetermined"
        ]

let controlStatus =
    set
        [
            "Proposed"
            "Designed"
            "Specified"
            "Implemented"
            "Operating"
            "Tested"
            "Deficient"
            "Remediating"
            "AuditorReviewed"
            "Retired"
        ]

let automationLevels = set [ "L0"; "L1"; "L2"; "L3"; "L4"; "L5"; "unknown" ]

let failureModes =
    set [ "fail-closed"; "fail-open-justified"; "detective-only"; "n/a" ]

let costClasses = set [ "none"; "low"; "medium"; "high"; "very-high"; "unknown" ]

let policyDisposition =
    set [ "standalone"; "section"; "technical-objective"; "not-needed"; "deferred" ]

let sourceTypes =
    set
        [
            "statute"
            "regulation"
            "regulator-guidance"
            "attestation-standard"
            "primary-standard"
            "standard-catalog"
            "accreditation-body"
            "auditor-publication"
            "industry-body"
            "academic"
            "vendor"
            "news"
            "echelon-internal"
        ]

let access = set [ "public"; "paywalled"; "summary-only"; "internal" ]

/// Dispositions applied to every requirement during consolidation (mission section 5).
let sourceDispositions =
    set
        [
            "retain"
            "consolidate"
            "strengthen"
            "simplify"
            "replace"
            "defer"
            "remove"
            "escalate"
        ]

let baselineColumns =
    [
        "br_id"
        "area"
        "title"
        "statement"
        "prevents"
        "does_not_prevent"
        "tenet_ids"
        "policy_ids"
        "control_ids"
        "risk_ids"
        "source_refs"
        "basis"
        "justification"
        "applicability"
        "owner_role"
        "enforcement_mechanism"
        "enforcement_class"
        "automation_target"
        "validation_method"
        "validation_ids"
        "validation_triggers"
        "evidence_type_ids"
        "evidence_obligation"
        "dependencies"
        "echelon_systems"
        "failure_behavior"
        "residual_risk"
        "implementation_status"
        "external_dependencies"
        "incremental_cost"
        "disposition"
        "disposition_rationale"
        "roadmap_phase"
        "launch_gate"
    ]

let validationTriggers =
    set
        [
            "every-change"
            "deployment-candidate"
            "config-change"
            "vulnerability-disclosure"
            "model-change"
            "dependency-change"
            "scheduled"
            "continuous"
            "event"
        ]

let baselineEnums =
    [
        "basis",
        set
            [
                "legal"
                "conditional-legal"
                "contractual"
                "framework-selected"
                "commercial-expectation"
                "voluntary"
                "undetermined"
            ]
        "applicability",
        set
            [
                "organization"
                "all-products"
                "financial-products"
                "ai-systems"
                "unbound"
            ]
        "enforcement_class",
        set
            [
                "executable-decision"
                "configuration-constraint"
                "scheduled-obligation"
                "human-judgment"
                "external-obligation"
            ]
        "automation_target", automationLevels
        "implementation_status", controlStatus
        "incremental_cost", costClasses
        "disposition", sourceDispositions.Remove "remove"
        "roadmap_phase", set [ "P0"; "P1"; "P2"; "P3"; "P4"; "P5" ]
        "launch_gate",
        set
            [
                "before-production"
                "before-observation-period"
                "before-external-examination"
                "on-demand"
                "not-a-launch-gate"
            ]
    ]

/// Multi-valued columns whose every element must belong to a vocabulary.
let listEnums: Map<string, Map<string, Set<string>>> =
    Map.ofList [ "baseline", Map.ofList [ "validation_triggers", validationTriggers ] ]

let baselineRefs =
    [
        "tenet_ids", "tenets"
        "policy_ids", "policies"
        "control_ids", "controls"
        "risk_ids", "risks"
        "validation_ids", "validations"
        "evidence_type_ids", "evidence_types"
        "dependencies", "baseline"
        "echelon_systems", "systems"
        "external_dependencies", "open_questions"
    ]

let capabilityColumns =
    [
        "system_id"
        "name"
        "repository_head"
        "language"
        "verification_basis"
        "proposed_responsibility"
        "implemented_capabilities"
        "test_evidence"
        "specified_only"
        "missing_capabilities"
        "architectural_fit"
        "required_extensions"
        "alternative_approach"
        "security_implications"
        "incremental_effort"
        "status"
    ]

let private spec name path idColumn idPattern columns enums refs =
    {
        Name = name
        Path = path
        IdColumn = idColumn
        IdPattern = idPattern
        Columns = columns
        Enums = Map.ofList enums
        Refs = Map.ofList refs
    }

let registers: RegisterSpec list =
    [
        spec
            "sources"
            "sources/source-register.csv"
            "source_id"
            @"^SRC-[A-Z0-9]+-\d{3}$"
            [
                "source_id"
                "title"
                "publisher"
                "url"
                "publication_date"
                "retrieved"
                "jurisdiction"
                "source_type"
                "access"
                "limitations"
                "used_in"
            ]
            [ "source_type", sourceTypes; "access", access ]
            []
        spec
            "requirements"
            "regulatory/requirements.csv"
            "req_id"
            @"^REQ-[A-Z0-9]+-\d{3}$"
            [
                "req_id"
                "framework"
                "framework_ref"
                "requirement_summary"
                "source_ids"
                "applicability"
                "applicability_reasoning"
                "applies_to"
                "evidence_class"
                "confidence"
                "control_ids"
                "open_question_ids"
            ]
            [
                "applicability", applicability
                "evidence_class", evidenceClasses
                "confidence", confidence
            ]
            [
                "source_ids", "sources"
                "control_ids", "controls"
                "open_question_ids", "open_questions"
            ]
        spec
            "controls"
            "controls/control-catalog.csv"
            "control_id"
            @"^CTL-[A-Z]{2,3}-\d{3}$"
            [
                "control_id"
                "title"
                "objective"
                "risk_addressed"
                "domain"
                "frameworks"
                "requirement_ids"
                "source_ids"
                "applicability"
                "policy_ids"
                "owner_role"
                "implementation_mechanism"
                "echelon_systems"
                "automation_target"
                "automation_current"
                "validation_ids"
                "validation_frequency"
                "evidence_type_ids"
                "independence_requirement"
                "failure_mode"
                "failure_response"
                "exception_handling"
                "impl_cost"
                "recurring_cost"
                "status"
                "known_limitations"
                "unbound_refs"
                "roadmap_phase"
            ]
            [
                "applicability", applicability
                "automation_target", automationLevels
                "automation_current", automationLevels
                "failure_mode", failureModes
                "impl_cost", costClasses
                "recurring_cost", costClasses
                "status", controlStatus
                "roadmap_phase", set [ "P0"; "P1"; "P2"; "P3"; "P4"; "P5" ]
            ]
            [
                "requirement_ids", "requirements"
                "source_ids", "sources"
                "policy_ids", "policies"
                "echelon_systems", "systems"
                "validation_ids", "validations"
                "evidence_type_ids", "evidence_types"
            ]
        spec
            "policies"
            "policies/policy-inventory.csv"
            "policy_id"
            @"^POL-\d{2}(\.\d{2})?$"
            [
                "policy_id"
                "title"
                "category"
                "disposition"
                "parent_policy"
                "rationale"
                "requirement_ids"
                "control_ids"
                "owner_role"
                "approval_authority"
                "review_cycle"
                "status"
                "draft_path"
            ]
            [
                "disposition", policyDisposition
                "status", set [ "inventory"; "drafted"; "approved"; "retired" ]
            ]
            [
                "requirement_ids", "requirements"
                "control_ids", "controls"
                "parent_policy", "policies"
            ]
        spec
            "evidence_types"
            "registers/evidence-types.csv"
            "evidence_type_id"
            @"^EVT-\d{3}$"
            [
                "evidence_type_id"
                "name"
                "source_system"
                "producing_event"
                "collection_method"
                "population_completeness_method"
                "integrity_mechanism"
                "retention_class"
                "access_class"
                "control_ids"
            ]
            []
            [ "control_ids", "controls" ]
        spec
            "validations"
            "registers/validation-procedures.csv"
            "validation_id"
            @"^VAL-\d{3}$"
            [
                "validation_id"
                "name"
                "method"
                "independence_level"
                "performed_by"
                "frequency"
                "description"
                "control_ids"
            ]
            [
                "independence_level",
                set
                    [
                        "self-assessment"
                        "automated"
                        "internal-independent"
                        "external-technical"
                        "external-penetration"
                        "external-attestation"
                    ]
            ]
            [ "control_ids", "controls" ]
        spec
            "risks"
            "registers/risk-register.csv"
            "risk_id"
            @"^RSK-\d{3}$"
            [
                "risk_id"
                "title"
                "category"
                "description"
                "likelihood"
                "impact"
                "control_ids"
                "treatment"
                "owner_role"
                "status"
            ]
            [
                "likelihood", set [ "low"; "medium"; "high" ]
                "impact", set [ "low"; "medium"; "high"; "severe" ]
                "treatment", set [ "mitigate"; "accept"; "transfer"; "avoid"; "undecided" ]
            ]
            [ "control_ids", "controls" ]
        spec
            "open_questions"
            "registers/open-questions.csv"
            "oq_id"
            @"^OQ-\d{3}$"
            [
                "oq_id"
                "question"
                "why_unresolvable_internally"
                "lowest_cost_resolution"
                "resolver"
                "blocks"
                "priority"
                "status"
                "related"
            ]
            [
                "resolver",
                set
                    [
                        "owner"
                        "cpa-firm"
                        "counsel"
                        "bank-partner"
                        "auditor"
                        "qsa"
                        "tax-advisor"
                        "insurer"
                        "customer"
                        "regulator"
                        "internal-research"
                    ]
                "priority", set [ "P0"; "P1"; "P2"; "P3" ]
                "status", set [ "open"; "answered"; "withdrawn" ]
            ]
            []
        spec
            "decisions"
            "registers/decision-register.csv"
            "decision_id"
            @"^DF-ASR-2026-[0-9A-F]{4}$"
            [
                "decision_id"
                "title"
                "status"
                "decided_by"
                "date"
                "summary"
                "record_path"
            ]
            [ "status", set [ "proposed"; "accepted"; "rejected"; "superseded" ] ]
            []
        spec
            "systems"
            "integration/system-register.csv"
            "system_id"
            @"^SYS-[A-Z0-9-]+$"
            [
                "system_id"
                "name"
                "repository"
                "verified_purpose"
                "maturity_observed"
                "assurance_capabilities_verified"
                "assurance_gaps"
                "evidence_produced"
                "trust_boundary"
                "verification_basis"
            ]
            []
            []
        spec
            "tenets"
            "registers/tenets.csv"
            "tenet_id"
            @"^TNT-\d{2}$"
            [ "tenet_id"; "title"; "statement"; "constraint"; "decision_ids" ]
            []
            [ "decision_ids", "decisions" ]
        spec
            "baseline"
            "requirements/baseline.csv"
            "br_id"
            @"^BR-[A-Z]{2,3}-\d{3}$"
            baselineColumns
            baselineEnums
            baselineRefs
        spec
            "source_dispositions"
            "requirements/source-dispositions.csv"
            "source_req_id"
            @"^(SBC|FIN-BC|PEC|AIG-BC|VAL-BC|CST|ECON|GOV-BC)-\d{3}$|^CA-[A-Z]+-\d{2}$|^REC-\d{2}$|^SBC-VC-\d{2}$"
            [
                "source_req_id"
                "source_document"
                "registered_req_id"
                "disposition"
                "baseline_ids"
                "rationale"
            ]
            [ "disposition", sourceDispositions ]
            [ "registered_req_id", "requirements"; "baseline_ids", "baseline" ]
        spec
            "capabilities"
            "integration/capability-verification.csv"
            "system_id"
            @"^SYS-[A-Z0-9-]+$"
            capabilityColumns
            [
                "verification_basis", set [ "tests-executed"; "source-inspected"; "docs-only"; "not-accessible" ]
                "status",
                set
                    [
                        "verified-implemented"
                        "implemented-unverified"
                        "partially-implemented"
                        "specified-only"
                        "absent"
                        "not-applicable"
                    ]
            ]
            []
        spec
            "backlog"
            "roadmap/backlog.csv"
            "work_item_id"
            @"^WI-[A-Z0-9]+-\d{2}$"
            [
                "work_item_id"
                "phase"
                "repository"
                "title"
                "requirements_addressed"
                "dependencies"
                "deliverables"
                "acceptance_criteria"
                "validation_method"
                "effort"
                "economic_justification"
                "security_implications"
                "blockers"
                "status"
            ]
            [
                "phase", set [ "P0"; "P1"; "P2"; "P3"; "P4"; "P5" ]
                "effort", set [ "S"; "M"; "L" ]
                "status", set [ "proposed"; "authorized"; "in-progress"; "done"; "withdrawn" ]
            ]
            [
                "requirements_addressed", "baseline"
                "dependencies", "backlog"
                "blockers", "open_questions"
            ]
        spec
            "question_dispositions"
            "registers/question-dispositions.csv"
            "oq_id"
            @"^OQ-\d{3}$"
            [
                "oq_id"
                "classification"
                "blocking_tier"
                "disposition_summary"
                "lowest_cost_resolution"
                "next_action"
                "resolver"
                "evidence_refs"
                "original_status"
                "proposed_status"
            ]
            [
                "classification",
                set
                    [
                        "resolved-owner-decision"
                        "resolved-authoritative-research"
                        "resolved-implementation-evidence"
                        "proposed-decision-awaiting-approval"
                        "dependent-customer"
                        "dependent-auditor"
                        "dependent-legal"
                        "dependent-vendor-insurer"
                        "dependent-future-standard"
                        "dependent-implementation"
                        "superseded"
                    ]
                "blocking_tier",
                set
                    [
                        "requirements-finalization"
                        "implementation"
                        "production-launch"
                        "external-examination"
                        "none"
                    ]
                "proposed_status", set [ "open"; "answered"; "superseded" ]
            ]
            [ "oq_id", "open_questions" ]
        spec
            "question_bank"
            "customer-assurance/question-bank.csv"
            "question_id"
            @"^CAQ-\d{3}$"
            [
                "question_id"
                "category"
                "question"
                "asked_by"
                "source_ids"
                "applicability"
                "proposed_answer"
                "answer_basis"
                "related_policy_ids"
                "related_control_ids"
                "supporting_evidence_type_ids"
                "implementation_status"
                "evidence_freshness_rule"
                "disclosure_class"
                "approval_required"
            ]
            [
                "answer_basis",
                set
                    [
                        "generated-from-control-catalog"
                        "generated-from-policy"
                        "owner-decision"
                        "open-question"
                        "manual"
                    ]
                "implementation_status", set [ "designed"; "not-started"; "not-applicable"; "unknown"; "operating" ]
                "disclosure_class", set [ "public"; "customer-nda"; "restricted" ]
                "approval_required", set [ "none"; "owner"; "owner+counsel" ]
            ]
            [
                "source_ids", "sources"
                "related_policy_ids", "policies"
                "related_control_ids", "controls"
                "supporting_evidence_type_ids", "evidence_types"
            ]
    ]

let byName name =
    registers |> List.find (fun s -> s.Name = name)

/// Fragment locations per register (glob as directory + suffix).
let fragmentSources: (string * string * string) list =
    [
        "sources", "sources/fragments", ".csv"
        "requirements", "regulatory/fragments", ".csv"
        "open_questions", "registers/fragments", "-open-questions.csv"
        "evidence_types", "registers/fragments", "-evidence-types.csv"
        "validations", "registers/fragments", "-validation-procedures.csv"
        "risks", "registers/fragments", "-risks.csv"
        "systems", "integration/fragments", "-system-register.csv"
    ]

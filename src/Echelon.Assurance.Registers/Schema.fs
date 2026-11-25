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
                "workstream"
                "design_disposition"
                "external_dependency"
                "closure_evidence"
            ]
            [
                "workstream",
                set
                    [
                        "authoritative-sources"
                        "commercial-legal-regulatory"
                        "external-assurance"
                        "technical-configuration"
                        "governance"
                    ]
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
    ]

let byName name =
    registers |> List.find (fun s -> s.Name = name)

/// Fragment locations per register (glob as directory + suffix). Open-question
/// fragments are frozen historical inputs: the register gained curated
/// disposition columns on 2026-11-25 and is no longer merged from fragments.
let fragmentSources: (string * string * string) list =
    [
        "sources", "sources/fragments", ".csv"
        "requirements", "regulatory/fragments", ".csv"
        "evidence_types", "registers/fragments", "-evidence-types.csv"
        "validations", "registers/fragments", "-validation-procedures.csv"
        "risks", "registers/fragments", "-risks.csv"
        "systems", "integration/fragments", "-system-register.csv"
    ]

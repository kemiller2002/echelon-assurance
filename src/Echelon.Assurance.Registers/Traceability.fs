/// Requirements-hierarchy traceability: tenets → policies → controls →
/// baseline requirements → enforcement → validation → evidence. Structural
/// rules (orphans, unsupported controls, missing validation, evidence or owner,
/// one-way links, status inflation) and the generated matrix. Pure.
module Echelon.Assurance.Registers.Traceability

open Echelon.Assurance.Registers.Validation

type Record = Map<string, string>

/// Records of one register, keyed by column name; empty when absent.
let records (loaded: Loaded) name : Record list =
    match Map.tryFind name loaded |> Option.flatten with
    | Some(header, rows) ->
        rows
        |> List.filter (fun r -> List.length r = List.length header)
        |> List.map (fun r -> List.zip header r |> Map.ofList)
    | None -> []

let private field (r: Record) col =
    Map.tryFind col r |> Option.defaultValue ""

let private items (r: Record) col = splitList (field r col)

let private items' (w: Record) = items w "dependencies"

/// Columns every live baseline requirement must fill (mission section 4).
let requiredBaselineColumns =
    [
        "tenet_ids"
        "policy_ids"
        "control_ids"
        "owner_role"
        "enforcement_mechanism"
        "validation_method"
        "validation_triggers"
        "evidence_obligation"
        "failure_behavior"
        "residual_risk"
        "justification"
    ]

/// Ordering of the status ladder used to detect status inflation.
let statusRank =
    [
        "Proposed"
        "Designed"
        "Specified"
        "Implemented"
        "Operating"
        "Tested"
        "AuditorReviewed"
    ]
    |> List.mapi (fun i s -> s, i)
    |> Map.ofList

let private rank s =
    Map.tryFind s statusRank |> Option.defaultValue 0

let missingFields (baseline: Record list) =
    baseline
    |> List.filter (fun b -> field b "disposition" <> "defer")
    |> List.collect (fun b ->
        requiredBaselineColumns
        |> List.filter (fun c -> (field b c).Trim() = "")
        |> List.map (sprintf "requirements/baseline.csv: %s has empty %s" (field b "br_id")))

/// Every original requirement cited by a baseline row is dispositioned to that
/// row, and every disposition points back (bidirectional).
let dispositionLinks (baseline: Record list) (dispositions: Record list) =
    let bySource =
        dispositions
        |> List.map (fun d -> field d "source_req_id", set (items d "baseline_ids"))
        |> Map.ofList

    let cites =
        baseline
        |> List.collect (fun b -> items b "source_refs" |> List.map (fun s -> s, field b "br_id"))

    let forward =
        cites
        |> List.choose (fun (s, br) ->
            match Map.tryFind s bySource with
            | None -> Some(sprintf "requirements/baseline.csv: %s cites %s which has no disposition" br s)
            | Some brs when not (brs.Contains br) ->
                Some(sprintf "requirements/source-dispositions.csv: %s does not list %s which cites it" s br)
            | Some _ -> None)

    let citeSet = set cites

    let backward =
        dispositions
        |> List.collect (fun d ->
            let s = field d "source_req_id"

            let unlinked =
                items d "baseline_ids"
                |> List.filter (fun br -> not (citeSet.Contains(s, br)))
                |> List.map (fun br ->
                    sprintf "requirements/source-dispositions.csv: %s -> %s but %s does not cite it" s br br)

            let removal =
                match field d "disposition", items d "baseline_ids" with
                | "remove", _ when (field d "rationale").Trim() = "" ->
                    [
                        sprintf "requirements/source-dispositions.csv: %s removed without rationale" s
                    ]
                | "remove", _ -> []
                | _, [] ->
                    [
                        sprintf "requirements/source-dispositions.csv: %s has no baseline requirement" s
                    ]
                | _ -> []

            unlinked @ removal)

    forward @ backward

/// Owner-mandated requirements registered in regulatory/requirements.csv must
/// each carry a disposition.
let ownerRequirementsDispositioned (requirements: Record list) (dispositions: Record list) =
    let registered =
        dispositions |> List.map (fun d -> field d "registered_req_id") |> set

    requirements
    |> List.filter (fun r -> (field r "framework").EndsWith "(owner)")
    |> List.map (fun r -> field r "req_id")
    |> List.filter (registered.Contains >> not)
    |> List.map (sprintf "regulatory/requirements.csv: owner requirement %s has no disposition")

/// A control is supported when a baseline requirement or an external
/// requirement points at it; it must also define objective, owner, validation
/// and evidence.
let controlSupport (controls: Record list) (baseline: Record list) (requirements: Record list) =
    ignore requirements
    let referenced = baseline |> List.collect (fun b -> items b "control_ids") |> set

    controls
    |> List.filter (fun c -> field c "status" <> "Retired")
    |> List.collect (fun c ->
        let id = field c "control_id"

        let unsupported =
            if referenced.Contains id then
                []
            else
                [
                    sprintf "controls/control-catalog.csv: %s is not supported by any requirement" id
                ]

        let gaps =
            [ "objective"; "owner_role"; "validation_ids"; "evidence_type_ids" ]
            |> List.filter (fun col -> (field c col).Trim() = "")
            |> List.map (sprintf "controls/control-catalog.csv: %s has empty %s" id)

        unsupported @ gaps)

/// A baseline requirement may not claim a status above its weakest control.
let statusInflation (controls: Record list) (baseline: Record list) =
    let status =
        controls
        |> List.map (fun c -> field c "control_id", field c "status")
        |> Map.ofList

    baseline
    |> List.choose (fun b ->
        let ceiling =
            items b "control_ids"
            |> List.choose (fun id -> Map.tryFind id status)
            |> List.map rank
            |> function
                | [] -> 0
                | ranks -> List.min ranks

        if rank (field b "implementation_status") > ceiling then
            Some(
                sprintf
                    "requirements/baseline.csv: %s status %s exceeds its controls"
                    (field b "br_id")
                    (field b "implementation_status")
            )
        else
            None)

/// Every tenet is realized by at least one baseline requirement.
let tenetCoverage (tenets: Record list) (baseline: Record list) =
    let used = baseline |> List.collect (fun b -> items b "tenet_ids") |> set

    tenets
    |> List.map (fun t -> field t "tenet_id")
    |> List.filter (used.Contains >> not)
    |> List.map (sprintf "registers/tenets.csv: %s has no baseline requirement")

/// Customer answers may not claim more than the controls they cite.
let answerInflation (controls: Record list) (answers: Record list) =
    let status =
        controls
        |> List.map (fun c -> field c "control_id", field c "status")
        |> Map.ofList

    let operating = rank "Operating"

    answers
    |> List.choose (fun a ->
        let claimsOperating = field a "implementation_status" = "operating"

        let backed =
            items a "related_control_ids"
            |> List.exists (fun id -> rank (Map.tryFind id status |> Option.defaultValue "") >= operating)

        if claimsOperating && not backed then
            Some(
                sprintf
                    "customer-assurance/question-bank.csv: %s claims operating without operating controls"
                    (field a "question_id")
            )
        else
            None)

/// Every open question carries a disposition with a next action, and closed
/// questions agree with their disposition.
let questionCoverage (questions: Record list) (dispositions: Record list) =
    let byId = dispositions |> List.map (fun d -> field d "oq_id", d) |> Map.ofList

    questions
    |> List.collect (fun q ->
        let id = field q "oq_id"

        match Map.tryFind id byId with
        | None -> [ sprintf "registers/open-questions.csv: %s has no disposition" id ]
        | Some d ->
            let expected =
                match field d "proposed_status" with
                | "superseded" -> "withdrawn"
                | s -> s

            [
                if (field d "next_action").Trim() = "" then
                    sprintf "registers/question-dispositions.csv: %s has no next action" id
                if field q "status" <> expected then
                    sprintf
                        "registers/open-questions.csv: %s status %s disagrees with disposition %s"
                        id
                        (field q "status")
                        expected
            ])

/// Backlog is dependency-driven: a work item never depends on a later phase,
/// and dependencies are acyclic.
let backlogOrder (items: Record list) =
    let phase =
        items
        |> List.map (fun w -> field w "work_item_id", field w "phase")
        |> Map.ofList

    let deps =
        items |> List.map (fun w -> field w "work_item_id", items' w) |> Map.ofList

    let later =
        items
        |> List.collect (fun w ->
            items' w
            |> List.filter (fun d ->
                match Map.tryFind d phase with
                | Some p -> System.String.CompareOrdinal(p, field w "phase") > 0
                | None -> false)
            |> List.map (fun d ->
                sprintf "roadmap/backlog.csv: %s depends on later-phase %s" (field w "work_item_id") d))

    let rec reaches seen target node =
        Map.tryFind node deps
        |> Option.defaultValue []
        |> List.exists (fun d -> d = target || (not (Set.contains d seen) && reaches (Set.add d seen) target d))

    let cycles =
        items
        |> List.map (fun w -> field w "work_item_id")
        |> List.filter (fun id -> reaches Set.empty id id)
        |> List.map (sprintf "roadmap/backlog.csv: %s is in a dependency cycle")

    later @ cycles

/// A baseline requirement's policies, validation and evidence must be those of
/// its controls (policies may match a parent or child section).
let controlAlignment (controls: Record list) (baseline: Record list) =
    let byId = controls |> List.map (fun c -> field c "control_id", c) |> Map.ofList

    let parent (p: string) =
        if p.Length > 6 then p.Substring(0, 6) else p

    baseline
    |> List.collect (fun b ->
        let ctls = items b "control_ids" |> List.choose (fun id -> Map.tryFind id byId)

        let union col =
            ctls |> List.collect (fun c -> items c col) |> set

        let id = field b "br_id"

        let policyErr =
            let theirs = union "policy_ids" |> Set.map parent
            let mine = items b "policy_ids" |> List.map parent |> set

            if
                List.isEmpty ctls
                || Set.isEmpty theirs
                || not (Set.isEmpty (Set.intersect mine theirs))
            then
                []
            else
                [
                    sprintf "requirements/baseline.csv: %s policies do not match its controls' policies" id
                ]

        let subset col =
            let allowed = union col

            items b col
            |> List.filter (allowed.Contains >> not)
            |> List.map (sprintf "requirements/baseline.csv: %s %s %s is not linked to any of its controls" id col)

        policyErr @ subset "validation_ids" @ subset "evidence_type_ids")

/// Every live baseline requirement is addressed by a work item, and its phase
/// is the earliest phase of those work items.
let roadmapCoverage (baseline: Record list) (backlog: Record list) =
    let phases =
        backlog
        |> List.collect (fun w -> items w "requirements_addressed" |> List.map (fun br -> br, field w "phase"))
        |> List.groupBy fst
        |> List.map (fun (br, ps) -> br, ps |> List.map snd |> List.min)
        |> Map.ofList

    baseline
    |> List.filter (fun b -> field b "disposition" <> "defer")
    |> List.choose (fun b ->
        let id = field b "br_id"

        match Map.tryFind id phases with
        | None -> Some(sprintf "requirements/baseline.csv: %s has no work item" id)
        | Some p when p <> field b "roadmap_phase" ->
            Some(
                sprintf
                    "requirements/baseline.csv: %s phase %s differs from earliest work item phase %s"
                    id
                    (field b "roadmap_phase")
                    p
            )
        | Some _ -> None)

/// Latest phase in which a requirement with a given launch gate may complete:
/// production launch follows operational readiness (P3); external examination
/// follows external assurance readiness (P4).
let latestPhaseFor gate =
    match gate with
    | "before-production"
    | "before-observation-period" -> Some "P3"
    | "before-external-examination" -> Some "P4"
    | _ -> None

let launchGates (baseline: Record list) (backlog: Record list) =
    let latest =
        backlog
        |> List.collect (fun w -> items w "requirements_addressed" |> List.map (fun br -> br, field w "phase"))
        |> List.groupBy fst
        |> List.map (fun (br, ps) -> br, ps |> List.map snd |> List.max)
        |> Map.ofList

    baseline
    |> List.choose (fun b ->
        let id = field b "br_id"

        let completes =
            Map.tryFind id latest |> Option.defaultValue (field b "roadmap_phase")

        match latestPhaseFor (field b "launch_gate") with
        | Some limit when System.String.CompareOrdinal(completes, limit) > 0 ->
            Some(
                sprintf
                    "requirements/baseline.csv: %s is %s but its work completes in %s"
                    id
                    (field b "launch_gate")
                    completes
            )
        | _ -> None)

/// Independence levels at or above technically separated verification
/// (owner decision ODP-02, DF-ASR-2026-0D0F). Agent review, technical
/// separation, organizational independence, qualified professional review and
/// external attestation are distinct; this set only orders them for the
/// Tested rule and never makes them interchangeable.
let technicallySeparatedOrHigher =
    set
        [
            "technically-separated"
            "internal-independent"
            "qualified-professional"
            "external-technical"
            "external-penetration"
            "external-attestation"
        ]

/// A control may be Tested only when a validation procedure at technically
/// separated level or higher supports it. Multiple-agent review is approved
/// for internal development review only (owner direction DF-ASR-2026-0D0F),
/// so it never supports an operating-effectiveness status (controls/README.md).
let testedNeedsIndependence (controls: Record list) (validations: Record list) =
    let acceptable (_: Record) = technicallySeparatedOrHigher

    let level =
        validations
        |> List.map (fun v -> field v "validation_id", field v "independence_level")
        |> Map.ofList

    controls
    |> List.filter (fun c -> rank (field c "status") >= rank "Tested")
    |> List.filter (fun c ->
        items c "validation_ids"
        |> List.exists (fun v -> (acceptable c).Contains(Map.tryFind v level |> Option.defaultValue ""))
        |> not)
    |> List.map (fun c ->
        sprintf
            "controls/control-catalog.csv: %s is %s without an independent validation procedure"
            (field c "control_id")
            (field c "status"))

/// Capability status may not exceed the verification actually performed:
/// documentation-only review cannot establish any implemented state.
let capabilityBasis (capabilities: Record list) =
    let implemented =
        set [ "implemented-unverified"; "partially-implemented"; "verified-implemented" ]

    capabilities
    |> List.filter (fun c ->
        field c "verification_basis" = "docs-only"
        && implemented.Contains(field c "status"))
    |> List.map (fun c ->
        sprintf
            "integration/capability-verification.csv: %s claims %s from documentation only"
            (field c "system_id")
            (field c "status"))

/// No operating-evidence register exists yet, so no control may claim a status
/// that needs observed operation (Implemented or above).
let controlEvidence (controls: Record list) =
    controls
    |> List.filter (fun c -> rank (field c "status") >= rank "Implemented")
    |> List.map (fun c ->
        sprintf
            "controls/control-catalog.csv: %s is %s but no operating evidence is recorded"
            (field c "control_id")
            (field c "status"))

/// Capability status may not exceed the verification actually performed.
let capabilityInflation (capabilities: Record list) =
    capabilities
    |> List.filter (fun c ->
        field c "status" = "verified-implemented"
        && field c "verification_basis" <> "tests-executed")
    |> List.map (fun c ->
        sprintf
            "integration/capability-verification.csv: %s verified-implemented without executed tests"
            (field c "system_id"))

/// Terms describing mechanisms deferred by decision DF-ASR-2026-C002.
let deferredMechanismTerms =
    [
        "tamper-evident evidence ledger"
        "external anchoring"
        "witness cosign"
        "hash-chained ledger"
    ]

/// Public answers may not describe non-operating controls, and no answer may
/// describe a deferred mechanism.
let answerDisclosure (controls: Record list) (answers: Record list) =
    let status =
        controls
        |> List.map (fun c -> field c "control_id", field c "status")
        |> Map.ofList

    answers
    |> List.collect (fun a ->
        let id = field a "question_id"

        let publicErr =
            let nonOperating =
                items a "related_control_ids"
                |> List.exists (fun c -> rank (Map.tryFind c status |> Option.defaultValue "") < rank "Operating")

            if field a "disclosure_class" = "public" && nonOperating then
                [
                    sprintf "customer-assurance/question-bank.csv: %s is public but cites non-operating controls" id
                ]
            else
                []

        let text = (field a "proposed_answer").ToLowerInvariant()

        let deferredEvidence =
            items a "supporting_evidence_type_ids"
            |> List.filter (fun e -> e = "EVT-024" || e = "EVT-025")
            |> List.map (sprintf "customer-assurance/question-bank.csv: %s cites deferred evidence type %s" id)

        let deferredText =
            deferredMechanismTerms
            |> List.filter text.Contains
            |> List.map (sprintf "customer-assurance/question-bank.csv: %s describes deferred mechanism '%s'" id)

        let deferred = deferredEvidence @ deferredText

        publicErr @ deferred)

/// The twelve owner decision items and the two owner tenets reconciled under
/// DF-ASR-2026-0D0F and DF-ASR-2026-0D10.
let ownerItems = [ for n in 1..12 -> sprintf "ODP-%02d" n ] @ [ "OT-01"; "OT-02" ]

/// The decision that implements each owner item, fixed by the owner records:
/// owner-review-direction (DF-ASR-2026-0D0F) and owner-remaining-approvals
/// (DF-ASR-2026-0D10).
let ownerItemDecision =
    [ "ODP-02"; "ODP-03"; "ODP-05"; "ODP-09"; "ODP-11"; "ODP-12"; "OT-02" ]
    |> List.map (fun i -> i, "DF-ASR-2026-0D0F")
    |> Map.ofList
    |> fun m ->
        ownerItems
        |> List.map (fun i -> i, Map.tryFind i m |> Option.defaultValue "DF-ASR-2026-0D10")
        |> Map.ofList

/// Every owner item has a traceability row naming an accepted decision, and
/// each row covers every baseline requirement that the item's source
/// disposition lists (bidirectional traceability of owner decisions).
let decisionTraceabilityWith
    (decisions: Record list)
    (traces: Record list)
    (dispositions: Record list)
    (baseline: Record list)
    (backlog: Record list)
    =
    let traced = traces |> List.map (fun t -> field t "item_id") |> set

    let accepted =
        decisions
        |> List.filter (fun d -> field d "status" = "accepted")
        |> List.map (fun d -> field d "decision_id")
        |> set

    let missing =
        ownerItems
        |> List.filter (traced.Contains >> not)
        |> List.map (sprintf "registers/decision-traceability.csv: owner item %s has no traceability row")

    let notAccepted =
        traces
        |> List.filter (fun t -> not (accepted.Contains(field t "decision_id")))
        |> List.map (fun t ->
            sprintf
                "registers/decision-traceability.csv: %s cites %s, which is not an accepted decision"
                (field t "item_id")
                (field t "decision_id"))

    let bySource =
        dispositions |> List.map (fun d -> field d "source_req_id", d) |> Map.ofList

    let incomplete =
        traces
        |> List.collect (fun t ->
            match Map.tryFind (field t "item_id") bySource with
            | None ->
                [
                    sprintf "registers/decision-traceability.csv: %s is not dispositioned" (field t "item_id")
                ]
            | Some d ->
                let listed = items t "br_ids" |> set

                items d "baseline_ids"
                |> List.filter (listed.Contains >> not)
                |> List.map (fun br ->
                    sprintf "registers/decision-traceability.csv: %s omits %s" (field t "item_id") br))

    let wrongDecision =
        traces
        |> List.choose (fun t ->
            match Map.tryFind (field t "item_id") ownerItemDecision with
            | Some d when d <> field t "decision_id" ->
                Some(sprintf "registers/decision-traceability.csv: %s must cite %s" (field t "item_id") d)
            | _ -> None)

    let byBr = baseline |> List.map (fun b -> field b "br_id", b) |> Map.ofList

    let derived (t: Record) column =
        items t "br_ids"
        |> List.choose (fun br -> Map.tryFind br byBr)
        |> List.collect (fun b -> items b column)
        |> set

    let derivedWork (t: Record) =
        let brs = items t "br_ids" |> set

        backlog
        |> List.filter (fun w ->
            field w "status" <> "withdrawn"
            && items w "requirements_addressed" |> List.exists brs.Contains)
        |> List.map (fun w -> field w "work_item_id")
        |> set

    let mismatch =
        if List.isEmpty baseline then
            []
        else
            traces
            |> List.collect (fun t ->
                [
                    "control_ids", derived t "control_ids"
                    "policy_ids", derived t "policy_ids"
                    "work_item_ids", derivedWork t
                ]
                |> List.filter (fun (column, expected) -> set (items t column) <> expected)
                |> List.map (fun (column, expected) ->
                    sprintf
                        "registers/decision-traceability.csv: %s %s should be %s"
                        (field t "item_id")
                        column
                        (String.concat ";" expected)))

    missing @ notAccepted @ incomplete @ wrongDecision @ mismatch

/// Decision traceability without the derived-set comparison.
let decisionTraceability (decisions: Record list) (traces: Record list) (dispositions: Record list) =
    decisionTraceabilityWith decisions traces dispositions [] []

/// An evidence reference is a repository path or a register identifier.
let evidenceRefWellFormed (ref: string) =
    System.Text.RegularExpressions.Regex.IsMatch(
        ref,
        @"^[A-Za-z0-9_.\-]+(/[A-Za-z0-9_.\-]+)*\.(md|csv|fs|yml|yaml|json)$"
    )
    || System.Text.RegularExpressions.Regex.IsMatch(ref, @"^[A-Z][A-Z0-9]*(-[A-Z0-9]+)+$")

/// Every open question states why it is unresolved, the accountable internal
/// role and the trigger that will resolve it; answered questions cite evidence.
/// Status, classification and tier are never blank; a question that depends on
/// another party or is deferred cannot be answered; evidence references are
/// paths or identifiers.
let questionCompleteness (dispositions: Record list) =
    dispositions
    |> List.collect (fun d ->
        let id = field d "oq_id"
        let status = field d "proposed_status"
        let cls = field d "classification"

        let blanks =
            [ "proposed_status"; "classification"; "blocking_tier" ]
            |> List.filter (fun c -> (field d c).Trim() = "")
            |> List.map (sprintf "registers/question-dispositions.csv: %s has no %s" id)

        let consistent =
            match status with
            | "answered" when not (cls.StartsWith("resolved-")) ->
                [
                    sprintf "registers/question-dispositions.csv: %s is answered but classified %s" id cls
                ]
            | "open" when cls.StartsWith("resolved-") || cls = "superseded" ->
                [
                    sprintf "registers/question-dispositions.csv: %s is open but classified %s" id cls
                ]
            | _ -> []

        let badRefs =
            items d "evidence_refs"
            |> List.filter (evidenceRefWellFormed >> not)
            |> List.map (
                sprintf
                    "registers/question-dispositions.csv: %s evidence reference '%s' is not a path or identifier"
                    id
            )

        let fields =
            if status = "open" then
                [ "why_unresolved"; "responsible_role"; "resolution_trigger" ]
                |> List.filter (fun c -> (field d c).Trim() = "")
                |> List.map (sprintf "registers/question-dispositions.csv: open %s has no %s" id)
            elif status = "answered" && (field d "evidence_refs").Trim() = "" then
                [
                    sprintf "registers/question-dispositions.csv: answered %s cites no evidence" id
                ]
            else
                []

        blanks @ consistent @ badRefs @ fields)

/// Production activation gates: every gate area is present; every requirement
/// that gates production and every open production-blocking question is cited
/// by a gate; a gate is "met" only with cited evidence.
let gateAreas =
    [
        "financial-authorization"
        "regulatory"
        "tenant-isolation"
        "security-validation"
        "recovery"
        "privileged-access"
        "operational-evidence"
        "incident-response"
        "customer-commitments"
        "external-assurance"
        "residual-risk"
    ]

let productionGates (gates: Record list) (baseline: Record list) (dispositions: Record list) =
    let cited column =
        gates |> List.collect (fun g -> items g column) |> Set.ofList

    let brs = cited "br_ids"
    let questions = cited "question_ids"
    let present = gates |> List.map (fun g -> field g "area") |> Set.ofList

    (gateAreas
     |> List.filter (present.Contains >> not)
     |> List.map (sprintf "registers/production-gates.csv: no gate for area %s"))
    @ (baseline
       |> List.filter (fun b ->
           field b "launch_gate" = "before-production"
           && not (brs.Contains(field b "br_id")))
       |> List.map (fun b ->
           sprintf "registers/production-gates.csv: %s gates production but no gate cites it" (field b "br_id")))
    @ (dispositions
       |> List.filter (fun d ->
           field d "proposed_status" = "open"
           && (field d "blocking_tier" = "production-launch"
               || field d "blocking_tier" = "implementation")
           && not (questions.Contains(field d "oq_id")))
       |> List.map (fun d ->
           sprintf "registers/production-gates.csv: open production-blocking %s is cited by no gate" (field d "oq_id")))
    @ (gates
       |> List.filter (fun g -> field g "status" = "met" && (field g "evidence_refs").Trim() = "")
       |> List.map (fun g -> sprintf "registers/production-gates.csv: %s is met without evidence" (field g "gate_id")))
    @ (gates
       |> List.collect (fun g ->
           items g "evidence_refs"
           |> List.filter (evidenceRefWellFormed >> not)
           |> List.map (
               sprintf
                   "registers/production-gates.csv: %s evidence reference '%s' is not a path or identifier"
                   (field g "gate_id")
           )))
    @ (let openIds =
        dispositions
        |> List.filter (fun d -> field d "proposed_status" = "open")
        |> List.map (fun d -> field d "oq_id")
        |> set

       gates
       |> List.filter (fun g -> field g "status" = "met")
       |> List.collect (fun g ->
           items g "question_ids"
           |> List.filter openIds.Contains
           |> List.map (sprintf "registers/production-gates.csv: %s is met while %s is open" (field g "gate_id"))))

let errors (loaded: Loaded) =
    let r = records loaded
    let baseline = r "baseline"
    let controls = r "controls"

    missingFields baseline
    @ dispositionLinks baseline (r "source_dispositions")
    @ ownerRequirementsDispositioned (r "requirements") (r "source_dispositions")
    @ controlSupport controls baseline (r "requirements")
    @ statusInflation controls baseline
    @ tenetCoverage (r "tenets") baseline
    @ answerInflation controls (r "question_bank")
    @ questionCoverage (r "open_questions") (r "question_dispositions")
    @ backlogOrder (r "backlog")
    @ controlAlignment controls baseline
    @ roadmapCoverage baseline (r "backlog")
    @ capabilityInflation (r "capabilities")
    @ answerDisclosure controls (r "question_bank")
    @ launchGates baseline (r "backlog")
    @ testedNeedsIndependence controls (r "validations")
    @ decisionTraceabilityWith
        (r "decisions")
        (r "decision_traceability")
        (r "source_dispositions")
        baseline
        (r "backlog")
    @ capabilityBasis (r "capabilities")
    @ controlEvidence controls
    @ questionCompleteness (r "question_dispositions")
    @ productionGates (r "production_gates") baseline (r "question_dispositions")

let private cell (s: string) = s.Replace("|", "/").Replace(";", ", ")

/// Generated traceability matrix (forward and reverse views).
let render (loaded: Loaded) =
    let r = records loaded
    let baseline = r "baseline"
    let controls = r "controls"

    let forward =
        baseline
        |> List.map (fun b ->
            sprintf
                "| %s | %s | %s | %s | %s | %s | %s | %s | %s |"
                (field b "br_id")
                (cell (field b "title"))
                (cell (field b "tenet_ids"))
                (cell (field b "policy_ids"))
                (cell (field b "control_ids"))
                (cell (field b "validation_ids"))
                (cell (field b "evidence_type_ids"))
                (field b "implementation_status")
                (field b "disposition"))

    let byControl id =
        baseline
        |> List.filter (fun b -> items b "control_ids" |> List.contains id)
        |> List.map (fun b -> field b "br_id")

    let externalCount id =
        r "requirements"
        |> List.filter (fun q -> items q "control_ids" |> List.contains id)
        |> List.length

    let reverse =
        controls
        |> List.map (fun c ->
            let id = field c "control_id"

            sprintf
                "| %s | %s | %s | %d | %s |"
                id
                (cell (field c "title"))
                (cell (String.concat ";" (byControl id)))
                (externalCount id)
                (field c "status"))

    let problems = errors loaded

    [
        "# Requirements traceability matrix (generated)"
        ""
        "Generated by `assurance trace` from `registers/tenets.csv`, `requirements/baseline.csv`,"
        "`requirements/source-dispositions.csv`, `controls/control-catalog.csv` and `regulatory/requirements.csv`."
        "Do not edit by hand."
        ""
        sprintf
            "Baseline requirements: %d. Source items dispositioned (original owner requirements, reconciliation findings and owner decision items): %d. Controls: %d. Traceability findings: %d."
            (List.length baseline)
            (List.length (r "source_dispositions"))
            (List.length controls)
            (List.length problems)
        ""
        "## Forward: baseline requirement → tenet, policy, control, validation, evidence"
        ""
        "| Requirement | Title | Tenets | Policies | Controls | Validation | Evidence | Status | Disposition |"
        "|---|---|---|---|---|---|---|---|---|"
    ]
    @ forward
    @ [
        ""
        "## Reverse: control → baseline requirements and external requirement count"
        ""
        "| Control | Title | Baseline requirements | External requirements | Status |"
        "|---|---|---|---|---|"
    ]
    @ reverse
    @ [
        ""
        "## Owner decisions → affected requirements, controls, policies, work items, questions"
        ""
        "| Item | Decision | Requirements | Controls | Policies | Work items | Questions |"
        "|---|---|---|---|---|---|---|"
    ]
    @ (r "decision_traceability"
       |> List.map (fun t ->
           sprintf
               "| %s | %s | %s | %s | %s | %s | %s |"
               (field t "item_id")
               (field t "decision_id")
               (cell (field t "br_ids"))
               (cell (field t "control_ids"))
               (cell (field t "policy_ids"))
               (cell (field t "work_item_ids"))
               (cell (field t "question_ids"))))
    @ [
        ""
        "## Reverse: requirement → owner decisions"
        ""
        "| Requirement | Owner decisions |"
        "|---|---|"
    ]
    @ (baseline
       |> List.choose (fun b ->
           let id = field b "br_id"

           match
               r "decision_traceability"
               |> List.filter (fun t -> items t "br_ids" |> List.contains id)
               |> List.map (fun t -> sprintf "%s (%s)" (field t "item_id") (field t "decision_id"))
           with
           | [] -> None
           | ds -> Some(sprintf "| %s | %s |" id (String.concat ", " ds))))
    @ [ ""; "## Traceability findings"; "" ]
    @ (if List.isEmpty problems then
           [ "None." ]
       else
           problems |> List.map (sprintf "- %s"))
    |> String.concat "\n"
    |> fun s -> s + "\n"

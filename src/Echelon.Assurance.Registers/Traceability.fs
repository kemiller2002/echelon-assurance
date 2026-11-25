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
    let referenced =
        (baseline |> List.collect (fun b -> items b "control_ids"))
        @ (requirements |> List.collect (fun r -> items r "control_ids"))
        |> set

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
            "Baseline requirements: %d. Original owner requirements dispositioned: %d. Controls: %d. Traceability findings: %d."
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
    @ [ ""; "## Traceability findings"; "" ]
    @ (if List.isEmpty problems then
           [ "None." ]
       else
           problems |> List.map (sprintf "- %s"))
    |> String.concat "\n"
    |> fun s -> s + "\n"

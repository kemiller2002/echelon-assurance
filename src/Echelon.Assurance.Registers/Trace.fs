/// Requirement → control → policy / validation / evidence traceability matrix,
/// derived from the registers. Pure.
module Echelon.Assurance.Registers.Trace

type Record = Map<string, string>

let private ids (field: string) (r: Record) = Validation.splitList r[field]

/// One row per (requirement, control) link, carrying the control's policies,
/// validations (with independence levels) and evidence types.
let matrix (requirements: Record list) (controls: Record list) (validations: Record list) : Csv.Row list =
    let controlById = controls |> List.map (fun c -> c["control_id"], c) |> Map.ofList

    let level =
        validations
        |> List.map (fun v -> v["validation_id"], v["independence_level"])
        |> Map.ofList

    let header =
        [
            "req_id"
            "framework_ref"
            "applicability"
            "control_id"
            "control_status"
            "automation_target"
            "policy_ids"
            "validations"
            "evidence_type_ids"
        ]

    let rows =
        requirements
        |> List.collect (fun req ->
            ids "control_ids" req
            |> List.choose (fun cid -> Map.tryFind cid controlById)
            |> List.map (fun c ->
                let vals =
                    ids "validation_ids" c
                    |> List.map (fun v -> sprintf "%s(%s)" v (Map.tryFind v level |> Option.defaultValue "?"))
                    |> String.concat ";"

                [
                    req["req_id"]
                    req["framework_ref"]
                    req["applicability"]
                    c["control_id"]
                    c["status"]
                    c["automation_target"]
                    c["policy_ids"]
                    vals
                    c["evidence_type_ids"]
                ]))
        |> List.sortWith (fun a b -> System.String.CompareOrdinal(String.concat "|" a, String.concat "|" b))

    header :: rows

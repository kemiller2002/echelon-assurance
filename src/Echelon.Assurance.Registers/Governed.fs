/// Governed record: the fields whose change alters a status, applicability,
/// independence, authority, gating or legal position. A committed snapshot of
/// their digests (tests/Echelon.Assurance.Tests/governed-record.csv) makes every
/// such change visible in review: changing one without regenerating the
/// snapshot fails the tests (adversarial rounds 3–5,
/// findings/reviews/RV-ASR-2026-11-26.md). Pure.
module Echelon.Assurance.Registers.Governed

open System.Security.Cryptography
open System.Text
open Echelon.Assurance.Registers.Validation

/// Register name → governed columns.
let columns: (string * string list) list =
    [
        "sources", [ "url"; "retrieved" ]
        "requirements", [ "applicability"; "open_question_ids" ]
        "controls", [ "status"; "owner_role"; "independence_requirement"; "validation_ids" ]
        "policies", [ "status"; "approval_authority"; "owner_role" ]
        "evidence_types",
        [
            "producing_event"
            "collection_method"
            "population_completeness_method"
            "integrity_mechanism"
            "retention_class"
            "access_class"
        ]
        "validations", [ "performed_by"; "independence_level" ]
        "risks", [ "treatment"; "status"; "owner_role" ]
        "open_questions", [ "status" ]
        "decisions", [ "status"; "decided_by"; "record_path" ]
        "systems", [ "repository"; "verification_basis" ]
        "tenets", [ "decision_ids" ]
        "baseline",
        [
            "launch_gate"
            "disposition"
            "implementation_status"
            "control_ids"
            "source_refs"
            "applicability"
        ]
        "source_dispositions", [ "disposition"; "baseline_ids" ]
        "capabilities", [ "verification_basis"; "status"; "test_evidence" ]
        "backlog", [ "status"; "blocking_scope"; "dependencies" ]
        "production_gates", [ "status"; "br_ids"; "question_ids"; "evidence_refs" ]
        "decision_traceability", [ "decision_id"; "br_ids"; "question_ids" ]
        "question_dispositions", [ "proposed_status"; "classification"; "blocking_tier"; "evidence_refs" ]
        "question_bank",
        [
            "proposed_answer"
            "implementation_status"
            "disclosure_class"
            "approval_required"
        ]
    ]

let private digest (text: string) =
    SHA256.HashData(Encoding.UTF8.GetBytes text)
    |> Array.take 8
    |> Array.map (fun b -> b.ToString "x2")
    |> String.concat ""

/// Snapshot rows: register, record id, digest of its governed fields.
let snapshot (loaded: Loaded) : (string * string * string) list =
    let specs = Schema.registers |> List.map (fun s -> s.Name, s) |> Map.ofList

    columns
    |> List.collect (fun (name, cols) ->
        match Map.tryFind name specs with
        | None -> []
        | Some spec ->
            Traceability.records loaded name
            |> List.map (fun r ->
                let value c =
                    Map.tryFind c r |> Option.defaultValue ""

                let id = value spec.IdColumn

                let fields = cols |> List.map (fun c -> c + "=" + value c) |> String.concat "\u001f"

                name, id, digest fields))
    |> List.sort

/// Snapshot as CSV text.
let render (loaded: Loaded) =
    "register,id,digest\n"
    + (snapshot loaded
       |> List.map (fun (r, i, d) -> sprintf "%s,%s,%s" r i d)
       |> String.concat "\n")
    + "\n"

/// Differences between a committed snapshot and the current registers.
let differences (committed: (string * string * string) list) (loaded: Loaded) =
    let current = snapshot loaded |> set
    let previous = set committed

    let key (r, i, _) = r, i

    let changed =
        Set.union (Set.difference current previous) (Set.difference previous current)
        |> Set.map key
        |> Set.toList

    changed |> List.map (fun (r, i) -> sprintf "%s %s" r i)

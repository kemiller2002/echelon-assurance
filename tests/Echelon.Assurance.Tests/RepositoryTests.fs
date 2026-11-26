/// Acceptance tests against the repository's own registers: the committed
/// registers must validate, fragments must not drift, and traceability rules
/// adopted in the catalog must hold.
module Echelon.Assurance.Tests.RepositoryTests

open System.IO
open Xunit
open Echelon.Assurance.Registers

let private root =
    let isRoot (dir: DirectoryInfo) =
        File.Exists(Path.Combine(dir.FullName, "global.json"))
        && Directory.Exists(Path.Combine(dir.FullName, "controls"))

    let rec up (dir: DirectoryInfo | null) =
        match dir with
        | null -> failwith "repository root not found"
        | d when isRoot d -> d.FullName
        | d -> up d.Parent

    up (DirectoryInfo System.AppContext.BaseDirectory)

let private read (spec: Schema.RegisterSpec) =
    let path = Path.Combine(root, spec.Path)

    if File.Exists path then
        Some(Csv.table (File.ReadAllText path))
    else
        None

let private loaded =
    Schema.registers |> List.map (fun s -> s.Name, read s) |> Map.ofList

let private records name =
    match loaded[name] with
    | Some(header, rows) -> rows |> List.map (fun r -> List.zip header r |> Map.ofList)
    | None -> []

[<Fact>]
let ``committed registers validate`` () =
    let errors = Validation.validate loaded [] @ Traceability.errors loaded
    Assert.True(List.isEmpty errors, String.concat "\n" errors)

[<Fact>]
let ``every applicable requirement maps to a control except documented exemptions`` () =
    let exempt =
        set [ "REQ-GLBA-019"; "REQ-PENTEST-009"; "REQ-PENTEST-012"; "REQ-TPRM-008" ]

    let unmapped =
        records "requirements"
        |> List.filter (fun r -> r["applicability"] <> "not-applicable" && r["control_ids"] = "")
        |> List.map (fun r -> r["req_id"])
        |> List.filter (exempt.Contains >> not)

    Assert.True(List.isEmpty unmapped, String.concat ", " unmapped)

[<Fact>]
let ``every policy and section is implemented by at least one control`` () =
    let without =
        records "policies"
        |> List.filter (fun p -> p["control_ids"] = "")
        |> List.map (fun p -> p["policy_id"])

    Assert.True(List.isEmpty without, String.concat ", " without)

[<Fact>]
let ``no control claims a status above Designed without evidence held here`` () =
    let above =
        records "controls"
        |> List.filter (fun c -> not (List.contains c["status"] [ "Proposed"; "Designed"; "Specified" ]))
        |> List.map (fun c -> c["control_id"])

    Assert.True(List.isEmpty above, String.concat ", " above)

[<Fact>]
let ``every control has a validation able to support Tested under ODP-02`` () =
    // Apply the exact Tested rule to every control as if it were Tested.
    let asTested =
        records "controls" |> List.map (fun c -> c |> Map.add "status" "Tested")

    let weak = Traceability.testedNeedsIndependence asTested (records "validations")
    Assert.True(List.isEmpty weak, String.concat "\n" weak)

[<Fact>]
let ``fragments do not drift from registers`` () =
    let drift =
        Schema.fragmentSources
        |> List.choose (fun (name, dir, suffix) ->
            let spec = Schema.byName name
            let existing = read spec |> Option.map snd |> Option.defaultValue []
            let full = Path.Combine(root, dir)

            let fragments =
                if Directory.Exists full then
                    Directory.EnumerateFiles full
                    |> Seq.filter (fun p -> p.EndsWith suffix)
                    |> Seq.map (fun p ->
                        let h, rows = Csv.table (File.ReadAllText p)
                        Merge.fragment p h rows)
                    |> List.ofSeq
                else
                    []

            let plan = Merge.plan spec existing fragments

            if plan.Added > 0 || not (List.isEmpty plan.Problems) then
                Some spec.Path
            else
                None)

    Assert.True(List.isEmpty drift, String.concat ", " drift)

[<Fact>]
let ``every economics activity has a known evidence purpose`` () =
    let header, rows =
        Csv.table (File.ReadAllText(Path.Combine(root, "economics/v2/activities.csv")))

    let bad =
        rows
        |> List.map (fun r -> List.zip header r |> Map.ofList |> Economics.activity)
        |> List.filter (fun a -> not (List.contains a.Purpose Economics.purposes))
        |> List.map (fun a -> a.Id)

    Assert.True(List.isEmpty bad, String.concat ", " bad)

/// Applies a change to one committed register, keeping its header.
let private corrupt name (change: Map<string, string> list -> Map<string, string> list) =
    match loaded[name] with
    | Some(header, rows) ->
        let changed =
            rows
            |> List.map (fun r -> List.zip header r |> Map.ofList)
            |> change
            |> List.map (fun m -> header |> List.map (fun h -> Map.tryFind h m |> Option.defaultValue ""))

        loaded |> Map.add name (Some(header, changed))
    | None -> loaded

let private reports (fragment: string) (l: Validation.Loaded) =
    let errors = Validation.validate l [] @ Traceability.errors l

    Assert.True(
        errors |> List.exists (fun e -> e.Contains fragment),
        sprintf "expected '%s' in:\n%s" fragment (String.concat "\n" errors)
    )

[<Fact>]
let ``corrupted committed registers fail validation`` () =
    corrupt "production_gates" (List.filter (fun g -> g["gate_id"] <> "PG-03"))
    |> reports "no gate for area tenant-isolation"

    corrupt "production_gates" (List.map (Map.add "status" "met"))
    |> reports "is met without evidence"

    corrupt
        "question_dispositions"
        (List.map (fun d ->
            if d["proposed_status"] = "open" then
                d |> Map.add "resolution_trigger" ""
            else
                d))
    |> reports "has no resolution_trigger"

    corrupt "decision_traceability" (List.filter (fun t -> t["item_id"] <> "ODP-07"))
    |> reports "ODP-07"

    corrupt
        "controls"
        (List.map (Map.add "status" "Tested")
         >> List.map (Map.add "validation_ids" "VAL-041"))
    |> reports "Tested"

    corrupt
        "baseline"
        (List.map (fun b ->
            if b["br_id"] = "BR-APP-014" then
                b |> Map.add "control_ids" "CTL-NOPE-999"
            else
                b))
    |> reports "CTL-NOPE-999"

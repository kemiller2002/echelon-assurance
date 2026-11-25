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
    let errors = Validation.validate loaded []
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
let ``every control has a validation at internal-independent level or higher`` () =
    let strong =
        set
            [
                "internal-independent"
                "external-technical"
                "external-penetration"
                "external-attestation"
            ]

    let level =
        records "validations"
        |> List.map (fun v -> v["validation_id"], v["independence_level"])
        |> Map.ofList

    let weak =
        records "controls"
        |> List.filter (fun c ->
            Validation.splitList c["validation_ids"]
            |> List.exists (fun v -> strong.Contains(Map.tryFind v level |> Option.defaultValue ""))
            |> not)
        |> List.map (fun c -> c["control_id"])

    Assert.True(List.isEmpty weak, String.concat ", " weak)

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

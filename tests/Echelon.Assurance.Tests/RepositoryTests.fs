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
let ``no control claims a status above Specified without evidence held here`` () =
    let above =
        records "controls"
        |> List.filter (fun c -> not (List.contains c["status"] [ "Proposed"; "Designed"; "Specified" ]))
        |> List.map (fun c -> c["control_id"])

    Assert.True(List.isEmpty above, String.concat ", " above)

[<Fact>]
let ``every executable control has a validation able to support Tested under ODP-02`` () =
    // Apply the exact Tested rule to every L3-L5 control as if it were Tested.
    // Judgment controls (L0-L2) cannot reach Tested until a technically
    // separated or organizational validator exists; that limit is disclosed.
    let asTested =
        records "controls"
        |> List.filter (fun c -> not (List.contains c["automation_target"] [ "L0"; "L1"; "L2" ]))
        |> List.map (fun c -> c |> Map.add "status" "Tested")

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

    // Round 4 (reviewer J).
    corrupt
        "backlog"
        (List.map (fun w ->
            if w["work_item_id"] = "WI-ASR-06" then
                w |> Map.add "status" "done"
            else
                w))
    |> reports "WI-ASR-06 is done but its dependency"

    corrupt
        "question_bank"
        (List.map (fun a ->
            if a["question_id"] = "CAQ-002" then
                a
                |> Map.add
                    "proposed_answer"
                    "A contracted independent reviewer and outside counsel review all controls; SOC 2 Type II report available"
            else
                a))
    |> reports "CAQ-002 claims"

    corrupt
        "validations"
        (List.map (fun v ->
            if v["validation_id"] = "VAL-001" then
                v
                |> Map.add "independence_level" "external-attestation"
                |> Map.add "performed_by" "Claude review agent"
            else
                v))
    |> reports "VAL-001 performed by"

    corrupt
        "tenets"
        (List.map (fun t ->
            if t["tenet_id"] = "TNT-01" then
                t |> Map.add "decision_ids" "DF-ASR-2026-C005"
            else
                t))
    |> reports "TNT-01 cites DF-ASR-2026-C005"

    // Round 3 (reviewer H): emptied gate lists, closing legal questions, removing owner requirements.
    corrupt
        "production_gates"
        (List.map (fun g ->
            if g["gate_id"] = "PG-03" then
                g
                |> Map.add "work_item_ids" ""
                |> Map.add "control_ids" ""
                |> Map.add "status" "met"
            else
                g))
    |> reports "PG-03 control_ids should be"

    corrupt
        "production_gates"
        (List.map (fun g ->
            if g["gate_id"] = "PG-03" then
                g |> Map.add "evidence_refs" "otherrepo:FAKE-1"
            else
                g))
    |> reports "names no record"

    corrupt
        "question_dispositions"
        (List.map (fun d ->
            if d["oq_id"] = "OQ-216" then
                d
                |> Map.add "proposed_status" "superseded"
                |> Map.add "classification" "superseded"
                |> Map.add "evidence_refs" "README.md"
            else
                d))
    |> reports "OQ-216 is superseded but classified"

    corrupt
        "source_dispositions"
        (List.map (fun d ->
            if d["source_req_id"] = "FIN-BC-013" then
                d
                |> Map.add "disposition" "remove"
                |> Map.add "baseline_ids" ""
                |> Map.add "rationale" "Not needed."
            else
                d))
    |> reports "FIN-BC-013 is remove without an accepted decision"

    // Round 2 (reviewer G): irrelevant or invented evidence.
    corrupt
        "production_gates"
        (List.map (fun g ->
            if g["gate_id"] = "PG-08" then
                g |> Map.add "status" "met" |> Map.add "evidence_refs" "README.md"
            else
                g))
    |> reports "PG-08 is met while work item"

    corrupt
        "production_gates"
        (List.map (fun g ->
            if g["gate_id"] = "PG-08" then
                g |> Map.add "evidence_refs" "EVIDENCE-FAKE-1"
            else
                g))
    |> reports "names no record"

    corrupt
        "question_dispositions"
        (List.map (fun d ->
            if d["oq_id"] = "OQ-216" then
                d |> Map.add "proposed_status" "superseded"
            else
                d))
    |> reports "OQ-216 is superseded but classified"

    corrupt
        "question_dispositions"
        (List.map (fun d ->
            if d["oq_id"] = "OQ-616" then
                d
                |> Map.add "proposed_status" "answered"
                |> Map.add "classification" "resolved-owner-decision"
                |> Map.add "evidence_refs" "README.md"
            else
                d))
    |> reports "OQ-616 is resolved by owner decision but cites no decision record"

    corrupt
        "capabilities"
        (List.map (fun c ->
            if c["system_id"] = "SYS-VIGILA" then
                c
                |> Map.add "verification_basis" "tests-executed"
                |> Map.add "status" "verified-implemented"
                |> Map.add "test_evidence" ""
            else
                c))
    |> reports "SYS-VIGILA claims executed tests without test evidence"

    corrupt
        "capabilities"
        (List.map (fun c ->
            if c["system_id"] = "SYS-CENSUS" then
                c
                |> Map.add "verification_basis" "not-accessible"
                |> Map.add "status" "implemented-unverified"
            else
                c))
    |> reports "from no access"

    corrupt
        "baseline"
        (List.map (fun b ->
            if b["br_id"] = "BR-APP-014" then
                b |> Map.add "control_ids" "CTL-NOPE-999"
            else
                b))
    |> reports "CTL-NOPE-999"

[<Fact>]
let ``every evidence path cited by questions, gates and decisions exists`` () =
    let paths name column =
        records name
        |> List.collect (fun r -> (Map.tryFind column r |> Option.defaultValue "").Split ';' |> List.ofArray)
        |> List.map (fun s -> s.Trim())
        |> List.filter (fun s -> s.Contains "/" || s.EndsWith ".md" || s.EndsWith ".csv")

    let missing =
        paths "question_dispositions" "evidence_refs"
        @ paths "production_gates" "evidence_refs"
        @ paths "decisions" "record_path"
        |> List.distinct
        |> List.filter (fun p -> not (File.Exists(Path.Combine(root, p)) || Directory.Exists(Path.Combine(root, p))))

    Assert.True(List.isEmpty missing, String.concat "\n" missing)

[<Fact>]
let ``economics activities use known timing and condition values`` () =
    let header, rows =
        Csv.table (File.ReadAllText(Path.Combine(root, "economics/v2/activities.csv")))

    let bad =
        rows
        |> List.map (fun r -> List.zip header r |> Map.ofList)
        |> List.filter (fun r ->
            not (List.contains r["timing"] [ "annual"; "one-time" ])
            || not (List.contains r["condition"] [ "always"; "reliance"; "nonreliance" ]))
        |> List.map (fun r -> r["activity_id"])

    Assert.True(List.isEmpty bad, String.concat ", " bad)

/// Regression for 84d412a: an unquoted comma gave a row more fields than its
/// header, and only `assurance econ` failed. Every committed CSV must be
/// rectangular.
[<Fact>]
let ``every committed CSV row has as many fields as its header`` () =
    let bad =
        Directory.EnumerateFiles(root, "*.csv", SearchOption.AllDirectories)
        |> Seq.filter (fun p ->
            let rel = Path.GetRelativePath(root, p).Replace('\\', '/')
            not (rel.Contains "/bin/" || rel.Contains "/obj/" || rel.StartsWith "registries/"))
        |> Seq.collect (fun p ->
            let header, rows = Csv.table (File.ReadAllText p)

            rows
            |> List.indexed
            |> List.filter (fun (_, r) -> List.length r <> List.length header)
            |> List.map (fun (i, _) -> sprintf "%s row %d" (Path.GetRelativePath(root, p)) (i + 2)))
        |> List.ofSeq

    Assert.True(List.isEmpty bad, String.concat "\n" bad)

/// Ratchet (round 3, reviewer H): closing a question or narrowing the
/// requirement set is a governed change. Any change to these sets must edit
/// this test in the same pull request, citing the owner decision that
/// authorizes it, so it is visible in review.
[<Fact>]
let ``closed questions and removed or deferred source items match the governed record`` () =
    let closedQuestions =
        set
            [
                "OQ-304"
                "OQ-505"
                "OQ-509"
                "OQ-611"
                "OQ-705"
                "OQ-708"
                "OQ-751"
                "OQ-753"
                "OQ-759"
                "OQ-760"
                "OQ-800"
                "OQ-801"
                "OQ-802"
                "OQ-803"
                "OQ-805"
                "OQ-806"
                "OQ-807"
                "OQ-808"
                "OQ-809"
            ]

    let narrowed = set [ "ECON-017"; "REC-04"; "VAL-BC-013" ]

    let actualClosed =
        records "question_dispositions"
        |> List.filter (fun q -> q["proposed_status"] <> "open")
        |> List.map (fun q -> q["oq_id"])
        |> set

    let actualNarrowed =
        records "source_dispositions"
        |> List.filter (fun d -> d["disposition"] = "remove" || d["disposition"] = "defer")
        |> List.map (fun d -> d["source_req_id"])
        |> set

    Assert.Equal<Set<string>>(closedQuestions, actualClosed)
    Assert.Equal<Set<string>>(narrowed, actualNarrowed)

/// Ratchet (round 4, reviewer J): lowering a question's blocking tier, adding
/// an accepted decision, marking work done, changing risk treatment from
/// mitigate, or introducing a new control owner role are governed changes and
/// must edit this test in the same pull request, citing the authority.
[<Fact>]
let ``gating questions, accepted decisions, done work, risk acceptance and owner roles match the governed record`` () =
    let field name col pred =
        records name |> List.filter pred |> List.map (fun r -> r[col]) |> set

    Assert.Equal<Set<string>>(
        set
            [
                "OQ-102"
                "OQ-104"
                "OQ-105"
                "OQ-200"
                "OQ-201"
                "OQ-202"
                "OQ-203"
                "OQ-204"
                "OQ-205"
                "OQ-206"
                "OQ-208"
                "OQ-209"
                "OQ-210"
                "OQ-211"
                "OQ-212"
                "OQ-213"
                "OQ-214"
                "OQ-215"
                "OQ-216"
                "OQ-217"
                "OQ-300"
                "OQ-301"
                "OQ-302"
                "OQ-303"
                "OQ-306"
                "OQ-307"
                "OQ-311"
                "OQ-312"
                "OQ-313"
                "OQ-502"
                "OQ-503"
                "OQ-504"
                "OQ-508"
                "OQ-603"
                "OQ-606"
                "OQ-607"
                "OQ-608"
                "OQ-609"
                "OQ-610"
                "OQ-614"
                "OQ-615"
                "OQ-616"
                "OQ-700"
                "OQ-703"
                "OQ-704"
                "OQ-706"
                "OQ-709"
                "OQ-710"
                "OQ-752"
                "OQ-756"
                "OQ-757"
                "OQ-761"
                "OQ-762"
                "OQ-764"
                "OQ-804"
            ],
        field "question_dispositions" "oq_id" (fun q ->
            q["blocking_tier"] = "production-launch"
            || q["blocking_tier"] = "implementation")
    )

    Assert.Equal<Set<string>>(
        set
            [
                "DF-ASR-2026-0D01"
                "DF-ASR-2026-0D02"
                "DF-ASR-2026-0D03"
                "DF-ASR-2026-0D04"
                "DF-ASR-2026-0D05"
                "DF-ASR-2026-0D06"
                "DF-ASR-2026-0D07"
                "DF-ASR-2026-0D08"
                "DF-ASR-2026-0D09"
                "DF-ASR-2026-0D0A"
                "DF-ASR-2026-0D0B"
                "DF-ASR-2026-0D0C"
                "DF-ASR-2026-0D0D"
                "DF-ASR-2026-0D0E"
                "DF-ASR-2026-C001"
                "DF-ASR-2026-C002"
                "DF-ASR-2026-C003"
                "DF-ASR-2026-C004"
                "DF-ASR-2026-C006"
                "DF-ASR-2026-0D0F"
                "DF-ASR-2026-0D10"
            ],
        field "decisions" "decision_id" (fun d -> d["status"] = "accepted")
    )

    Assert.Equal<Set<string>>(
        set [ "WI-ASR-01"; "WI-ASR-02"; "WI-ASR-13"; "WI-ASR-14" ],
        field "backlog" "work_item_id" (fun w -> w["status"] = "done")
    )

    Assert.Empty(field "risks" "risk_id" (fun r -> r["treatment"] = "accept" || r["status"] <> "identified"))

    Assert.Equal<Set<string>>(
        set [ "Engineering lead"; "Finance/controls owner"; "Owner" ],
        field "controls" "owner_role" (fun _ -> true)
    )

[<Fact>]
let ``every decision record file exists and names its decision`` () =
    let bad =
        records "decisions"
        |> List.filter (fun d ->
            let path = Path.Combine(root, d["record_path"])
            // Owner records are the owner's own words and are not edited to add
            // identifiers; which decisions cite them is fixed by the ratchet above.
            let ownerRecord = d["record_path"].StartsWith "docs/decisions/owner-"

            not (
                File.Exists path
                && (ownerRecord || File.ReadAllText(path).Contains(d["decision_id"]))
            ))
        |> List.map (fun d -> d["decision_id"])

    Assert.True(List.isEmpty bad, String.concat ", " bad)

/// Governed record (round 5, reviewer K): every change to a governed field
/// (status, applicability, independence, authority, gating, legal position,
/// customer answer) must regenerate this snapshot with `assurance governed`,
/// so the change is visible in review as a fixture diff.
[<Fact>]
let ``governed fields match the committed governed record`` () =
    let committed =
        Csv.table (File.ReadAllText(Path.Combine(root, "tests/Echelon.Assurance.Tests/governed-record.csv")))
        |> snd
        |> List.choose (function
            | [ r; i; d ] -> Some(r, i, d)
            | _ -> None)

    Assert.NotEmpty committed

    let files =
        Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
        |> Seq.map (fun p -> Path.GetRelativePath(root, p).Replace('\\', '/'), p)
        |> Seq.filter (fst >> Governed.coversFile)
        |> Seq.map (fun (rel, p) -> rel, File.ReadAllText p)
        |> List.ofSeq

    Assert.NotEmpty files
    let diffs = Governed.differencesWith committed loaded files

    Assert.True(
        List.isEmpty diffs,
        "regenerate with `assurance governed` after review:\n"
        + String.concat "\n" diffs
    )

[<Fact>]
let ``round 5 corruptions change the governed record`` () =
    let committed = Governed.snapshot loaded

    let changes l =
        Assert.NotEmpty(Governed.differences committed l)

    corrupt
        "policies"
        (List.map (fun p ->
            if p["policy_id"] = "POL-01" then
                p |> Map.add "status" "approved"
            else
                p))
    |> changes

    corrupt
        "requirements"
        (List.map (fun r ->
            if r["req_id"] = "REQ-GLBA-001" then
                r |> Map.add "applicability" "not-applicable"
            else
                r))
    |> changes

    corrupt
        "baseline"
        (List.map (fun b ->
            if b["br_id"] = "BR-FIN-006" then
                b |> Map.add "launch_gate" "not-a-launch-gate"
            else
                b))
    |> changes

    corrupt
        "decisions"
        (List.map (fun d ->
            if d["decision_id"] = "DF-ASR-2026-C004" then
                d |> Map.add "record_path" "docs/decisions/owner-review-direction-2026-11-25.md"
            else
                d))
    |> changes

    corrupt
        "validations"
        (List.map (fun v ->
            if v["validation_id"] = "VAL-001" then
                v
                |> Map.add "performed_by" "Automated verifier"
                |> Map.add "independence_level" "qualified-professional"
            else
                v))
    |> changes

    corrupt
        "decision_traceability"
        (List.map (fun t ->
            if t["item_id"] = "ODP-02" then
                t |> Map.add "question_ids" ""
            else
                t))
    |> changes

    corrupt
        "question_bank"
        (List.map (fun a ->
            if a["question_id"] = "CAQ-002" then
                a |> Map.add "proposed_answer" "Echelon is SOC 2 compliant."
            else
                a))
    |> reports "CAQ-002 claims"

[<Fact>]
let ``round 6 corruptions of any column change the governed record`` () =
    let committed = Governed.snapshot loaded

    let changes l =
        Assert.NotEmpty(Governed.differences committed l)

    corrupt
        "tenets"
        (List.map (fun t ->
            if t["tenet_id"] = "TNT-01" then
                t |> Map.add "statement" "Agents may approve money movement"
            else
                t))
    |> changes

    corrupt
        "decisions"
        (List.map (fun d ->
            if d["decision_id"] = "DF-ASR-2026-0D0F" then
                d |> Map.add "summary" "External reviewer on retainer"
            else
                d))
    |> changes

    corrupt
        "controls"
        (List.map (fun c ->
            if c["control_id"] = "CTL-FIN-006" then
                c |> Map.add "applicability" "not-applicable"
            else
                c))
    |> changes

    corrupt
        "risks"
        (List.map (fun r ->
            if r["risk_id"] = "RSK-002" then
                r |> Map.add "likelihood" "low"
            else
                r))
    |> changes

[<Fact>]
let ``economics inputs are in the governed record`` () =
    let path = "economics/v2/activities.csv"
    let content = File.ReadAllText(Path.Combine(root, path))
    let committed = Governed.full loaded [ path, content ]
    let changed = [ path, content.Replace("prevention", "auditor-packaging") ]
    Assert.NotEmpty(Governed.differencesWith committed loaded changed)
    Assert.True(Governed.coversFile path)
    Assert.False(Governed.coversFile "economics/v2/generated/results.csv")

[<Fact>]
let ``owner decision records are in the governed record`` () =
    let path = "docs/decisions/owner-review-direction-2026-11-25.md"
    let content = File.ReadAllText(Path.Combine(root, path))
    let committed = Governed.full loaded [ path, content ]
    Assert.True(Governed.coversFile path)
    Assert.NotEmpty(Governed.differencesWith committed loaded [ path, content + "\nEngage legal counsel." ])

[<Fact>]
let ``owner requirement records, validator code and workflows are governed`` () =
    Assert.True(Governed.coversFile "requirements/continuous-security-testing.md")
    Assert.True(Governed.coversFile "src/Echelon.Assurance.Registers/Governed.fs")
    Assert.True(Governed.coversFile ".github/workflows/assurance-validation.yml")
    Assert.False(Governed.coversFile "src/Echelon.Assurance.Registers/obj/Release/net10.0/X.AssemblyInfo.fs")
    Assert.False(Governed.coversFile "requirements/generated/traceability-matrix.md")

/// Round 10 (reviewer Q): build inputs are governed, and no build hook can
/// redirect compilation outside the governed sources.
[<Fact>]
let ``build inputs are governed and compile only governed sources`` () =
    Assert.True(Governed.coversFile "src/Echelon.Assurance.Registers/Echelon.Assurance.Registers.fsproj")
    Assert.True(Governed.coversFile "Echelon.Assurance.sln")
    Assert.True(Governed.coversFile "Directory.Build.props")
    Assert.True(Governed.coversFile "Directory.Build.targets")
    Assert.True(Governed.coversFile "tests/Echelon.Assurance.Tests/Echelon.Assurance.Tests.fsproj")
    Assert.True(Governed.coversFile "docs/decisions/NEW.MD")
    Assert.True(Governed.coversFile ".github/workflows/x.yaml")

    let hooks =
        Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
        |> Seq.map (fun p -> Path.GetRelativePath(root, p).Replace('\\', '/'))
        |> Seq.filter (fun p -> not (p.Contains "/obj/" || p.Contains "/bin/" || p.StartsWith ".git/"))
        |> Seq.filter (fun p ->
            let n = p.Substring(p.LastIndexOf '/' + 1).ToLowerInvariant()
            n = "directory.build.targets" || n.EndsWith ".rsp")
        |> List.ofSeq

    Assert.True(List.isEmpty hooks, String.concat ", " hooks)

    let outside =
        Directory.EnumerateFiles(Path.Combine(root, "src"), "*.fsproj", SearchOption.AllDirectories)
        |> Seq.collect (fun proj ->
            let dir = Path.GetDirectoryName proj |> nonNull

            System.Text.RegularExpressions.Regex.Matches(File.ReadAllText proj, "Include=\"([^\"]+\\.fs)\"")
            |> Seq.map (fun m -> Path.GetFullPath(Path.Combine(dir, m.Groups[1].Value)))
            |> Seq.filter (fun full -> not (full.StartsWith(Path.GetFullPath(Path.Combine(root, "src")))))
            |> Seq.map (fun full -> proj + " -> " + full))
        |> List.ofSeq

    Assert.True(List.isEmpty outside, String.concat "\n" outside)

[<Fact>]
let ``normative Markdown is governed`` () =
    Assert.True(Governed.coversFile "controls/README.md")
    Assert.True(Governed.coversFile "governance/operating-model.md")
    Assert.True(Governed.coversFile "README.md")
    Assert.False(Governed.coversFile "roadmap/generated/backlog-by-phase.md")
    Assert.False(Governed.coversFile "registers/fragments/README.md")

[<Fact>]
let ``all current Markdown is governed except historical and generated`` () =
    Assert.True(Governed.coversFile "integration/capability-findings.md")
    Assert.True(Governed.coversFile "trust-center/trust-center-specification.md")
    Assert.True(Governed.coversFile "policy-as-code/policy-as-code-specification.md")
    Assert.False(Governed.coversFile "research/journals/JR-ASR-2026-C001.md")
    Assert.False(Governed.coversFile "findings/challenge-passes/round1.md")

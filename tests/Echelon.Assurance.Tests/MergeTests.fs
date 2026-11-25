module Echelon.Assurance.Tests.MergeTests

open Xunit
open FsCheck
open FsCheck.Xunit
open Echelon.Assurance.Registers
open Echelon.Assurance.Registers.Schema

let private decisions = byName "decisions"

let private row id title =
    [ id; title; "accepted"; "owner"; "d"; "s"; "p" ]

let private fragment path rows =
    Merge.fragment path decisions.Columns rows

[<Fact>]
let ``existing rows win over fragments and output is sorted`` () =
    let existing = [ row "DF-ASR-2026-0002" "kept" ]

    let frag =
        fragment "f.csv" [ row "DF-ASR-2026-0002" "ignored"; row "DF-ASR-2026-0001" "added" ]

    let plan = Merge.plan decisions existing [ frag ]
    Assert.Equal(1, plan.Added)
    Assert.Equal<Csv.Row list>([ row "DF-ASR-2026-0001" "added"; row "DF-ASR-2026-0002" "kept" ], plan.Merged)

[<Fact>]
let ``first fragment occurrence wins`` () =
    let a = fragment "a.csv" [ row "DF-ASR-2026-0001" "first" ]
    let b = fragment "b.csv" [ row "DF-ASR-2026-0001" "second" ]
    let plan = Merge.plan decisions [] [ a; b ]
    Assert.Equal<Csv.Row list>([ row "DF-ASR-2026-0001" "first" ], plan.Merged)

[<Fact>]
let ``wrong header is skipped and reported`` () =
    let bad = Merge.fragment "bad.csv" [ "nope" ] [ row "DF-ASR-2026-0001" "x" ]
    let plan = Merge.plan decisions [] [ bad ]
    Assert.Empty plan.Merged
    Assert.Equal<string list>([ "bad.csv: header mismatch, skipped" ], plan.Problems)

[<Fact>]
let ``rows of the wrong width are skipped and reported`` () =
    let frag = fragment "f.csv" [ [ "DF-ASR-2026-0001"; "short" ] ]
    let plan = Merge.plan decisions [] [ frag ]
    Assert.Empty plan.Merged
    Assert.Single plan.Problems |> ignore

[<Property>]
let ``merge is idempotent`` () =
    let ids = Gen.choose (0, 50) |> Gen.map (sprintf "DF-ASR-2026-%04X")
    let rows = Gen.listOf (ids |> Gen.map (fun id -> row id "t"))

    Prop.forAll (Arb.fromGen (Gen.zip rows rows)) (fun (existing, incoming) ->
        let existing = existing |> List.distinctBy List.head
        let once = Merge.merge decisions existing incoming
        Merge.merge decisions once incoming = once)

[<Property>]
let ``merge never drops an existing row`` () =
    let ids = Gen.choose (0, 50) |> Gen.map (sprintf "DF-ASR-2026-%04X")
    let rows = Gen.listOf (ids |> Gen.map (fun id -> row id "t"))

    Prop.forAll (Arb.fromGen (Gen.zip rows rows)) (fun (existing, incoming) ->
        let existing = existing |> List.distinctBy List.head
        let merged = Merge.merge decisions existing incoming
        existing |> List.forall (fun r -> List.contains r merged))

module Tco =
    open Echelon.Assurance.Registers.Tco

    let private l scenario avoidable y1 annual =
        {
            Scenario = scenario
            Avoidable = avoidable
            Y1 = y1
            Annual = annual
        }

    [<Fact>]
    let ``five-year total is year one plus four annual years undiscounted`` () =
        Assert.Equal((500.0, 900.0), fiveYear 0.0 (l "s" "no" (100.0, 100.0) (100.0, 200.0)))

    [<Fact>]
    let ``unavoidable and reducible costs are separated`` () =
        let totals =
            summarise 0.0 [ l "s" "no" (1.0, 1.0) (0.0, 0.0); l "s" "yes" (2.0, 2.0) (0.0, 0.0) ]

        let _, t = List.exactlyOne totals
        Assert.Equal((1.0, 1.0), t.Unavoidable)
        Assert.Equal((2.0, 2.0), t.Reducible)

    [<Fact>]
    let ``discounting reduces later years only`` () =
        let undiscounted = fiveYear 0.0 (l "s" "no" (100.0, 100.0) (100.0, 100.0))
        let discounted = fiveYear 0.08 (l "s" "no" (100.0, 100.0) (100.0, 100.0))
        Assert.True(fst discounted < fst undiscounted && fst discounted > 100.0)

module TraceTests =
    open Echelon.Assurance.Registers

    [<Fact>]
    let ``matrix emits one row per requirement-control link with independence levels`` () =
        let req =
            Map.ofList
                [
                    "req_id", "REQ-X-001"
                    "framework_ref", "X-1"
                    "applicability", "legal"
                    "control_ids", "CTL-GOV-001;CTL-NOPE-999"
                ]

        let ctl =
            Map.ofList
                [
                    "control_id", "CTL-GOV-001"
                    "status", "Designed"
                    "automation_target", "L4"
                    "policy_ids", "POL-01"
                    "validation_ids", "VAL-025"
                    "evidence_type_ids", "EVT-021"
                ]

        let v =
            Map.ofList [ "validation_id", "VAL-025"; "independence_level", "internal-independent" ]

        let table = Trace.matrix [ req ] [ ctl ] [ v ]
        Assert.Equal(2, List.length table)
        Assert.Contains("VAL-025(internal-independent)", List.item 1 table)

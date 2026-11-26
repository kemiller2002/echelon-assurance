module Echelon.Assurance.Tests.TraceabilityTests

open Xunit
open Echelon.Assurance.Registers.Traceability

let private rec' (pairs: (string * string) list) = Map.ofList pairs

let private br id sources controls status =
    rec' (
        [
            "br_id", id
            "source_refs", sources
            "control_ids", controls
            "implementation_status", status
            "disposition", "retain"
            "tenet_ids", "TNT-01"
        ]
        @ (requiredBaselineColumns
           |> List.filter (fun c -> c <> "tenet_ids" && c <> "control_ids")
           |> List.map (fun c -> c, "x"))
    )

let private disp source brs disposition =
    rec'
        [
            "source_req_id", source
            "baseline_ids", brs
            "disposition", disposition
            "rationale", ""
        ]

[<Fact>]
let ``complete baseline row has no missing fields`` () =
    Assert.Empty(missingFields [ br "BR-APP-001" "" "CTL-APP-001" "Designed" ])

[<Fact>]
let ``empty owner or validation is reported`` () =
    let b =
        (br "BR-APP-001" "" "CTL-APP-001" "Designed").Add("owner_role", "").Add("validation_method", " ")

    Assert.Equal(2, List.length (missingFields [ b ]))

[<Fact>]
let ``deferred requirements may be incomplete`` () =
    let b =
        (br "BR-APP-001" "" "CTL-APP-001" "Designed").Add("owner_role", "").Add("disposition", "defer")

    Assert.Empty(missingFields [ b ])

[<Fact>]
let ``bidirectional disposition links pass when consistent`` () =
    let baseline = [ br "BR-APP-001" "SBC-001;SBC-005" "" "Designed" ]

    let ds =
        [
            disp "SBC-001" "BR-APP-001" "consolidate"
            disp "SBC-005" "BR-APP-001" "consolidate"
        ]

    Assert.Empty(dispositionLinks baseline ds)

[<Fact>]
let ``citation without disposition and one-way disposition are both reported`` () =
    let baseline =
        [ br "BR-APP-001" "SBC-001" "" "Designed"; br "BR-APP-002" "" "" "Designed" ]

    let ds = [ disp "SBC-009" "BR-APP-002" "retain" ]
    let errors = dispositionLinks baseline ds
    Assert.Equal(2, List.length errors)

[<Fact>]
let ``removal requires rationale and needs no baseline row`` () =
    let ok = (disp "CST-099" "" "remove").Add("rationale", "duplicate of SBC-001")
    Assert.Empty(dispositionLinks [] [ ok ])
    Assert.Single(dispositionLinks [] [ disp "CST-098" "" "remove" ]) |> ignore
    Assert.Single(dispositionLinks [] [ disp "CST-097" "" "retain" ]) |> ignore

[<Fact>]
let ``unsupported control is reported`` () =
    let ctl id =
        rec'
            [
                "control_id", id
                "objective", "o"
                "owner_role", "Owner"
                "validation_ids", "VAL-001"
                "evidence_type_ids", "EVT-001"
                "status", "Designed"
            ]

    let errors =
        controlSupport [ ctl "CTL-APP-001"; ctl "CTL-APP-002" ] [ br "BR-APP-001" "" "CTL-APP-001" "Designed" ] []

    Assert.Equal<string list>(
        [
            "controls/control-catalog.csv: CTL-APP-002 is not supported by any requirement"
        ],
        errors
    )

[<Fact>]
let ``baseline status above its controls is inflation`` () =
    let ctl = rec' [ "control_id", "CTL-APP-001"; "status", "Designed" ]

    Assert.Single(statusInflation [ ctl ] [ br "BR-APP-001" "" "CTL-APP-001" "Operating" ])
    |> ignore

    Assert.Empty(statusInflation [ ctl ] [ br "BR-APP-001" "" "CTL-APP-001" "Designed" ])

[<Fact>]
let ``customer answer cannot claim operating over designed controls`` () =
    let ctl = rec' [ "control_id", "CTL-APP-001"; "status", "Designed" ]

    let answer =
        rec'
            [
                "question_id", "CAQ-001"
                "implementation_status", "operating"
                "related_control_ids", "CTL-APP-001"
            ]

    Assert.Single(answerInflation [ ctl ] [ answer ]) |> ignore

[<Fact>]
let ``tenet without baseline requirement is reported`` () =
    let tenets = [ rec' [ "tenet_id", "TNT-01" ]; rec' [ "tenet_id", "TNT-02" ] ]

    Assert.Single(tenetCoverage tenets [ br "BR-APP-001" "" "" "Designed" ])
    |> ignore

[<Fact>]
let ``question status must agree with its disposition`` () =
    let q = rec' [ "oq_id", "OQ-100"; "status", "open" ]

    let d =
        rec' [ "oq_id", "OQ-100"; "proposed_status", "answered"; "next_action", "x" ]

    Assert.Single(questionCoverage [ q ] [ d ]) |> ignore
    Assert.Single(questionCoverage [ q ] []) |> ignore

[<Fact>]
let ``backlog dependency on a later phase or in a cycle is reported`` () =
    let wi id phase deps =
        rec' [ "work_item_id", id; "phase", phase; "dependencies", deps ]

    Assert.Empty(backlogOrder [ wi "WI-A-01" "P1" ""; wi "WI-A-02" "P2" "WI-A-01" ])

    Assert.Single(backlogOrder [ wi "WI-A-01" "P1" "WI-A-02"; wi "WI-A-02" "P2" "" ])
    |> ignore

    Assert.Equal(2, List.length (backlogOrder [ wi "WI-A-01" "P1" "WI-A-02"; wi "WI-A-02" "P1" "WI-A-01" ]))

[<Fact>]
let ``baseline validation and evidence must belong to its controls`` () =
    let ctl =
        rec'
            [
                "control_id", "CTL-APP-001"
                "policy_ids", "POL-06.01"
                "validation_ids", "VAL-001"
                "evidence_type_ids", "EVT-003"
            ]

    let good =
        rec'
            [
                "br_id", "BR-APP-001"
                "control_ids", "CTL-APP-001"
                "policy_ids", "POL-06"
                "validation_ids", "VAL-001"
                "evidence_type_ids", "EVT-003"
            ]

    Assert.Empty(controlAlignment [ ctl ] [ good ])
    let bad = good.Add("validation_ids", "VAL-002").Add("policy_ids", "POL-12.02")
    Assert.Equal(2, List.length (controlAlignment [ ctl ] [ bad ]))

[<Fact>]
let ``baseline requirement needs a work item at its own phase`` () =
    let b =
        rec' [ "br_id", "BR-APP-001"; "roadmap_phase", "P1"; "disposition", "retain" ]

    let w phase =
        rec'
            [
                "work_item_id", "WI-A-01"
                "phase", phase
                "requirements_addressed", "BR-APP-001"
            ]

    Assert.Empty(roadmapCoverage [ b ] [ w "P1" ])
    Assert.Single(roadmapCoverage [ b ] [ w "P2" ]) |> ignore
    Assert.Single(roadmapCoverage [ b ] []) |> ignore
    Assert.Empty(roadmapCoverage [ b.Add("disposition", "defer") ] [])

[<Fact>]
let ``verified capability requires executed tests`` () =
    let c basis =
        rec'
            [
                "system_id", "SYS-X"
                "status", "verified-implemented"
                "verification_basis", basis
            ]

    Assert.Empty(capabilityInflation [ c "tests-executed" ])
    Assert.Single(capabilityInflation [ c "source-inspected" ]) |> ignore

[<Fact>]
let ``public answers cannot cite designed controls or deferred mechanisms`` () =
    let ctl = rec' [ "control_id", "CTL-ASR-002"; "status", "Designed" ]

    let a cls text =
        rec'
            [
                "question_id", "CAQ-001"
                "disclosure_class", cls
                "related_control_ids", "CTL-ASR-002"
                "proposed_answer", text
            ]

    Assert.Single(answerDisclosure [ ctl ] [ a "public" "Designed." ]) |> ignore

    Assert.Single(answerDisclosure [ ctl ] [ a "customer-nda" "Uses external anchoring." ])
    |> ignore

    Assert.Empty(answerDisclosure [ ctl ] [ a "customer-nda" "Designed, not yet operating." ])

[<Fact>]
let ``before-production requirement whose work completes after operational readiness is reported`` () =
    let b gate =
        rec' [ "br_id", "BR-APP-001"; "launch_gate", gate; "roadmap_phase", "P1" ]

    let w phase =
        rec'
            [
                "work_item_id", "WI-A-0" + phase
                "phase", phase
                "requirements_addressed", "BR-APP-001"
            ]

    Assert.Empty(launchGates [ b "before-production" ] [ w "P1"; w "P3" ])

    Assert.Single(launchGates [ b "before-production" ] [ w "P1"; w "P4" ])
    |> ignore

    Assert.Empty(launchGates [ b "before-external-examination" ] [ w "P4" ])
    Assert.Empty(launchGates [ b "on-demand" ] [ w "P5" ])

[<Fact>]
let ``tested control needs an independent validation procedure`` () =
    let v id level =
        rec' [ "validation_id", id; "independence_level", level ]

    let c vals =
        rec' [ "control_id", "CTL-APP-001"; "status", "Tested"; "validation_ids", vals ]

    let vs = [ v "VAL-001" "automated"; v "VAL-025" "internal-independent" ]
    Assert.Single(testedNeedsIndependence [ c "VAL-001" ] vs) |> ignore
    Assert.Empty(testedNeedsIndependence [ c "VAL-001;VAL-025" ] vs)

[<Fact>]
let ``agent review supports Tested only for judgment controls`` () =
    let v id level =
        rec' [ "validation_id", id; "independence_level", level ]

    let c target =
        rec'
            [
                "control_id", "CTL-GOV-001"
                "status", "Tested"
                "validation_ids", "VAL-025"
                "automation_target", target
            ]

    let vs = [ v "VAL-025" "agent-review" ]
    Assert.Empty(testedNeedsIndependence [ c "L2" ] vs)
    Assert.Single(testedNeedsIndependence [ c "L4" ] vs) |> ignore

[<Fact>]
let ``technically separated verification supports Tested for executable controls`` () =
    let v id level =
        rec' [ "validation_id", id; "independence_level", level ]

    let c =
        rec'
            [
                "control_id", "CTL-APP-001"
                "status", "Tested"
                "validation_ids", "VAL-040"
                "automation_target", "L4"
            ]

    Assert.Empty(testedNeedsIndependence [ c ] [ v "VAL-040" "technically-separated" ])

[<Fact>]
let ``owner items without traceability rows are reported`` () =
    let d = rec' [ "decision_id", "DF-ASR-2026-0D0F"; "status", "accepted" ]
    Assert.Equal(14, List.length (decisionTraceability [ d ] [] []))

[<Fact>]
let ``traceability row must cite an accepted decision and cover its requirements`` () =
    let d = rec' [ "decision_id", "DF-ASR-2026-0D0F"; "status", "accepted" ]

    let disp =
        rec' [ "source_req_id", "ODP-02"; "baseline_ids", "BR-GOV-004;BR-VAL-001" ]

    let row ids decision =
        rec' [ "item_id", "ODP-02"; "decision_id", decision; "br_ids", ids ]

    let errs rows =
        decisionTraceability [ d ] rows [ disp ]
        |> List.filter (fun (e: string) -> e.Contains "ODP-02")

    Assert.Empty(errs [ row "BR-GOV-004;BR-VAL-001" "DF-ASR-2026-0D0F" ])
    Assert.Single(errs [ row "BR-GOV-004" "DF-ASR-2026-0D0F" ]) |> ignore
    Assert.Single(errs [ row "BR-GOV-004;BR-VAL-001" "DF-ASR-2026-C005" ]) |> ignore

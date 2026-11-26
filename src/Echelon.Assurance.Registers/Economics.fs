/// Activity-based assurance economics model (version 2). Pure.
/// Inputs: economics/v2/activities.csv, alternatives.csv, scenarios.csv.
/// Every cost is a low/high range; nothing is collapsed to a point estimate
/// except the explicitly labelled midpoint used for break-even search.
module Echelon.Assurance.Registers.Economics

open System.Globalization

let years = 5
let productGrid = [ 1; 2; 5; 10; 15 ]
let breakEvenSearchLimit = 30

type Range = float * float

type Activity =
    {
        Id: string
        Alternative: string
        Component: string
        CostClass: string
        PerProduct: bool
        Annual: bool
        StartYear: int
        Hours: Range
        Cash: Range
        Scaling: float
        Condition: string
        Purpose: string
    }

type Scenario =
    {
        Id: string
        Description: string
        MaintenanceMultiplier: float
        /// Share of system-generated evidence the examiner relies on: 0 none, 1 full.
        Reliance: float
        AdditionalProductStartYear: int
        DiscountRate: float
        OpportunityMultiplier: float
        LaborRate: Range
        ExcludedClasses: Set<string>
        ExcludedPurposes: Set<string>
        /// Multiplier on customer-packaging lines (customer-specific evidence requests).
        CustomerMultiplier: float
    }

/// Evidence-purpose classes (ODP-09: owner brief of 2026-11-26, with DF-ASR-2026-0D0F).
let purposes =
    [
        "baseline-engineering"
        "operational-visibility"
        "financial-integrity"
        "prevention"
        "remediation"
        "continuous-validation"
        "operational-evidence"
        "shared-infrastructure"
        "auditor-packaging"
        "customer-packaging"
        "external-services"
    ]

type Result =
    {
        Scenario: string
        Alternative: string
        Products: int
        Incremental: Range
        Baseline: Range
        Opportunity: Range
    }

let private number (s: string) =
    if s.Trim() = "" then
        0.0
    else
        System.Double.Parse(s, CultureInfo.InvariantCulture)

let activity (r: Map<string, string>) =
    {
        Id = r["activity_id"]
        Alternative = r["alternative_id"]
        Component = r["component"]
        CostClass = r["cost_class"]
        PerProduct = r["scope"] = "per-product"
        Annual = r["timing"] = "annual"
        StartYear = int r["start_year"]
        Hours = number r["hours_low"], number r["hours_high"]
        Cash = number r["cash_low"], number r["cash_high"]
        Scaling = number r["product_scaling"]
        Condition = r["condition"]
        Purpose = Map.tryFind "purpose" r |> Option.defaultValue ""
    }

let private idSet (v: string) =
    v.Split ';'
    |> Array.map (fun x -> x.Trim())
    |> Array.filter ((<>) "")
    |> Set.ofArray

let reliance (v: string) =
    match v.Trim() with
    | "yes"
    | "full" -> 1.0
    | "no"
    | "none" -> 0.0
    | "partial" -> 0.5
    | other -> number other

let scenario (r: Map<string, string>) =
    {
        Id = r["scenario_id"]
        Description = r["description"]
        MaintenanceMultiplier = number r["maintenance_multiplier"]
        Reliance = reliance r["auditor_reliance"]
        AdditionalProductStartYear = int r["additional_product_start_year"]
        DiscountRate = number r["discount_rate"]
        OpportunityMultiplier = number r["opportunity_multiplier"]
        LaborRate = number r["labor_rate_low"], number r["labor_rate_high"]
        ExcludedClasses = Map.tryFind "excluded_cost_classes" r |> Option.defaultValue "" |> idSet
        ExcludedPurposes = Map.tryFind "excluded_purposes" r |> Option.defaultValue "" |> idSet
        CustomerMultiplier =
            Map.tryFind "customer_multiplier" r
            |> Option.filter (fun v -> v.Trim() <> "")
            |> Option.map number
            |> Option.defaultValue 1.0
    }

let private add (a: Range) (b: Range) = fst a + fst b, snd a + snd b
let private scale k (r: Range) = k * fst r, k * snd r
let private zero: Range = 0.0, 0.0
let private sum (rs: Range list) = List.fold add zero rs

let isMaintenance (a: Activity) =
    a.CostClass = "shared-maintenance" || a.CostClass = "product-maintenance"

let isBuild (a: Activity) =
    a.CostClass = "shared-build" || a.CostClass = "product-build"

let isBaseline (a: Activity) = a.Alternative = "BASELINE"

/// Share of an activity that applies in a scenario: 0 when its cost class or
/// purpose is excluded; reliance-conditional lines scale with the reliance share.
let weight (s: Scenario) (a: Activity) =
    if s.ExcludedClasses.Contains a.CostClass || s.ExcludedPurposes.Contains a.Purpose then
        0.0
    else
        let customer =
            if a.Purpose = "customer-packaging" then
                s.CustomerMultiplier
            else
                1.0

        customer
        * (match a.Condition with
           | "nonreliance" -> 1.0 - s.Reliance
           | "reliance" -> s.Reliance
           | _ -> 1.0)

let applies (s: Scenario) (a: Activity) = weight s a > 0.0

/// Cost of one unit (one product's share, one year) before discounting.
let unitCost (s: Scenario) (a: Activity) : Range =
    let labor = fst a.Hours * fst s.LaborRate, snd a.Hours * snd s.LaborRate
    let raw = add labor a.Cash

    if isMaintenance a then
        scale s.MaintenanceMultiplier raw
    else
        raw

let private active (a: Activity) start year =
    if a.Annual then year >= start else year = start

let discountFactor rate year =
    1.0 / ((1.0 + rate) ** float (year - 1))

/// Product-count multiplier for a year: the first product, plus additional
/// products' scaled share once they have adopted.
let productMultiplier (s: Scenario) (a: Activity) products year =
    if not a.PerProduct then
        (if active a a.StartYear year then 1.0 else 0.0)
    else
        let first = if active a a.StartYear year then 1.0 else 0.0
        let laterStart = max a.StartYear s.AdditionalProductStartYear

        let others =
            if products > 1 && active a laterStart year then
                float (products - 1) * a.Scaling
            else
                0.0

        first + others

/// Lifecycle cost of an activity over the model horizon.
let lifecycle (s: Scenario) (products: int) (a: Activity) : Range =
    if not (applies s a) then
        zero
    else
        [ 1..years ]
        |> List.map (fun y -> scale (productMultiplier s a products y * discountFactor s.DiscountRate y) (unitCost s a))
        |> sum
        |> scale (weight s a)

/// Opportunity cost: build hours diverted from product work, valued at the
/// labor rate times the scenario opportunity multiplier. Reported separately.
let opportunity (s: Scenario) (products: int) (a: Activity) : Range =
    if not (isBuild a) || not (applies s a) then
        zero
    else
        let hoursOnly = { a with Cash = zero }
        lifecycle s products hoursOnly |> scale s.OpportunityMultiplier

let activitiesFor (alternative: string) (all: Activity list) =
    all
    |> List.filter (fun a -> a.Alternative = alternative || a.Alternative = "ALL")

let evaluate (all: Activity list) (s: Scenario) (alternative: string) (products: int) =
    let own = activitiesFor alternative all
    let baseline = all |> List.filter isBaseline

    {
        Scenario = s.Id
        Alternative = alternative
        Products = products
        Incremental = own |> List.map (lifecycle s products) |> sum
        Baseline = baseline |> List.map (lifecycle s products) |> sum
        Opportunity = own |> List.map (opportunity s products) |> sum
    }

let midpoint (r: Range) = (fst r + snd r) / 2.0

/// Marginal lifecycle cost of the n-th product.
let marginal all s alternative products : Range =
    if products <= 1 then
        (evaluate all s alternative 1).Incremental
    else
        let now = (evaluate all s alternative products).Incremental
        let before = (evaluate all s alternative (products - 1)).Incremental
        fst now - fst before, snd now - snd before

/// Smallest product count at which `candidate` costs no more than `reference`
/// at the midpoint, searching 1..breakEvenSearchLimit.
let breakEven all s candidate reference =
    [ 1..breakEvenSearchLimit ]
    |> List.tryFind (fun n ->
        midpoint (evaluate all s candidate n).Incremental
        <= midpoint (evaluate all s reference n).Incremental)

let private money (v: float) =
    "$"
    + (System.Math.Round(v / 1000.0, System.MidpointRounding.ToEven)).ToString("#,0", CultureInfo.InvariantCulture)
    + "k"

let private range (lo, hi) = sprintf "%s–%s" (money lo) (money hi)

let results (all: Activity list) (scenarios: Scenario list) (alternatives: string list) =
    [
        for s in scenarios do
            for a in alternatives do
                for n in productGrid do
                    evaluate all s a n
    ]

let renderCsv all (scenarios: Scenario list) (alternatives: string list) : Csv.Row list =
    let r (v: float) =
        string (int64 (System.Math.Round(v, System.MidpointRounding.ToEven)))

    let header =
        [
            "scenario"
            "alternative"
            "products"
            "incremental_low"
            "incremental_high"
            "marginal_low"
            "marginal_high"
            "per_product_mid"
            "opportunity_low"
            "opportunity_high"
            "baseline_low"
            "baseline_high"
        ]

    let byId = scenarios |> List.map (fun s -> s.Id, s) |> Map.ofList

    header
    :: (results all scenarios alternatives
        |> List.map (fun x ->
            let m = marginal all byId[x.Scenario] x.Alternative x.Products

            [
                x.Scenario
                x.Alternative
                string x.Products
                r (fst x.Incremental)
                r (snd x.Incremental)
                r (fst m)
                r (snd m)
                r (midpoint x.Incremental / float x.Products)
                r (fst x.Opportunity)
                r (snd x.Opportunity)
                r (fst x.Baseline)
                r (snd x.Baseline)
            ]))

/// Break-even pairs reported: each automated alternative against each
/// conventional alternative.
let breakEvenPairs =
    [
        for c in
            [
                "A4-mvp-echelon"
                "A5-targeted-automation"
                "A6-hybrid"
                "A7-full-internal-platform"
            ] do
            for r in
                [
                    "A1-conventional-manual"
                    "A2-commercial-platform"
                    "A3-outsourced-management"
                ] do
                c, r
    ]

/// Five-year cost of one alternative attributable to one purpose class.
let byPurpose (all: Activity list) (s: Scenario) (alternative: string) (products: int) (purpose: string) =
    activitiesFor alternative all @ (all |> List.filter isBaseline)
    |> List.filter (fun a -> a.Purpose = purpose)
    |> List.map (lifecycle s products)
    |> sum

/// Scenarios and alternatives broken down by purpose (ODP-09).
let purposeScenarios =
    [
        "S8-owner-direction"
        "S9-no-exam"
        "S10-partial-reliance"
        "S11-full-reliance"
        "S12-customer-specific"
    ]

let purposeAlternatives =
    [ "A4-mvp-echelon"; "A8-mvp-plus-prevention"; "A5-targeted-automation" ]

let private purposeSection all (scenarios: Scenario list) (alternatives: string list) =
    let header =
        "| Purpose | "
        + (productGrid |> List.map (sprintf "N=%d") |> String.concat " | ")
        + " |"

    let rule =
        "|---|" + (productGrid |> List.map (fun _ -> "---") |> String.concat "|") + "|"

    let one (s: Scenario) (alt: string) =
        [ ""; sprintf "### %s, %s" s.Id alt; ""; header; rule ]
        @ (purposes
           |> List.map (fun p ->
               "| "
               + p
               + " | "
               + (productGrid
                  |> List.map (fun n -> money (midpoint (byPurpose all s alt n p)))
                  |> String.concat " | ")
               + " |"))

    let chosen = scenarios |> List.filter (fun s -> List.contains s.Id purposeScenarios)

    let alts =
        purposeAlternatives |> List.filter (fun a -> List.contains a alternatives)

    if List.isEmpty chosen then
        []
    else
        [
            ""
            "## Five-year cost by evidence purpose (midpoint, includes baseline engineering)"
            ""
            "Purpose classes per the owner brief of 2026-11-26 (ODP-09, with DF-ASR-2026-0D0F). Assignment of each activity to a purpose is a hypothetical classification recorded in `activities.csv`."
        ]
        @ (chosen |> List.collect (fun s -> alts |> List.collect (one s)))

let renderMarkdown all (scenarios: Scenario list) (alternatives: string list) =
    let gridHeader =
        "| Alternative | "
        + (productGrid |> List.map (sprintf "N=%d") |> String.concat " | ")
        + " |"

    let gridRule =
        "|---|" + (productGrid |> List.map (fun _ -> "---") |> String.concat "|") + "|"

    let table (cell: string -> int -> string) =
        [ gridHeader; gridRule ]
        @ (alternatives
           |> List.map (fun a ->
               "| "
               + a
               + " | "
               + (productGrid |> List.map (cell a) |> String.concat " | ")
               + " |"))

    let s0 = List.head scenarios

    let section (s: Scenario) =
        [
            sprintf "### %s" s.Id
            ""
            s.Description
            ""
            "Five-year incremental assurance cost, midpoint (low–high in the base case table):"
            ""
        ]
        @ table (fun a n -> money (midpoint (evaluate all s a n).Incremental))
        @ [ "" ]
        @ (if List.contains s.Id purposeScenarios then
               [
                   "Break-even is not reported for ODP-09 scenarios: no product-count threshold gates investment (ODP-09, owner brief of 2026-11-26)."
                   ""
               ]
           else
               [
                   "Break-even (smallest product count at which the automated alternative's midpoint is no higher):"
                   ""
                   "| Automated | vs conventional | Break-even N |"
                   "|---|---|---|"
               ]
               @ (breakEvenPairs
                  |> List.map (fun (c, r) ->
                      sprintf
                          "| %s | %s | %s |"
                          c
                          r
                          (match breakEven all s c r with
                           | Some n -> string n
                           | None -> sprintf "none ≤ %d" breakEvenSearchLimit)))
               @ [ "" ])

    [
        "# Assurance economics model v2 (generated)"
        ""
        "Generated by `assurance econ` from `economics/v2/activities.csv`, `alternatives.csv` and `scenarios.csv`."
        "Do not edit by hand. All inputs are hypothetical or vendor claims unless marked otherwise; no quotes exist."
        "Incremental cost excludes baseline product engineering and infrastructure, which are identical across alternatives and shown separately."
        "Opportunity cost (build hours diverted from product work) is reported separately and not added to cash cost."
        ""
        sprintf "## Base case (%s): five-year incremental cost ranges" s0.Id
        ""
    ]
    @ table (fun a n -> range (evaluate all s0 a n).Incremental)
    @ [ ""; "## Base case: marginal five-year cost of the N-th product"; "" ]
    @ table (fun a n -> range (marginal all s0 a n))
    @ [ ""; "## Base case: average five-year cost per product (midpoint)"; "" ]
    @ table (fun a n -> money (midpoint (evaluate all s0 a n).Incremental / float n))
    @ [ ""; "## Base case: opportunity cost of build hours (not included above)"; "" ]
    @ table (fun a n -> range (evaluate all s0 a n).Opportunity)
    @ [
        ""
        sprintf
            "Baseline engineering and infrastructure (all alternatives, not incremental): N=1 %s; N=5 %s."
            (range (evaluate all s0 (List.head alternatives) 1).Baseline)
            (range (evaluate all s0 (List.head alternatives) 5).Baseline)
        ""
        "## Scenarios"
        ""
    ]
    @ (scenarios |> List.collect section)
    @ purposeSection all scenarios alternatives
    |> String.concat "\n"
    |> fun s -> s + "\n"

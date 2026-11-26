/// RFC 4180 CSV parsing and writing, compatible with Python's csv module defaults
/// (comma delimiter, double-quote quoting, QUOTE_MINIMAL, "\n" line terminator).
/// Pure and immutable: the parser is a fold over characters.
module Echelon.Assurance.Registers.Csv

type Row = string list

type private Mode =
    | Unquoted
    | Quoted
    | QuoteSeen // inside quotes, just read a '"' (either escape or closing quote)

type private State =
    {
        Field: char list // reversed
        Row: string list // reversed
        Rows: Row list // reversed
        Started: bool // current field has been started (distinguishes "" from no field)
        Mode: Mode
    }

let private fieldText (chars: char list) =
    chars |> List.rev |> Array.ofList |> System.String

let private endField s =
    { s with
        Row = fieldText s.Field :: s.Row
        Field = []
        Started = false
        Mode = Unquoted
    }

let private endRow s =
    let e = endField s

    { e with
        Rows = List.rev e.Row :: e.Rows
        Row = []
    }

let private append c s =
    { s with
        Field = c :: s.Field
        Started = true
    }

let private step (s: State) (c: char) : State =
    match s.Mode, c with
    | Quoted, '"' -> { s with Mode = QuoteSeen }
    | Quoted, other -> append other s
    | QuoteSeen, '"' -> { append '"' s with Mode = Quoted }
    | QuoteSeen, ',' -> endField s
    | QuoteSeen, '\n' -> endRow s
    | QuoteSeen, '\r' -> { s with Mode = Unquoted }
    | QuoteSeen, other -> { append other s with Mode = Unquoted }
    | Unquoted, '"' when List.isEmpty s.Field -> { s with Mode = Quoted; Started = true }
    | Unquoted, ',' -> endField s
    | Unquoted, '\n' -> endRow s
    | Unquoted, '\r' -> s
    | Unquoted, other -> append other s

let private initial =
    {
        Field = []
        Row = []
        Rows = []
        Started = false
        Mode = Unquoted
    }

/// Parse CSV text into rows. A final line without a terminator is still a row;
/// a trailing terminator does not create an empty row.
let parse (text: string) : Row list =
    let final = text |> Seq.fold step initial

    let pending =
        final.Started || not (List.isEmpty final.Field) || not (List.isEmpty final.Row)

    let closed = if pending then endRow final else final
    List.rev closed.Rows

let private needsQuoting (field: string) =
    field |> Seq.exists (fun c -> c = ',' || c = '"' || c = '\n' || c = '\r')

let private quote (field: string) =
    if needsQuoting field then
        "\"" + field.Replace("\"", "\"\"") + "\""
    else
        field

/// Render rows as CSV with "\n" terminators (QUOTE_MINIMAL).
let render (rows: Row list) : string =
    rows
    |> List.map (fun row -> (row |> List.map quote |> String.concat ",") + "\n")
    |> String.concat ""

/// Header and non-blank data rows.
let table (text: string) : Row * Row list =
    match parse text with
    | [] -> [], []
    | header :: rows ->
        let isBlank (row: Row) =
            row |> List.forall (fun f -> f.Trim() = "")

        header, rows |> List.filter (isBlank >> not)

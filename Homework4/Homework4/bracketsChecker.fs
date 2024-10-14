module bracketsChecker

(*   Скобочная последовательность   *)

let checkBrackets (s: string) =
    let rec check lst chars =
        match chars, lst with
        | [], [] -> true
        | [], _ -> false
        | c::cs, _ when c = '(' || c = '[' || c = '{' -> check (c::lst) cs
        | ')'::cs, '('::ls -> check ls cs 
        | ']'::cs, '['::ls -> check ls cs 
        | '}'::cs, '{'::ls -> check ls cs
        | _ -> false
    s |> Seq.toList |> check []

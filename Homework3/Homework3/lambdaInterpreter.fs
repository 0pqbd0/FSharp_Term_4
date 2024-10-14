module lambdaInterpreter

(*   Лямбда-интерпритатор   *)

type LambdaExpr =
    | Var of string
    | Application of LambdaExpr * LambdaExpr
    | Abstraction of string * LambdaExpr

let rec getFreeVariables term =
    match term with
    | Var x -> Set.singleton x
    | Application (first, second) -> Set.union (getFreeVariables first) (getFreeVariables second)
    | Abstraction (variable, term2) -> getFreeVariables term2 |> Set.remove variable

let isFree variable term = getFreeVariables term |> Set.contains variable  
    
let changeVar varSet = 
    let rec change var =
        if Set.contains var varSet 
            then change var + "_"
        else var 
    change "z"

let rec substitute var expression insertTerm =
    match expression with
    | Var v when v = var -> insertTerm
    | Var _ -> expression
    | Application (expr1, expr2) -> Application (substitute var expr1 insertTerm, substitute var expr2 insertTerm)
    | Abstraction (v, _) when v = var -> expression 
    | Abstraction (v, expr) when isFree v insertTerm |> not || isFree var expr |> not ->
        Abstraction (v, substitute var expr insertTerm)
    | Abstraction (v, expr) -> 
        let newVar = Set.union (getFreeVariables expr) (getFreeVariables insertTerm) |> changeVar
        Abstraction(newVar, substitute var (substitute v expr (Var(newVar))) insertTerm)
    
let rec reduce term =
    match term with
    | Var x -> term 
    | Abstraction (var, expr) -> Abstraction (var, reduce expr)
    | Application (Abstraction(var, expr1), expr2) -> substitute var expr1 expr2
    | Application (expr1, expr2) -> Application (reduce expr1, reduce expr2)
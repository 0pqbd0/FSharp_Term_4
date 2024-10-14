module Utils

(*   1 Задание   *)

let isPrime number =
    if number < 2 then false
    else
        let maxDivider = number |> float |> sqrt |> int
        Seq.exists (fun x -> number % x = 0) {2..maxDivider} |> not

let primeNumbersGenerator = 
    Seq.initInfinite(fun x -> x + 2) |> Seq.filter isPrime
 

(*   2 Задание   *)

type Operator =
    | Plus
    | Minus
    | Multiplication
    | Division

type 'a ParsingTree = 
    | Operand of 'a
    | Operation of 'a ParsingTree * Operator * 'a ParsingTree

let rec calculateTree tree =
    match tree with
    | Operand value -> value
    | Operation (leftOperand, operation, rightOperand) ->
        match operation with
        | Plus -> (calculateTree leftOperand) + (calculateTree rightOperand)
        | Minus -> (calculateTree leftOperand) - (calculateTree rightOperand)
        | Multiplication -> (calculateTree leftOperand) * (calculateTree rightOperand)
        | Division -> (calculateTree leftOperand) / (calculateTree rightOperand)


(*   3 Задание   *)

type 'a BinaryTree =
    | Node of 'a * 'a BinaryTree * 'a BinaryTree
    | Leaf

let rec binaryTreeMap tree mappingFunction =
    match tree with
    | Node (value, left, right) -> 
    Node (mappingFunction value, 
    binaryTreeMap left mappingFunction,
    binaryTreeMap right mappingFunction)
    | Leaf -> Leaf



(*   4 задание   *)

let countEvenUsingMap list = 
    list |> List.map (fun elem -> if elem % 2 = 0 then 1 else 0) |> List.sum

let countEvenUsingFilter list =
    list |> List.filter (fun elem -> elem % 2 = 0) |> List.length

let countEvenUsingFold list =
    list |> List.fold (fun acc elem -> if elem % 2 = 0 then acc + 1 else acc) 0

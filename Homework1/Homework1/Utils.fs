module Utils

let factorial number =
    if number < 0 then None
    else
        let rec calculation acc iter =
            match iter with
            |0 -> Some(acc)
            |x -> calculation (x * acc) (iter - 1)
        calculation 1 number

let fibonacci number = 
    if number <= 0 then None
    else
        let rec calculation firstNumber secondNumber iter =
            match iter with
            | 1 -> Some(firstNumber) 
            | _ -> calculation secondNumber (firstNumber + secondNumber) (iter - 1) 
        calculation 0 1 number

let reverseList list =
    let rec reverser ls acc =
        match ls with
        | [] -> acc
        | head::tail -> reverser  tail (head::acc)
    reverser list []

let powerList n m =
    if n < 0 || m < 0 then None
        else
            let max = pown 2 (n + m)
            let rec calculation acc iter =
                match iter with
                | x when x < n -> Some(acc) 
                | _ -> calculation ((pown 2 iter) :: acc) (iter - 1) 
            calculation [max] (n + m - 1)

let findList list elem =
    let rec finder list iter =
        match list with
        | [] -> None
        | head :: tail when head = elem -> Some(iter) 
        | head :: tail -> finder tail (iter + 1)
    finder list 0
module Homework2.Tests

open NUnit.Framework
open Utils
open FsUnit
open FsCheck

[<Test>]
let IsPrimeTests () =
    let primes = [2; 3; 5; 7; 11; 13; 17; 19; 23; 29; 2147483647]
    primes |> List.iter (fun n -> isPrime n |> should equal true)

    let nonPrimes = [0; 1; 4; 6; 8; 9; 10; 12; 14; 15; 16; 2147483645]
    nonPrimes |> List.iter (fun n -> isPrime n |> should equal false)


let GeneratePrimeNumbersTests () =
    let firstFifteenPrimes = primeNumbersGenerator |> Seq.take 15 |> Seq.toList
    let expectedPrimes = [2; 3; 5; 7; 11; 13; 17; 19; 23; 29; 31; 37; 41; 43; 47]
    firstFifteenPrimes |> should equal expectedPrimes


let correctTestData () =
    [
        Operation(Operation(Operand(1), Plus, Operand(2)), Minus, Operation(Operand(3), Minus, Operand(1))), 1

        Operand(3), 3

        Operation(Operation(Operand(4), Multiplication, Operand(2)), Plus, Operation(Operand(8), Division, Operand(2))), 12

        Operation(Operand(10), Division, Operand(2)), 5
    ]
    |> List.map (fun (expression, expected) -> TestCaseData(expression, expected))

[<TestCaseSource("correctTestData")>]
let CalculateTreeWithCorrectArguments expression expected =
    calculateTree expression |> should equal expected

let binaryTreeTestCases =
    [
        ((fun x -> 0) : int -> int), 
        Node(50, Node(60, Leaf, Leaf), Leaf), 
        Node(0, Node(0, Leaf, Leaf), Leaf)

        ((fun x -> x / 10) : int -> int), 
        Node(100, Node(300, Leaf, Leaf), Leaf), 
        Node(10, Node(30, Leaf, Leaf), Leaf)

        ((fun x -> x * 100): int -> int), Leaf, Leaf
    ]
    |> List.map (fun (f, tree, expected) -> TestCaseData(f, tree, expected))

[<TestCaseSource("binaryTreeTestCases")>]
let BinaryTreeMapWithVariousMappingFunctions (mappingFunction: int -> int) tree expected =
    binaryTreeMap tree mappingFunction |> should equal expected

let countEvenNumbersTestData () = 
    [
        ([], 0)
        ([1], 0)
        ([1..5], 2)
        ([1..11], 5)
        ([-4 .. 1], 3)
        ([2; 4; 6; 8; 18], 5)
    ]
    |> List.map (fun (ls, expected) -> TestCaseData(ls, expected))

[<TestCaseSource("countEvenNumbersTestData")>]
let CountEvenUsingMapTest (ls, expected) =
    countEvenUsingMap ls |> should equal expected

[<TestCaseSource("countEvenNumbersTestData")>]
let CountEvenUsingFilterTest (ls, expected) =
    countEvenUsingFilter ls |> should equal expected

[<TestCaseSource("countEvenNumbersTestData")>]
let CountEvenUsingFoldTest (ls, expected) =
    countEvenUsingFold ls |> should equal expected

[<Test>]
let countEvenWithMapAndWithFilterEquivalent =
    Check.QuickThrowOnFailure(fun list -> countEvenUsingMap list = countEvenUsingFilter list)

[<Test>]
let countEvenWithMapAndWithFoldEquivalent =
    Check.QuickThrowOnFailure(fun list -> countEvenUsingMap list = countEvenUsingFold list)
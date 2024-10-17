module Homework7.Tests

open NUnit.Framework
open Lazy
open System.Threading
open FsUnit


let lazyConstructors  = 
    [ (fun l -> SingleThreadLazy l :> obj ILazy)
      (fun l -> MultiThreadLazy l :> obj ILazy)
      (fun l -> LockFreeLazy l :> obj ILazy) ]
    |> List.map (fun a -> TestCaseData(a))

let multiThreadLazyConstructors =
    [ (fun f -> MultiThreadLazy f :> obj ILazy)
      (fun f -> LockFreeLazy f :> obj ILazy) ]
    |> List.map (fun a -> TestCaseData(a))

[<TestCaseSource("lazyConstructors")>]
[<Repeat(10)>]
let CounterShouldBeIncrementedOnce (lazySupplier: (unit -> obj) -> obj ILazy) =
    let counter = ref 0
    let supplier = fun _ -> 
        let result = Interlocked.Increment(counter)
        result :> obj 
    let lazyInstance = lazySupplier supplier

    lazyInstance.Get() |> ignore
    lazyInstance.Get() |> ignore

    Assert.AreEqual(1, !counter)

[<TestCaseSource("multiThreadLazyConstructors")>]
[<Repeat(10)>]
let  CounterShouldReturnSameValueOnceMultiThread (lazySupplier: (unit -> obj) -> obj ILazy) =
    let counter = ref 0
    let supplier = fun _ -> Interlocked.Increment(counter) 
                            obj()

    let lazyObject = lazySupplier supplier

    Seq.initInfinite (fun _ -> async { return lazyObject.Get()})
    |> Seq.take 50
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Seq.distinct
    |> Seq.length 
    |> should equal 1

[<Test>]
[<Repeat(10)>]
let  MultiThreadLazyShouldUseSupplierOnce () =
    let counter = ref 0
    let supplier = fun _ -> Interlocked.Increment(counter) 
                            obj()

    let lazyObject = MultiThreadLazy supplier :> obj ILazy

    Seq.initInfinite (fun _ -> async { return lazyObject.Get()})
    |> Seq.take 50
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Seq.distinct
    |> Seq.length 
    |> should equal 1

    counter.Value |> should equal 1
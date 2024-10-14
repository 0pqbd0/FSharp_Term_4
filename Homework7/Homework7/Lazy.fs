module Lazy

open System.Threading

type ILazy<'T> =
    abstract member Get: unit -> 'T

type SingleThreadLazy<'T>(supplier: unit -> 'T) =
    let mutable value = None
    let mutable isInitialized = false

    let computeValue () =
        if not isInitialized then
            let result = supplier()
            value <- Some result
            isInitialized <- true
        value.Value

    interface ILazy<'T> with
        member this.Get() =
            computeValue()

type MultiThreadLazy<'T>(supplier: unit -> 'T) =
    let mutable value = None
    let mutable isInitialized = false
    let syncObject = obj()

    let computeValue () =
        if not isInitialized then
            let result = supplier()
            value <- Some result
            isInitialized <- true
        value.Value

    interface ILazy<'T> with
        member this.Get() =
            lock syncObject (fun () ->
                computeValue())

type LockFreeLazy<'T>(supplier: unit -> 'T) =
    let mutable value = None : Option<'T>
    let mutable isInitialized = false

    interface ILazy<'T> with
        member this.Get() : 'T =
            if not isInitialized then
                let suppliedValue = supplier()
                match Interlocked.CompareExchange(&value, Some suppliedValue, None) with
                | Some existingValue -> existingValue
                | None ->
                    isInitialized <- true
                    suppliedValue
            else value.Value

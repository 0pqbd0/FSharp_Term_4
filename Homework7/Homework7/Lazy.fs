module Lazy

open System.Threading

type ILazy<'T> =
    abstract member Get: unit -> 'T

type SingleThreadLazy<'T>(supplier: unit -> 'T) =
    let mutable supplier = Some supplier
    let mutable value = None : Option<'T>

    let computeValue () =
        match value with
        | None ->
            let result = supplier.Value()
            supplier <- None
            value <- Some result
            result
        | Some result -> result

    interface ILazy<'T> with
        member _.Get() =
            computeValue()

type MultiThreadLazy<'T>(supplier: unit -> 'T) =
    let mutable supplier = Some supplier
    let mutable value = None 
    let mutable isInitialized = false
    let syncObject = obj()

    let computeValue () =
        if not isInitialized then
            let result = supplier.Value()
            supplier <- None
            value <- Some result
            isInitialized <- true
        value.Value

    interface ILazy<'T> with
        member this.Get() =
            lock syncObject (fun () ->
                computeValue())

type LockFreeLazy<'T>(supplier: unit -> 'T) =
    let mutable supplier = Some supplier
    let mutable value = None : Option<'T>
    let mutable isInitialized = false

    interface ILazy<'T> with
        member this.Get() : 'T =
            if not isInitialized then
                let suppliedValue = supplier.Value()
                match Interlocked.CompareExchange(&value, Some suppliedValue, None) with
                | Some existingValue -> existingValue
                | None ->
                    supplier <- None
                    isInitialized <- true
                    suppliedValue
            else value.Value

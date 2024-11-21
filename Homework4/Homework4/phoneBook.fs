module phoneBook

open System
open System.IO

type PhoneEntry = { Name: string; PhoneNumber: string }
type Phonebook = PhoneEntry list

let addEntry (entry: PhoneEntry) (phonebook: Phonebook) = entry :: phonebook

let findPhoneByName (name: string) (phonebook: Phonebook) =
    phonebook
    |> List.filter (fun e -> e.Name = name)
    |> List.map (fun e -> e.PhoneNumber)

let findNameByPhone (phoneNumber: string) (phonebook: Phonebook) =
    phonebook
    |> List.filter (fun e -> e.PhoneNumber = phoneNumber)
    |> List.map (fun e -> e.Name)

let printPhoneBook (phoneBook: Phonebook) =
    if List.isEmpty phoneBook then
        printfn "Phonebook is empty."
    else
        phoneBook |> List.iter (fun entry -> printfn "Name: %s, Phone: %s" entry.Name entry.PhoneNumber)

let saveToFile (filename: string) (phonebook: Phonebook) =
    let data = phonebook |> List.map (fun e -> sprintf "%s,%s\n" e.Name e.PhoneNumber)
    File.WriteAllText(filename, String.concat "" data)
    printfn "Phonebook saved to %s" filename

let readFromFile (filename: string) =
    try
        File.ReadAllLines(filename)
        |> Array.map (fun line ->
            let parts = line.Split(',')
            { Name = parts.[0].Trim(); PhoneNumber = parts.[1].Trim() })
        |> List.ofArray
    with
    | :? FileNotFoundException ->
        printfn "File not found: %s" filename
        []

let rec commandLoop phoneBook =
    printfn "Enter command (add, findphone, findname, printall, save, load, exit):"
    let input = Console.ReadLine().ToLower().Split(' ')

    let processAdd name phone =
        let updatedPhoneBook = addEntry { Name = name; PhoneNumber = phone } phoneBook
        printfn "Entry added: %s, %s" name phone
        commandLoop updatedPhoneBook

    let processFindPhone name =
        match findPhoneByName name phoneBook with
        | [] -> printfn "No phone found for name: %s" name
        | phones -> phones |> List.iter (printfn "Phone for %s: %s" name)
        commandLoop phoneBook

    let processFindName phone =
        match findNameByPhone phone phoneBook with
        | [] -> printfn "No name found for phone: %s" phone
        | names -> names |> List.iter (printfn "Name for phone %s: %s" phone)
        commandLoop phoneBook

    let processPrintAll () =
        printPhoneBook phoneBook
        commandLoop phoneBook

    let processSave filePath =
        saveToFile filePath phoneBook
        commandLoop phoneBook

    let processLoad filePath =
        let loadedPhoneBook = readFromFile filePath
        printfn "Phonebook loaded from %s" filePath
        commandLoop loadedPhoneBook

    match input with
    | [| "exit" |] -> 
        printfn "Exiting the program."
    | [| "add"; name; phone |] -> processAdd name phone
    | [| "findphone"; name |] -> processFindPhone name
    | [| "findname"; phone |] -> processFindName phone
    | [| "printall" |] -> processPrintAll ()
    | [| "save"; filePath |] -> processSave filePath
    | [| "load"; filePath |] -> processLoad filePath
    | _ -> 
        printfn "Invalid command."
        commandLoop phoneBook

[<EntryPoint>]
let main argv =
    printfn "Hello! This is a phone book!"
    let initialPhonebook = []
    commandLoop initialPhonebook
    0

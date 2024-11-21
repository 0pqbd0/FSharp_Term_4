module phoneBookTest

open System.IO
open NUnit.Framework
open FsUnit
open phoneBook

[<Test>]
let addEntryToPhonebook () =
    let phoneBook = []
    let newEntry = { Name = "Ivan"; PhoneNumber = "+79001002030" }
    let updatedPhoneBook = addEntry (newEntry) phoneBook
    updatedPhoneBook |> List.exists (fun x -> x.Name = newEntry.Name) |> should be True
    updatedPhoneBook |> List.exists (fun x -> x.PhoneNumber = newEntry.PhoneNumber) |> should be True

[<Test>]
let findPhoneByName () =
    let phoneBook = []
    let newEntry = { Name = "Ivan"; PhoneNumber = "+79001002030" }
    let updatedPhoneBook = addEntry (newEntry) phoneBook
    let foundPhone = findPhoneByName newEntry.Name updatedPhoneBook
    foundPhone[0] |> should equal newEntry.PhoneNumber

[<Test>]
let findNameByPhone () =
    let phoneBook = []
    let newEntry = { Name = "Ivan"; PhoneNumber = "+79001002030" }
    let updatedPhoneBook = addEntry (newEntry) phoneBook
    let foundName = findNameByPhone newEntry.PhoneNumber updatedPhoneBook
    foundName[0] |> should equal newEntry.Name

[<Test>]
let SaveAndReadFromFile () =
    let tmpFilePath = Path.GetTempFileName()
    let newEntryIvan = { Name = "Ivan"; PhoneNumber = "+79001002030" }
    let newEntryAlena = { Name = "Alena"; PhoneNumber = "+79012003050" }
    let phoneBook = [] |>  addEntry (newEntryIvan) |> addEntry (newEntryAlena)
    saveToFile tmpFilePath phoneBook
    let loadedPhoneBook = readFromFile tmpFilePath
    phoneBook |> should equal loadedPhoneBook 
    File.Delete(tmpFilePath) 
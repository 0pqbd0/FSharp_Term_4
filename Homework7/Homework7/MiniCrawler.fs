module MiniCrawler

open System
open System.Net.Http
open System.Text.RegularExpressions
open System.Threading

let downloadPageAsync (url: string, httpClient: HttpClient) =
    async {
        try
            let request = new HttpRequestMessage(HttpMethod.Get, url) 
            let! response = httpClient.SendAsync(request, CancellationToken.None) |> Async.AwaitTask
            if response.IsSuccessStatusCode then
                return! response.Content.ReadAsStringAsync() |> Async.AwaitTask
            else
                printfn "Failed to download %s. Status code: %d" url (int response.StatusCode)
                return ""
        with ex ->
            printfn "Exception while downloading %s: %s" url ex.Message
            return ""
    }

let extractLinks (html: string) =
    let pattern = "<a\\s+(?:[^>]*?\\s+)?href=\"(https://[^\"]*)\""
    Regex.Matches(html, pattern)
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.Groups.[1].Value)
    |> Seq.toArray

let fetchSizes (url: string, client: HttpClient) =
    async {
        let! mainHtml = downloadPageAsync (url, client)
        let links = extractLinks mainHtml

        let fetchTasks =
            links
            |> Seq.map (fun link -> 
                async {
                    let! html = downloadPageAsync (link, client)
                    return (link, html.Length)
                })
        let! results = fetchTasks |> Async.Parallel
        return results
    }

let printSizes (sizes: (string * int)[]) =
    sizes
    |> Array.iter (fun (link, size) -> printfn "%s — %d characters." link size)

let fetchAndPrintSizes (client: HttpClient) (url: string) =
    async {
        let! sizes = fetchSizes (url, client)
        printSizes sizes
    }
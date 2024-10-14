module MiniTest

open System.Threading
open NUnit.Framework
open System.Net.Http
open Moq
open System.Net
open FsUnit
open MiniCrawler


[<Test>]
let MockLinkWithOneLinkShouldWorkCorrect (): unit =
    let client = Mock<HttpClient>()
    let url = "https://example.com/"

    let response = new HttpResponseMessage(HttpStatusCode.OK)

    client
        .Setup(fun client -> client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(response)
        .Callback(fun (req: HttpRequestMessage) (ct: CancellationToken) -> 
            if (req).RequestUri.AbsoluteUri = url then
                response.Content <- new StringContent("Example content")
            else
                response.Content <- new StringContent($"<html><body><a href=\"{url}\">Example</a></body></html>")) |> ignore

    let expected = [| (url, 15) |]
    let actual = Async.RunSynchronously (fetchSizes ("https://fakeurl.com", client.Object))
    
    Assert.AreEqual(expected, actual)

[<Test>]
let MockLinkWithOneCorrectLinkAndOneIncorrectShouldWorkCorrect (): unit =
    let client = Mock<HttpClient>()
    let url = "https://example.com/"
    let urlNotFould = "notFound.com"

    let response = new HttpResponseMessage(HttpStatusCode.OK)

    client
        .Setup(fun client -> client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(response)
        .Callback(fun (req: HttpRequestMessage) (ct: CancellationToken) -> 
            if (req).RequestUri.AbsoluteUri = url then
                response.Content <- new StringContent("Example content")
            else
                response.Content <- new StringContent($"<html><body><a href=\"{url}\">Example</a></body> <a href=\"{urlNotFould}\">Example</a></body></html>")) |> ignore

    let expected = [| (url, 15) |]
    let actual = Async.RunSynchronously (fetchSizes ("https://fakeurl.com", client.Object))
    
    Assert.AreEqual(expected, actual)

module Tests


open System
open System.IO
open System.Net
open System.Net.Http
open Xunit
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.DependencyInjection
open Braili.Web


// ---------------------------------
// Helper functions (extend as you need)
// ---------------------------------

let startupObject = new Startup()

let createHost() =
    WebHostBuilder()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .Configure(Action<IApplicationBuilder> startupObject.Configure)
        .ConfigureServices(Action<IServiceCollection> startupObject.ConfigureServices)

let runTask task =
    task
    |> Async.AwaitTask
    |> Async.RunSynchronously

let httpGet (path : string) (client : HttpClient) =
    path
    |> client.GetAsync
    |> runTask

let httpPost (path : string) (content: HttpContent) (client : HttpClient)  =
    client.PostAsync(path,content)
    |> runTask

let isStatus (code : HttpStatusCode) (response : HttpResponseMessage) =
    Assert.Equal(code, response.StatusCode)
    response

let ensureSuccess (response : HttpResponseMessage) =
    if not response.IsSuccessStatusCode
    then response.Content.ReadAsStringAsync() |> runTask |> failwithf "%A"
    else response

let readText (response : HttpResponseMessage) =
    response.Content.ReadAsStringAsync()
    |> runTask

let shouldEqual expected actual =
    Assert.Equal(expected, actual)

let shouldContain (expected : string) (actual : string) =
    Assert.True(actual.Contains expected)

// ---------------------------------
// Tests
// ---------------------------------

[<Fact>]
let ``Route /health returns "alive"`` () =
    use server = new TestServer(createHost())
    use client = server.CreateClient()

    client
    |> httpGet "/health"
    |> ensureSuccess
    |> readText
    |> shouldContain "alive"

[<Fact>]
let ``Route /ocr returns text content from an image`` () =
    use server = new TestServer(createHost())
    use client = server.CreateClient()

    let headerPart = sprintf "Upload----%A" DateTime.UtcNow
    use content = new MultipartFormDataContent(headerPart)
    use fileStream = File.Open("helloworld.png", FileMode.Open)
    content.Add(new StreamContent(fileStream), "test-picture", "upload.png")
    client
    |> httpPost "/ocr" content
    |> ensureSuccess
    |> readText
    |> shouldContain "hello world!"

[<Fact>]
let ``Route which doesn't exist returns 404 Page not found`` () =
    use server = new TestServer(createHost())
    use client = server.CreateClient()

    client
    |> httpGet "/route/which/does/not/exist"
    |> isStatus HttpStatusCode.NotFound
    |> readText
    |> shouldEqual "Not Found"
namespace Braili.Web

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Giraffe
open HttpHandler

//https://github.com/giraffe-fsharp/Giraffe

type Startup() =

    let webApp =
        choose [
            GET >=>
                choose [
                    route "/health" >=> text "alive"
                ]
            POST >=>
                choose [
                    route "/ocr" >=> (handleFormRequest readTextFromFormFileImage)
                ]
            setStatusCode 404 >=> text "Not Found" ]

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddGiraffe() |> ignore

    member this.Configure(app: IApplicationBuilder) =
        app.UseGiraffe webApp
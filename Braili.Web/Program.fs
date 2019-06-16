namespace Braili.Web

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging

module Program =
    let exitCode = 0

    let CreateWebHostBuilder args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseStartup<Startup>()

    let configureLogging (builder : ILoggingBuilder) =
        builder
            //.AddFilter(filter) // Optional filter
            .AddConsole()      // Set up the Console logger
            .AddDebug()        // Set up the Debug logger
        |> ignore

    [<EntryPoint>]
    let main args =
        CreateWebHostBuilder(args)
            .ConfigureLogging(configureLogging)
            .Build()
            .Run()

        exitCode

module HttpHandler

    open Microsoft.AspNetCore.Http
    open Giraffe
    open FSharp.Control.Tasks.ContextInsensitive
    open Ocr

    let handleOcrRequest =
        fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let resultText = readTextfromImage
            return! text resultText next ctx
        }
            

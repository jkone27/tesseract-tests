module HttpHandler

    open Microsoft.AspNetCore.Http
    open Giraffe
    open FSharp.Control.Tasks.ContextInsensitive
    open Ocr
    open System.Drawing.Common

    let handleOcrRequest =
        fun (next : HttpFunc) (ctx : HttpContext) ->
        task {

            //if (ctx.Request.ContentType <> "multipart/form-data") then
            //    let! r = RequestErrors.BAD_REQUEST "unsupported mime" next ctx 
            //    return r
            let img = new Image()
            use imageStream = ctx.Request.Form.Files.[0].OpenReadStream()
            let test : byte[] = [||]
            let resultText = readTextfromImage test
            return! text resultText next ctx
        }
            

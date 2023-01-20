using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace webapi.Controllers;

[ApiController]
[Route("/wss")]
public class InfoController : ControllerBase
{
    [HttpGet("subinfo")]
    public async Task SubserverAsync(int group,int seq_num){
        if(HttpContext.WebSockets.IsWebSocketRequest){
            using var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var info = HttpContext.Request.Host;
           
            try{
                var service = App.get_service<service.ObserverService>()!;
                var tar_server = service.group_sub[group][seq_num];
                await tar_server.ws_info_sync(ws);
            }
            catch(OperationCanceledException){

            }
            await ws.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            string.Empty,
            CancellationToken.None);
        }
        else{
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    [HttpGet("dashboard")]
    public async Task dashbroadSync(){
        if(HttpContext.WebSockets.IsWebSocketRequest){
            using var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var info = HttpContext.Request.Host;

            var service = App.get_service<service.ObserverService>()!;
            await service.dashboard_echo(ws);
            
            await ws.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            string.Empty,
            CancellationToken.None);
        }
        else{
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    /*
    public async Task echo_info(WebSocket ws){
        var buffer = new byte[1024];
        var receive = await ws.ReceiveAsync(
            new ArraySegment<byte>(buffer),
            CancellationToken.None
        );

        while(!receive.CloseStatus.HasValue){

            var info = Encoding.UTF8.GetString(buffer,0,receive.Count);
            if(info == "ws_close")
                break;

            await ws.SendAsync(
                new ArraySegment<byte>(init_info()),
                WebSocketMessageType.Text,
                WebSocketMessageFlags.EndOfMessage,
                CancellationToken.None
            );

            receive = await ws.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None
            );

        }

        Console.WriteLine("ws closed");
        await ws.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            string.Empty,
            CancellationToken.None);
        
    }
    */

}



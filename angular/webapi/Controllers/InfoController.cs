using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace webapi.Controllers;

public class InfoController : ControllerBase
{
    [HttpGet("/wss")]
    public async Task GetInfoAsync(){
        if(HttpContext.WebSockets.IsWebSocketRequest){
            using var ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var info = HttpContext.Request.Host;
            Console.WriteLine($"open a web socket from {info}");
            await echo_info(ws);
        }
        else{
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

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

    Random gen = new Random();

    public byte[] init_info(){
        var num1 = gen.Next() % 1331;
        var num2 = gen.Next() % 1331;
        var res = num1+num2 + (gen.Next() % 2);
        var st = (res == num1+num2 ? "accept" : "wrong_answer");
        var str = $"{num1}+{num2}=|{res}|{st}";
        return Encoding.UTF8.GetBytes(str);
    }

}



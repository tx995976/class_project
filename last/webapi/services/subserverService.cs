using System.Net.WebSockets;

namespace webapi.service;

/*
    #与前端计分板链接
        @同步分数
    #节点状态检测
        @心跳包
    ?wss 格式 
        1. server|name|none
        2. result|case|score !(status -> done)
        3. status|case|stats(fetch,commit)
*/
public class subserverService
{
    public static int live_time = 6000; //心跳时间
    public static bool is_running = false;

    public subserver_info? current_server { 
        get => _current_server;
        set{
            if(_current_server is not null){
                _current_server.Isactive = false;
                stopWatch();
                message_buffer.Clear();
            }
            _current_server = value!;
            if(value is null)
                return;
            _current_server.Isactive = true;
            startWatch();
            if(is_running)
                server_wakeup();
        }
    }
    subserver_info? _current_server;

    Timer? _timer;
    HttpClient heart_client = new();
    bool iswatch = false;

    SemaphoreSlim ws_sync_signal = new(0);
    Queue<string> message_buffer = new();

    ILogger<subserverService> logger;

    public event Action<subserverService>? subserver_error;

    public subserverService( ILogger<subserverService> _log){
        logger = _log;
    }

    #region server operations

    void startWatch(){
        if(iswatch)
            return;
        _timer = new(server_watch,null,0,live_time);
        logger.LogInformation("Start watching server {0}",current_server!.name);
        iswatch = true;
        server_process_receive();
    }

    void stopWatch(){
        if(!iswatch)
            return;
        iswatch = false;
        _timer?.Dispose();
       logger.LogInformation("stop watching server {0}",current_server?.name);
    }

    async void server_watch(object? para){
        try{
            //logger.LogInformation("send pack");
            var request = await heart_client.GetAsync(current_server!.hostname+"/api/test");
        }
        catch(Exception){
            stopWatch();
            subserver_error?.Invoke(this);
        }
    }

    //唤醒服务器
    async public void server_wakeup(){
        if(current_server is null)
            return;
        var get = new HttpClient();
        try{
            var request = await heart_client.PostAsJsonAsync(current_server.hostname+"/api/start",
                questionService.start_token!);

            logger.LogInformation("server {0} start to calculate",current_server.name);
        }
        catch(HttpRequestException){
            logger.LogWarning("can't wakeup server {0}",current_server.name);

        }
    }

    async public void server_stop(){
        if(current_server is null)
            return;
        var get = new HttpClient();
        try{
            var request = await heart_client.PostAsJsonAsync(current_server.hostname+"/api/start",
                questionService.start_token! with {Iscancelltoken = true});

            logger.LogInformation("server {0} stop",current_server.name);
        }
        catch(HttpRequestException){
            logger.LogWarning("failed connect to server {0}",current_server.name);

        }
    }

    #endregion

    #region data sync

    public void result_receive(judge_result data){
        if(data.uuid == current_server?.uuid)
            message_push(data.get_result_str());
    }

    public void commit_receive(answers data){
        if(data.uuid == current_server?.uuid)
            message_push(data.get_commit_str());
    }

    public void fetch_receive(fetch_info data){
        if(data.uuid == current_server?.uuid)
            message_push(data.get_fetch_str());
    }

    public void server_process_receive(){   
        message_push($"server|{current_server!.name}|none");
        //ADDON: 同步前面所有记录

    }

    private void message_push(string data)
    {
        message_buffer.Enqueue(data);
        if (ws_sync_signal.CurrentCount == 0)
            ws_sync_signal.Release();
    }


    async public Task ws_info_sync(WebSocket ws){
        //ADDON: 完成断线续传
        
        while(ws.State == WebSocketState.Open){
            await ws_sync_signal.WaitAsync();
        
            while(message_buffer.Count is not 0){
                var data = Encoding.UTF8.GetBytes(message_buffer.Dequeue());
                await ws.SendAsync(
                    new ArraySegment<byte>(data),
                    WebSocketMessageType.Text,
                    WebSocketMessageFlags.EndOfMessage,
                    CancellationToken.None
                );
            }
        }
    }


    #endregion


}
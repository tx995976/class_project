using Snowflake.Core;
using System.Net.WebSockets;

namespace webapi.service;


/*
    #监测节点活动
        @注册节点
        @唤醒节点
        @节点数据检测与异常处理
    ?wss 格式
        1. score|group1-group2...-
        2. group|group_num|each_group
*/

public class ObserverService
{
    static IdWorker snow_id = new(1,20);

    public static int groups = 2; //st form 0
    public static int groups_node = 2;

    ILogger<ObserverService> logger;
    dataService dbdata;
    
    public List<List<subserverService>> group_sub = new();

    Queue<string> message_buffer = new();
    SemaphoreSlim message_lock = new(1);

    //public bool contest_running = false;

    public event Action<subserver_info>? new_subserver_join;
    public event Action? contest_started;
    public event Action? contest_stopped;

    public ObserverService(ILogger<ObserverService> _log,dataService db){
        logger = _log; 
        dbdata = db;
    }

    public void start(){
        var ques_service = App.get_service<questionService>()!;
        for(int i = 0; i < groups; i++){
            var list = new List<subserverService>();
            for(int j = 0; j < groups_node; j++){
                var node = App.get_service<subserverService>()!;
                node.subserver_error += replace_subserver;

                contest_started += node.server_wakeup;
                contest_stopped += node.server_stop;

                ques_service.fetch_problems += node.fetch_receive;
                ques_service.answer_scores += node.result_receive;
                ques_service.receive_answers += node.commit_receive;

                list.Add(node);
            }
            group_sub.Add(list);
        }

        new_subserver_join += new_subserver;
        dbdata.score_up_result += push_message;

        logger.LogInformation("Observe Service ready | group : {0} |" , groups);
    }

    public void start_contest(){
        subserverService.is_running = true;
        contest_started?.Invoke();
        logger.LogInformation("contest started");
    }

    public void stop_contest(){
        subserverService.is_running = false;
        contest_stopped?.Invoke();
        logger.LogInformation("contest end");
    }

    #region subserver manage

    public reg_info reg_subserver(string host,string name,int group){
        var uuid = snow_id.NextId();
        var info = new subserver_info{
            uuid = uuid,
            hostname = host,
            name = name,
            group = group
        };

        dbdata.subservers.Add(uuid,info);
        dbdata.oj_results.Add(uuid, new());
        dbdata.commit_list.Add(uuid,new());
        dbdata.fetch_list.Add(uuid, new());

        new_subserver_join?.Invoke(info);

        logger.LogInformation("server {0} register in group {1}",name,group);

        return new reg_info{
            success = true,
            uuid = uuid
        };
    }

    public void new_subserver(subserver_info new_sub){
        var group_num = new_sub.group;
        foreach(var server in group_sub[group_num]){
            if(server.current_server is null){
                server.current_server = new_sub;
                break;
            }
        }
    }

    public void replace_subserver(subserverService node){
        var error_subserver = node.current_server;
        logger.LogWarning("subserver {0} error,try to replace",error_subserver?.name);
        
        var new_node = dbdata.subservers.Values
                    .Where(x => (x.group == error_subserver!.group) && !x.Isactive)
                    .FirstOrDefault();
                    
        if(new_node is not null){
            node.current_server = new_node;
            logger.LogInformation("subserver {0} select",new_node?.name);
        }
        else{
            node.current_server = null;
            logger.LogWarning("no server to select \n\tObserver will replace when new server register in");
        }
    
        error_subserver!.Isactive = false;
    }

    #endregion

    #region dashboard sync

    public void push_message(string data){
        message_buffer.Enqueue(data);
        if(message_lock.CurrentCount == 0)
            message_lock.Release();
    }

    async public Task dashboard_echo(WebSocket ws){
        push_message($"group|{groups}|{groups_node}");

        while(ws.State == WebSocketState.Open){
            await message_lock.WaitAsync();

            if(ws.State != WebSocketState.Open){
                message_lock.Release();
                break;
            }

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

    #region data observer
    //ADDON: 分析数据动态调整




    #endregion

}


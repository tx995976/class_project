using subserver.calculate;
using subserver.services;
using System.Collections.Generic;
using System.Net.Http;

using shared.Model;

namespace subserver;

public class App{

    static WebApplication? _app;

    //server info
    public static string? subserver_name;
    public static long uuid;
    public static int group;
    public static string subserver_url = "";
    public static string master_url = "https://localhost:7232";
    public static string kafka_url = "";

    async public static Task Main(string[] args){

        #region Config
        var config = new Dictionary<string,string>();
        if (args.Length < 8){
            Console.WriteLine("\u001B[31m 缺少配置\nUsage:\n p 指定端口\n n 指定节点名字\n m 指定计算方法\n g 指定组 \u001B[0m");
            return;
        }
        else{
            config[args[0]] = args[1];
            config[args[2]] = args[3];
            config[args[4]] = args[5];
            config[args[6]] = args[7];
        }

        subserver_name = config["n"];
        subserver_url = "https://localhost:"+config["p"];
        group = int.Parse(config["g"]);

        Console.WriteLine($"\u001B[32minfo: \u001B[0mserver name: {subserver_name}");
        Console.WriteLine($"\u001B[32minfo: \u001B[0mserver group: {group}");

        #endregion

        #region http server
        var builder = WebApplication.CreateBuilder();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //url
        builder.WebHost.UseUrls(subserver_url);

        // Ioc settings
        builder.Services.AddSingleton<CalculateService>();
 

        _app = builder.Build();
        // Configure the HTTP request pipeline.
        if (_app.Environment.IsDevelopment())
        {
            _app.UseSwagger();
            _app.UseSwaggerUI((config) => {
                config.RoutePrefix = "swagger";
            } );
        }

        _app.UseHttpsRedirection();
        _app.UseAuthorization();
        _app.MapControllers();

        //method find
        var node_type = Type.GetType($"subserver.calculate.{config["m"]}");
        if(node_type == null){
            Console.WriteLine($"\u001B[31mmethod {config["m"]} not found \u001B[0m");
            return;
        }
        Console.WriteLine($"\u001B[32minfo: \u001B[0mmethod: {node_type.FullName}");
        get_service<CalculateService>().alo = (Inode_calculator)Activator.CreateInstance(node_type)!;

        //cancel method
        if(await Task.Run(() => reg_node()) is false)
            return;
        _app.Run();

        #endregion
    }

    public static T get_service<T>() => _app!.Services.GetService<T>()!;


    /*
    @send hostname,app_name,group;
    @receive uuid
    */
    async public static Task<bool> reg_node(){
        var get = new HttpClient{
            BaseAddress = new Uri(master_url)
        };

        try{
            var back = (await get.GetAsync($"api/register?host={subserver_url}&name={subserver_name}&group={group}"));
            var data = await back.Content.ReadFromJsonAsync<reg_info>();
        
            if(!(data?.success ?? false)|| back.StatusCode != System.Net.HttpStatusCode.OK){
                Console.WriteLine($"\u001B[31m failed: \u001B[0m can't register to master");
                return false;
            }

            Console.WriteLine($"\u001B[32minfo: \u001B[0mnode register to master\n\twaiting master signal");
            uuid = data.uuid;
        }
        catch(HttpRequestException){
            Console.WriteLine($"\u001B[31merror: \u001B[0m can't connect to master server");
        }
        return true;
    }
}
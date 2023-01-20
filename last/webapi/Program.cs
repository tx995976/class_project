global using System.Collections.Generic;
global using shared.Model;
global using System.Text;

namespace webapi;

public class App{

    static WebApplication? _app;

    public static void Main(string[] args){

        #region server
        
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Ioc
        builder.Services.AddSingleton<service.ObserverService>();
        builder.Services.AddSingleton<service.questionService>();
        builder.Services.AddSingleton<service.dataService>();
        builder.Services.AddTransient<service.subserverService>();
        
        
        _app = builder.Build();

        // Configure the HTTP request pipeline.
        if (_app.Environment.IsDevelopment())
        {
            _app.UseSwagger();
            _app.UseSwaggerUI();
        }

        _app.UseCors(options => options.AllowAnyOrigin());
        _app.UseHttpsRedirection();
        _app.UseAuthorization();
        _app.MapControllers();
        //websocket
        var webSocketOptions = new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2)
        };
        _app.UseWebSockets(webSocketOptions);

        Task.Run(() => get_service<service.ObserverService>()!.start());
        Task.Run(() => get_service<service.dataService>()!.start());
        
        _app.Run();

        #endregion
    }

    public static T? get_service<T>() => _app!.Services.GetService<T>();


}
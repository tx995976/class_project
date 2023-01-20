using Microsoft.AspNetCore.Mvc;

using shared.Model;

namespace subserver.Controllers;

[ApiController]
[Route("/api")]
public class wakeupController : ControllerBase
{
    [HttpPost("start")]
    async public void wakeup(start_info info)
    {
        var service = App.get_service<services.CalculateService>();
        if(info.Iscancelltoken){
            service.defuse_cal();
            return;
        }
        service.time_limit = info.time_limit;
        service.all_cases = info.all_cases;
        await Task.Run(() => service.start_calculate());
        return;
    }

    [HttpGet("test")]
    public int test(){
        return 200;
    }

}
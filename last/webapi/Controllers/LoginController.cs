using Microsoft.AspNetCore.Mvc;
using shared.Model;

namespace webapi.Controllers;

[ApiController]
[Route("api/register")]
public class LoginController : ControllerBase
{
    [HttpGet]
    public reg_info register(string host,string name,int group)
    {
        var server = App.get_service<service.ObserverService>();
        
        if(service.ObserverService.groups <= group)
            return new reg_info{
                success = false
            };

        return server!.reg_subserver(host, name, group);
    }

}


using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController :Controller
{
    [HttpGet]
    public string login(string id,string password)
    {
        return $"\"ok user:{id} is login\"";
    }

}


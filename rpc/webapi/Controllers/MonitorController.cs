using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MonitorController : ControllerBase
    {
        public MonitorController() { }

        PerformanceCounter cpuCounter = new ("Processor", "% Processor Time", "_Total");
        PerformanceCounter ramCounter = new ("Memory", "Available MBytes");

        [HttpGet(Name = "getuseage")]
        public info GetPair()
        {
            var cpu = cpuCounter.NextValue();
            var ram = ramCounter.NextValue();
            return new info() { cpu_percent = cpu, ram_last = ram };
        }
    }

    public record class info
    {
        public double cpu_percent { get; set; }
        public double ram_last { get; set; }
    }
}

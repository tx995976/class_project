using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using test.rpc;

namespace test;

public class MonitorHandler : MonitorService.IAsync
{
    public async Task<ProcessCPURAM> getProcessInfo(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("receive RPC");
        var response = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7005"),
        };

        var getres = await response.GetFromJsonAsync<info>("Monitor");
        var res = new ProcessCPURAM
        {
            CpuPercentage = getres!.cpu_percent,
            RamPercentage = getres!.ram_last
        };
        return await Task.FromResult(res);
    }

    public class info
    {
        public double cpu_percent { get; set; }
        public double ram_last { get; set; }
    }
}


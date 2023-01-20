// See https://aka.ms/new-console-template for more information
using test;
using Thrift;
using test.rpc;
using Thrift.Processor;
using Thrift.Protocol;
using Thrift.Transport;
using Thrift.Transport.Client;
using Thrift.Transport.Server;

TTransport trans = new TSocketTransport("localhost", 9090, null, 1000);
TProtocol protocol = new TBinaryProtocol(trans, true, true);

var client = new MonitorService.Client(protocol);
var token = new CancellationTokenSource();

var res =  await client.getProcessInfo(token.Token);
Console.WriteLine($"result is Cpu:{res.CpuPercentage}|Ram:{res.RamPercentage}");
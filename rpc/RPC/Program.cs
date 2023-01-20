// See https://aka.ms/new-console-template for more information

using test;
using Thrift;
using test.rpc;
using Thrift.Processor;
using Thrift.Protocol;
using Thrift.Transport;
using Thrift.Transport.Server;



TServerTransport socket = new TServerSocketTransport(9090, null, 1000);
TTransportFactory transportFactory = new TBufferedTransport.Factory();
TProtocolFactory protocolFactory = new TBinaryProtocol.Factory(true,true);
ITAsyncProcessor processor = new MonitorService.AsyncProcessor(new MonitorHandler());

var server = new Thrift.Server.TThreadPoolAsyncServer(processor, socket, transportFactory, protocolFactory);
var token = new CancellationTokenSource();
await Task.Run(() => { Console.WriteLine("server start"); server.ServeAsync(token.Token); });

Console.WriteLine("press any key to stop");
Console.ReadLine();
token.Cancel();


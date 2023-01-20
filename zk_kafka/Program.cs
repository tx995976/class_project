using Confluent.Kafka;

using zk_kafka.test;

var cancellation = new CancellationTokenSource();
Console.WriteLine("press cancel to stop");

Parallel.Invoke(() => {kafkaClient.start_producer(cancellation.Token);});
await Task.Delay(3000);
Parallel.Invoke(() => {kafkaClient.start_consumer(cancellation.Token);});

Console.CancelKeyPress += (_,e) => {
    cancellation.Cancel();
    e.Cancel = true;
};

cancellation.Token.WaitHandle.WaitOne();
Console.WriteLine("Clients stopped");
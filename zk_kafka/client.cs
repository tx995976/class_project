using Confluent.Kafka;

namespace zk_kafka.test;

public class kafkaClient{

    static public async void start_producer(CancellationToken cancell){
        var config = new ProducerConfig{
            BootstrapServers = "localhost:9092"
        };

        using (var pr = new ProducerBuilder<Null,string>(config).Build())
        {
            Console.WriteLine("Starting producer");
            while(!cancell.IsCancellationRequested){
                var data = await pr.ProduceAsync(
                    "test_hello", 
                    new Message<Null,string> {Value = "hello"});
                Console.WriteLine($"Produced top:{data.Topic} value:{data.Value}");
                await Task.Delay(1500);
            }
        }
        Console.WriteLine("Finished producer");
    }

    static public async void start_consumer(CancellationToken cancell){
        var config = new ConsumerConfig{
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = "test_consumer"
        };

        using (var con = new ConsumerBuilder<Ignore,string>(config).Build())
        {
            Console.WriteLine("starting consumer");
            con.Subscribe("test_hello");

            while(!cancell.IsCancellationRequested){
                try{
                    var data = con.Consume(cancell);
                    Console.WriteLine($"consumer top:{data.Topic} value:{data.Message.Value}");
                }
                catch(ConsumeException e){
                    Console.WriteLine(e);
                }
            }
        }
        Console.WriteLine("consumer stoped");
        await Task.CompletedTask;
    }


}
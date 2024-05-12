using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

public class KafkaProducerService : IHostedService
{
    private readonly ProducerConfig _producerConfig;
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;
    public KafkaProducerService()
    {
        // Initialize Kafka producer configuration
        _producerConfig = new ProducerConfig
        {
            BootstrapServers = "kafka:9092" // Replace with your Kafka broker endpoint
        };

        // Create a new Kafka producer
        _producer = new ProducerBuilder<string, string>(_producerConfig).Build();
        // Specify the Kafka topic to produce messages to
        _topic = "log"; // Replace with your Kafka topic
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Kafka producer service started.");

        // Generate and produce 1 million messages
        const int batchSize = 1000;
        var messageCount = 100000;
        var messagesProduced = 0;

        while (messagesProduced < messageCount && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                var messages = new List<Message<string, string>>();

                // Generate a batch of messages
                for (int i = 0; i < batchSize; i++)
                {
                    var key = Guid.NewGuid().ToString(); // Generate a unique key for each message
                    var value = $"Message {messagesProduced + i + 1}";
                    messages.Add(new Message<string, string> { Key = key, Value = value });
                    await _producer.ProduceAsync(_topic, new Message<string, string> { Key = key, Value = value });
                }
                // Produce the batch of messages to the Kafka topic

                messagesProduced += batchSize;
                Console.WriteLine($"Produced {messagesProduced} messages out of {messageCount}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while producing messages: {ex.Message}");
            }
        }

        Console.WriteLine("Message generation and production complete.");

        // Flush and dispose the Kafka producer
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Dispose the Kafka producer when stopping the service
        _producer.Dispose();
        return Task.CompletedTask;
    }
}

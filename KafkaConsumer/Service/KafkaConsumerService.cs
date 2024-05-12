using System;
using System.Threading;
using System.Threading.Tasks;
using Application.MongoDB.Log;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

public class KafkaConsumerService : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly string _topic;
    //private readonly ILogRepository _repository;
    public KafkaConsumerService()
    {
        // Initialize Kafka consumer configuration
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "kafka:9092", // Replace with your Kafka broker endpoint
            GroupId = "logs",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        // Create a new Kafka consumer
        _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();

        // Specify the Kafka topic to subscribe to
        _topic = "log"; // Replace with your Kafka topic
        //_repository = repository;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Subscribe to the Kafka topic
        _consumer.Subscribe(_topic);

        // Start consuming messages in a background task
        return Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Consume a message from the Kafka topic
                    var consumeResult = _consumer.Consume(stoppingToken);

                    // Process the consumed message (in this example, we'll just print it)
                    //await _repository.AddAsync(new()
                    //{
                    //    CreatedAt = DateTime.Now,
                    //    LogData = consumeResult.Message.Value
                    //});
                    Console.WriteLine($"Received message: {consumeResult.Message.Value}");
                }
                catch (OperationCanceledException)
                {
                    // Task was canceled
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while consuming messages: {ex.Message}");
                }
            }
        }, stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        // Unsubscribe and dispose the Kafka consumer when stopping the service
        _consumer.Unsubscribe();
        _consumer.Dispose();
        return base.StopAsync(cancellationToken);
    }
}

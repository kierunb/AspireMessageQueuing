using AspireMessageQueuing.ServiceDefaults;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace WorkerService.Services;

class KafkaService(ILogger<KafkaService> logger, IConfiguration config)
{
    public async Task CreateTopic(string topic)
    {
        logger.LogInformation("Creating Kafka topic: {Topic}", topic);

        using var adminClient = new AdminClientBuilder(new AdminClientConfig
        {
            BootstrapServers = config.GetConnectionString(Constants.KafkaConnectionName)
        }).Build();

        try
        {
            await adminClient.CreateTopicsAsync(
            [
                new TopicSpecification
                {
                    Name = topic,
                    ReplicationFactor = 1,
                    NumPartitions = 4
                }
            ]);

            logger.LogInformation("Kafka topic created: {Topic}", topic);
        }
        catch (AggregateException e)
        {
            logger.LogError(e, "Error creating Kafka topic: {Topic}", topic);
        }

    }
}

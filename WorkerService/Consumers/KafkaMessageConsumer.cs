using Contracts;
using MassTransit;

namespace WorkerService.Consumers;

class KafkaMessageConsumer(ILogger<KafkaMessageConsumer> logger) : IConsumer<KafkaMessage>
{
    public Task Consume(ConsumeContext<KafkaMessage> context)
    {
        logger.LogInformation("Kafka message received. Message: {Message}", context.Message.Message);
        return Task.CompletedTask;
    }
}

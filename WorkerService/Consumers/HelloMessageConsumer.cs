using Contracts;
using MassTransit;

namespace WorkerService.Consumers;

public class HelloMessageConsumer(ILogger<HelloMessageConsumer> logger) : IConsumer<HelloMessage>
{

    public Task Consume(ConsumeContext<HelloMessage> context)
    {
        logger.LogInformation("RabbitMQ message received. Message: {Message}", context.Message.Message);
        return Task.CompletedTask;
    }
}

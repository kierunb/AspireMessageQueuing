using Contracts;
using MassTransit;

namespace WorkerService.Consumers;

public class HelloMessageConsumer : IConsumer<HelloMessage>
{
    private readonly ILogger<HelloMessageConsumer> _logger;

    public HelloMessageConsumer(ILogger<HelloMessageConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<HelloMessage> context)
    {
        _logger.LogInformation("Message received. Message: {Message}", context.Message.Message);
        return Task.CompletedTask;
    }
}

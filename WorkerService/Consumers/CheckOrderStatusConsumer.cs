using Contracts;
using MassTransit;

namespace WorkerService.Consumers;

public class CheckOrderStatusConsumer : IConsumer<CheckOrderStatus>
{
    public async Task Consume(ConsumeContext<CheckOrderStatus> context)
    {
        await context.RespondAsync(new OrderStatusResult
        {
            OrderId = context.Message.OrderId,
            StatusCode = 200,
            StatusText = "OK",
            Timestamp = DateTime.UtcNow
        });
    }
}

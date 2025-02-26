using System.Threading;
using Confluent.Kafka;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WebApiMessageQueues.Extensions;

namespace WebApiMessageQueues.Endpoints;

public record CheckOrderStatusRequest(string OrderId);

public class CheckOrderStatusEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/check-order-status",
            async (
                [FromServices] IRequestClient<CheckOrderStatus> requestClient,
                [FromBody] CheckOrderStatusRequest request
            ) =>
            {
                var response = await requestClient.GetResponse<OrderStatusResult>(
                    new CheckOrderStatus { OrderId = request.OrderId }
                );

                return TypedResults.Ok(response.Message);
            }
        );
    }
}

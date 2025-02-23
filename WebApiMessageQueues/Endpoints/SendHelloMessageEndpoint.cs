using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WebApiMessageQueues.Extensions;

namespace WebApiMessageQueues.Endpoints;

public record SendHelloMessageRequest(string Message);

public class SendHelloMessageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/send-hello",
            async (
                [FromServices] IPublishEndpoint publishEndpoint,
                SendHelloMessageRequest request
            ) =>
            {
                await publishEndpoint.Publish(new HelloMessage { Message = request.Message });

                return Results.Accepted();
            }
        );
    }
}

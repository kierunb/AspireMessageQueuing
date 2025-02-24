using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WebApiMessageQueues.Extensions;

namespace WebApiMessageQueues.Endpoints;

public record SendKafkaRequest(string Message);

public class SendKafkaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/send-kafka",
            async (
                [FromServices] ITopicProducer<KafkaMessage> producer,
                [FromBody] SendKafkaRequest request
            ) =>
            {
                await producer.Produce(new KafkaMessage
                {
                    Message = request.Message
                });

                return Results.Accepted();
            }
        );
    }
}

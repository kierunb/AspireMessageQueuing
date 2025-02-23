using WebApiMessageQueues.Extensions;

namespace WebApiMessageQueues.Endpoints;

public class HelloEnpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/hello", () => "Hello, World!");
    }
}

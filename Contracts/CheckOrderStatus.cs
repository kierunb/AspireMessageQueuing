namespace Contracts;

public record CheckOrderStatus
{
    public string OrderId { get; init; } = string.Empty;
}

public record OrderStatusResult
{
    public string OrderId { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; } 
    public short StatusCode { get; init; }
    public string StatusText { get; init; } = string.Empty;
}

namespace Contracts;

public record KafkaMessage
{
    public string Message { get; init; } = string.Empty;
}

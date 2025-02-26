namespace AspireMessageQueuing.ServiceDefaults;

public static class Constants
{
    public const string RabbitMQConnectionName = "rabbit-mq";
    public const string KafkaConnectionName = "kafka";

    public const string KafkaTopicName = "topic1";
    public const string KafkaGroupId = "group1";

    public const bool WithRabbitMQ = true;
    public const bool WithKafka = true;
}

namespace LinFx.Extensions.EventBus.Kafka;

public class KafkaEventBusOptions
{
    public string ConnectionName { get; set; }

    public string TopicName { get; set; } = string.Empty;

    public string GroupId { get; set; } = string.Empty;
}

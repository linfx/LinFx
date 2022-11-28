using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace LinFx.Extensions.Kafka;

public class KafkaOptions
{
    public KafkaConnections Connections { get; } = new KafkaConnections();

    public Action<ProducerConfig> ConfigureProducer { get; set; }

    public Action<ConsumerConfig> ConfigureConsumer { get; set; }

    public Action<TopicSpecification> ConfigureTopic { get; set; }
}

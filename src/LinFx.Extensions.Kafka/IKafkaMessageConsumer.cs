using Confluent.Kafka;

namespace LinFx.Extensions.Kafka;

public interface IKafkaMessageConsumer
{
    void OnMessageReceived(Func<Message<string, byte[]>, Task> callback);
}

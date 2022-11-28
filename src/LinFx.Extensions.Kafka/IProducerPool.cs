using Confluent.Kafka;

namespace LinFx.Extensions.Kafka;

public interface IProducerPool : IDisposable
{
    IProducer<string, byte[]> Get(string connectionName = null);
}

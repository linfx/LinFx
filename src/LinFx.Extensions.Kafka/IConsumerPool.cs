using System;
using Confluent.Kafka;

namespace LinFx.Extensions.Kafka;

public interface IConsumerPool : IDisposable
{
    IConsumer<string, byte[]> Get(string groupId, string connectionName = null);
}

using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Kafka;

public class KafkaMessageConsumerFactory : IKafkaMessageConsumerFactory, ISingletonDependency, IDisposable
{
    protected IServiceScope ServiceScope { get; }

    public KafkaMessageConsumerFactory(IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScope = serviceScopeFactory.CreateScope();
    }

    public IKafkaMessageConsumer Create(string topicName, string groupId, string? connectionName = null)
    {
        var consumer = ServiceScope.ServiceProvider.GetRequiredService<KafkaMessageConsumer>();
        consumer.Initialize(topicName, groupId, connectionName);
        return consumer;
    }

    public void Dispose()
    {
        ServiceScope?.Dispose();
    }
}

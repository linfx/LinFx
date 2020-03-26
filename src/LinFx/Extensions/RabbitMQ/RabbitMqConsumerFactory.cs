using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.RabbitMq
{
    public class RabbitMqConsumerFactory : IConsumerFactory, IDisposable
    {
        protected IServiceScope ServiceScope { get; }

        public RabbitMqConsumerFactory(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScope = serviceScopeFactory.CreateScope();
        }

        public IRabbitMqConsumer Create(ExchangeDeclareConfiguration exchange, QueueDeclareConfiguration queue, string connectionName = default)
        {
            var consumer = ServiceScope.ServiceProvider.GetRequiredService<RabbitMqConsumer>();
            consumer.Initialize(exchange, queue, connectionName);
            return consumer;
        }

        public void Dispose()
        {
            ServiceScope?.Dispose();
        }
    }
}

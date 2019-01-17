using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.RabbitMQ
{
    public class DefaultRabbitMqMessageConsumerFactory : IRabbitMqMessageConsumerFactory, IDisposable
    {
        protected IServiceScope ServiceScope { get; }

        public DefaultRabbitMqMessageConsumerFactory(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScope = serviceScopeFactory.CreateScope();
        }

        public IRabbitMqMessageConsumer Create(
            ExchangeDeclareConfiguration exchange,
            QueueDeclareConfiguration queue,
            string connectionName = null)
        {
            var consumer = ServiceScope.ServiceProvider.GetRequiredService<DefaultRabbitMqMessageConsumer>();
            consumer.Initialize(exchange, queue, connectionName);
            return consumer;
        }

        public void Dispose()
        {
            ServiceScope?.Dispose();
        }
    }
}

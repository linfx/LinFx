using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.RabbitMQ
{
    public class ConsumerFactory : IConsumerFactory, IDisposable
    {
        protected IServiceScope ServiceScope { get; }

        public ConsumerFactory(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScope = serviceScopeFactory.CreateScope();
        }

        public IConsumer Create(
            ExchangeDeclareConfiguration exchange,
            QueueDeclareConfiguration queue,
            string connectionName = null)
        {
            var consumer = ServiceScope.ServiceProvider.GetRequiredService<Consumer>();
            consumer.Initialize(exchange, queue, connectionName);
            return consumer;
        }

        public void Dispose()
        {
            ServiceScope?.Dispose();
        }
    }
}

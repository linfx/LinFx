﻿using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.RabbitMQ;

/// <summary>
/// 消费者工厂
/// </summary>
public class RabbitMqMessageConsumerFactory : IRabbitMqMessageConsumerFactory, IDisposable
{
    protected IServiceScope ServiceScope { get; }

    public RabbitMqMessageConsumerFactory(IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScope = serviceScopeFactory.CreateScope();
    }

    public IRabbitMqMessageConsumer Create(ExchangeDeclareConfiguration exchange, QueueDeclareConfiguration queue, string connectionName)
    {
        var consumer = ServiceScope.ServiceProvider.GetRequiredService<RabbitMqMessageConsumer>();
        consumer.Initialize(exchange, queue, connectionName);
        return consumer;
    }

    public void Dispose()
    {
        ServiceScope?.Dispose();
    }
}

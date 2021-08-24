using System;

namespace LinFx.Extensions.EventBus.RabbitMq
{
    public interface IEventNameProvider
    {
        string GetName(Type eventType);
    }
}
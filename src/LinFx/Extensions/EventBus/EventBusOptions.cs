using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.EventBus
{
    public class EventBusOptions
    {
        public string BrokerName { get; set; }

        public string QueueName { get; set; }

        public int RetryCount { get; set; } = 3;

        public int FailCount { get; set; } = 3;

        public bool Durable { get; set; }

        public bool AutoDelete { get; set; }

        public int PrefetchCount { get; set; } = 1;

        public Action<ILinFxBuilder, EventBusOptionsBuilder> ConfigureEventBus { get; set; }
    }
}

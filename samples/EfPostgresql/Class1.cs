using LinFx.Extensions.EventBus;
using LinFx.Extensions.EventBus.Distributed;
using LinFx.Extensions.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfPostgresql
{
    internal class Class1 : BackgroundService
    {
        IDistributedEventBus eventBus;

        public Class1(IDistributedEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await eventBus.PublishAsync("Hello World!");


            Console.WriteLine("fff");
        }
    }
}

using LinFx.Test.Extensions.EventBus.EventHandling;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Xunit;
using LinFx.Test.EventBus.Events;
using LinFx.Extensions.EventBus;

namespace LinFx.Test.Extensions.EventBus
{
    public class RabbitMqDistributedEventBusTests
    {
        private readonly ServiceProvider _services;

        public RabbitMqDistributedEventBusTests()
        {
            var services = new ServiceCollection();
            services
                .AddLinFx()
                .AddEventBus(options =>
                {
                    options.UseRabbitMq(x =>
                    {
                        x.Connections.Default.HostName = "127.0.0.1";
                        x.Connections.Default.UserName = "admin";
                        x.Connections.Default.Password = "admin.123456";
                        x.ExchangeName = "linfx_event_bus";
                        x.ClientName = "linfx_event_queue";
                    });
                });

            services.AddTransient<OrderStatusChangedToAwaitingValidationEventHandler>();
            _services = services.BuildServiceProvider();
        }


        [Fact]
        public async Task Should_Call_Handler_On_Event_With_Correct_SourceAsync()
        {
            var eventBus = _services.GetRequiredService<IEventBus>();

            //ConfigureEventBus
            //_eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();

            var orderId = Guid.NewGuid().GetHashCode() & ushort.MaxValue;
            var evt = new OrderStatusChangedToAwaitingValidationEvent(orderId, new List<OrderStockItem>
            {
                new OrderStockItem(1000, 1)
            });
            await eventBus.PublishAsync(evt);
        }
    }
}

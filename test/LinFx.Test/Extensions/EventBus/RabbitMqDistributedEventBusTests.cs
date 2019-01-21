using LinFx.Test.Extensions.EventBus.Events;
using LinFx.Test.Extensions.EventBus.EventHandling;
using LinFx.Extensions.EventBus;
using LinFx.Extensions.EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Xunit;

namespace LinFx.Test.Extensions.EventBus
{
    public class RabbitMqDistributedEventBusTests
    {
        private readonly IEventBus _eventBus;

        public RabbitMqDistributedEventBusTests()
        {
            var services = new ServiceCollection();

            services.AddLinFx()
                .AddEventBus(builder =>
                {
                    builder.Configure(options =>
                    {
                        options.RetryCount = 3;
                    })
                    .UseRabbitMQ(options =>
                    {
                        options.Host = "14.21.34.85";
                        options.UserName = "admin";
                        options.Password = "admin.123456";
                        options.QueueName = "linfx_event_queue";
                        options.ExchangeName = "linfx_event_bus";
                    });
                });

            //services
            services.AddTransient<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            //services.AddTransient<OrderStatusChangedToPaidIntegrationEventHandler>();

            var applicationServices = services.BuildServiceProvider();

            //ConfigureEventBus
            _eventBus = applicationServices.GetRequiredService<IEventBus>();
            _eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
        }


        [Fact]
        public async Task Should_Call_Handler_On_Event_With_Correct_SourceAsync()
        {
            var orderId = Guid.NewGuid().GetHashCode() & ushort.MaxValue;
            var evt = new OrderStatusChangedToAwaitingValidationIntegrationEvent(orderId, new List<OrderStockItem>
            {
            });
            await _eventBus.PublishAsync(evt);
            await Task.Delay(100000);
        }
    }
}

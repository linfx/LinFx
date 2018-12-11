using LinFx.Extensions.EventBus.Abstractions;
using LinFx.Test.Extensions.EventBus.Events;
using LinFx.Utils;
using LinFx.Extensions.EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using LinFx.Test.Extensions.EventBus.EventHandling;
using System.Collections.Generic;
using System;

namespace LinFx.Test.Extensions.EventBus
{
    public class EventBusRabbitMQTest
    {
        private readonly IEventBus _eventBus;

        public EventBusRabbitMQTest()
        {
            var services = new ServiceCollection();

            services.AddLinFx()
                .AddEventBus(options =>
                {
                    options.Durable = true;
                    options.BrokerName = "tc_cloud_event_bus";
                    options.QueueName = "tc_cloud_process_queue";
                    options.ConfigureEventBus = (fx, builder) => builder.UseRabbitMQ(fx, x =>
                    {
                        x.Host = "14.21.34.85";
                        x.UserName = "admin";
                        x.Password = "admin.123456";
                    });
                });

            //services
            services.AddTransient<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            //services.AddTransient<OrderStatusChangedToPaidIntegrationEventHandler>();

            var applicationServices = services.BuildServiceProvider();

            //ConfigureEventBus
            _eventBus = applicationServices.GetRequiredService<IEventBus>();
            _eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            //eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
        }


        [Fact]
        public async Task Should_Call_Handler_On_Event_With_Correct_SourceAsync()
        {
            var orderId = Guid.NewGuid().GetHashCode() & ushort.MaxValue;
            var evt = new OrderStatusChangedToAwaitingValidationIntegrationEvent(orderId, new List<OrderStockItem>
            {
            });
            await _eventBus.PublishAsync(evt);

            //for (int i = 0; i < 2; i++)
            //{
            //    await _eventBus.PublishAsync(new ClientCreateIntergrationEvent
            //    {
            //        ClientId = IDUtils.CreateNewId().ToString(),
            //        ClientSecrets = new[] { "191d437f0cc3463b85669f2b570cdc21" },
            //        AllowedGrantTypes = new[] { "client_credentials" },
            //        AllowedScopes = new[] { "api3.device" }
            //    });
            //}
        }
    }
}

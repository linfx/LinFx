using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.Test.MassTransit.MediatR
{
    /// <summary>
    /// 中介者模式测试
    /// </summary>
    public class MediatRTests
    {
        [Fact]
        public async Task RequestTestAsync()
        {
            var services = new ServiceCollection();
            services.AddMassTransit(x =>
            {
                x.AddConsumers(GetType().Assembly);

                //x.AddMediator((provider, cfg) =>
                //{
                //    cfg.ConfigureConsumer(provider);
                //});

                x.AddRequestClient<GetOrderStatus>();
            });
            var container = services.BuildServiceProvider();

            var client = container.GetService<IRequestClient<GetOrderStatus>>();
            var response = await client.GetResponse<OrderStatus>(new { OrderNumber = "90210" });

            Assert.Equal("Pending", response.Message.Status);
        }
    }

    public class SubmitOrder
    {
        public string OrderNumber { get; }
    }

    public class GetOrderStatus
    {
        public string OrderNumber { get; set; }
    }

    public class OrderStatus
    {
        public string OrderNumber { get; set; }

        public string Status { get; set; }
    }

    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        public Task Consume(ConsumeContext<SubmitOrder> context)
        {
            // ... do the work ...
            return Task.Run(() => Console.WriteLine("Hello World!"));
        }
    }

    public class OrderStatusConsumer : IConsumer<GetOrderStatus>
    {
        public async Task Consume(ConsumeContext<GetOrderStatus> context)
        {
            await context.RespondAsync<OrderStatus>(new { context.Message.OrderNumber, Status = "Pending" });
        }
    }
}

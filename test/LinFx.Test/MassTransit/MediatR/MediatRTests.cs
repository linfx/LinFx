using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.Test.MassTransit.MediatR
{
    public class MediatRTests
    {
        [Fact]
        public async Task StartupAsync()
        {
            //IMediator
            var services = new ServiceCollection();
            //services.AddLinFx();
            services.AddMassTransit(x =>
            {
                x.AddConsumers(GetType().Assembly);
                x.AddMediator();
            });
            var container = services.BuildServiceProvider();


            var mediator = Bus.Factory.CreateMediator(cfg =>
            {
                cfg.Consumer<SubmitOrderConsumer>();
                cfg.Consumer<OrderStatusConsumer>();
            });

            var client = mediator.CreateRequestClient<IGetOrderStatus>();


            var response = await client.GetResponse<IOrderStatus>(new { OrderNumber = "90210" });
            Console.WriteLine("Order Status: {0}", response.Message.Status);
        }
    }

    public interface ISubmitOrder
    {
        string OrderNumber { get; }
    }

    public interface IGetOrderStatus
    {
        string OrderNumber { get; }
    }

    public interface IOrderStatus
    {
        string OrderNumber { get; }
        string Status { get; }
    }

    public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
    {
        public Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            // ... do the work ...
            return Task.Run(() => Console.WriteLine("Hello World!"));
        }
    }

    public class OrderStatusConsumer : IConsumer<IGetOrderStatus>
    {
        public async Task Consume(ConsumeContext<IGetOrderStatus> context)
        {
            await context.RespondAsync<IOrderStatus>(new { context.Message.OrderNumber, Status = "Pending" });
        }
    }
}

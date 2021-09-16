using Automatonymous;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.Test.MassTransit.Automatonymous
{
    public class OrderStateMachineTests
    {
        private readonly OrderStateMachine machine = new OrderStateMachine();

        [Fact]
        public async Task Should_Be_FriendAsync()
        {
            var instance = new OrderState();

            var orderSubmitted = new OrderSubmitted
            {
                OrderId = "123",
                CustomerId = "ABC",
                ReceiveTimestamp = DateTime.UtcNow
            };

            await machine.RaiseEvent(instance, machine.OrderSubmission, orderSubmitted);

            await machine.RaiseEvent(instance, machine.OrderCancellationRequested);
        }
    }

    class OrderStateMachine : AutomatonymousStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            // 定义当前 状态属性 
            InstanceState(x => x.CurrentState);
        }

        public State Submitted { get; private set; }
        public State Accepted { get; private set; }

        public Event<OrderSubmitted> OrderSubmission { get; private set; }
        public Event OrderCancellationRequested { get; private set; }
    }

    class OrderState
    {
        public State CurrentState { get; set; }
    }

    class OrderSubmitted
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime ReceiveTimestamp { get; set; }
    }
}

using LinFx.Extensions.EventBus.Abstractions;
using LinFx.Test.EventBus.Events;
using System.Threading.Tasks;

namespace LinFx.Test.Extensions.EventBus.EventHandling
{
    public class OrderStatusChangedToAwaitingValidationEventHandler : IEventHandler<OrderStatusChangedToAwaitingValidationEvent>
    {
        public Task HandleAsync(OrderStatusChangedToAwaitingValidationEvent evt)
        {
            return Task.CompletedTask;
        }
    }
}

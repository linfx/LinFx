using LinFx.Extensions.EventBus;
using LinFx.Test.Extensions.EventBus.Events;
using System.Threading.Tasks;

namespace LinFx.Test.Extensions.EventBus.EventHandling
{
    public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler :
        IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
    {
        public Task HandleAsync(OrderStatusChangedToAwaitingValidationIntegrationEvent evt)
        {
            return Task.CompletedTask;
        }
    }
}

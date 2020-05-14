using LinFx.Test.EventBus.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Test.Domain.EventHandlers
{
    public class ValidateOrAddBuyerWhenOrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {
        public Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

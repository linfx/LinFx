using LinFx.Extensions.EventBus.Events;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent evt);
    }

    public interface IIntegrationEventHandler
    {
    }
}

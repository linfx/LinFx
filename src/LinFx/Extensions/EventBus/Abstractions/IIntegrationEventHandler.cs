using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus
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

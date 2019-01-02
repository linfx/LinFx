using LinFx.Extensions.EventBus.Events;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent evt);

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        //void SubscribeDynamic<TH>(string eventName)
        //    where TH : IDynamicIntegrationEventHandler;

        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;
    }
}

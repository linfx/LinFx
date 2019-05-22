using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus
{
    public interface IEventBus
    {
        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        Task PublishAsync(IntegrationEvent evt, string routingKey = default);

        /// <summary>
        /// Registers to an event.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        void Subscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        void Unsubscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>;
    }
}

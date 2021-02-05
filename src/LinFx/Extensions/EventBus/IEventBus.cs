using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        Task PublishAsync(IEvent evt, string routingKey = default);

        /// <summary>
        /// Registers to an event.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        void Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        void Unsubscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>;
    }
}

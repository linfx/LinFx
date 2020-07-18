using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.Abstractions
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IEvent
    {
        Task HandleAsync(TEvent evt);
    }

    public interface IEventHandler
    {
    }
}

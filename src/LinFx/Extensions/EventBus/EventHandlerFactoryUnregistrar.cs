namespace LinFx.Extensions.EventBus;

/// <summary>
/// Used to unregister a <see cref="IEventHandlerFactory"/> on <see cref="Dispose"/> method.
/// </summary>
public class EventHandlerFactoryUnregistrar(IEventBus eventBus, Type eventType, IEventHandlerFactory factory) : IDisposable
{
    private readonly IEventBus _eventBus = eventBus;
    private readonly Type _eventType = eventType;
    private readonly IEventHandlerFactory _factory = factory;

    public void Dispose()
    {
        _eventBus.Unsubscribe(_eventType, _factory);
    }
}
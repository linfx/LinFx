namespace LinFx.Extensions.EventBus;

public class EventHandlerDisposeWrapper(IEventHandler eventHandler, Action? disposeAction = default) : IEventHandlerDisposeWrapper
{
    public IEventHandler EventHandler { get; } = eventHandler;

    private readonly Action? _disposeAction = disposeAction;

    public void Dispose()
    {
        _disposeAction?.Invoke();
    }
}
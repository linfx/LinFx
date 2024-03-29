using LinFx.Extensions.EventBus.Local;

namespace LinFx.Extensions.EventBus.Distributed;

public sealed class NullDistributedEventBus : IDistributedEventBus
{
    public static NullDistributedEventBus Instance { get; } = new NullDistributedEventBus();

    private NullDistributedEventBus() { }

    public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class => NullDisposable.Instance;

    public IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class => NullDisposable.Instance;

    public IDisposable Subscribe<TEvent, THandler>() where TEvent : class where THandler : IEventHandler, new() => NullDisposable.Instance;

    public IDisposable Subscribe(Type eventType, IEventHandler handler) => NullDisposable.Instance;

    public IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class => NullDisposable.Instance;

    public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory) => NullDisposable.Instance;

    public void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class { }

    public void Unsubscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class { }

    public void Unsubscribe(Type eventType, IEventHandler handler) { }

    public void Unsubscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class { }

    public void Unsubscribe(Type eventType, IEventHandlerFactory factory) { }

    public void UnsubscribeAll<TEvent>() where TEvent : class { }

    public void UnsubscribeAll(Type eventType) { }

    public Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true) where TEvent : class => Task.CompletedTask;

    public Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true) => Task.CompletedTask;
}

using LinFx.Collections;
using LinFx.Extensions.EventBus.Local;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace LinFx.Extensions.EventBus.Distributed;

public class LocalDistributedEventBus : IDistributedEventBus
{
    private readonly ILocalEventBus _localEventBus;

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected DistributedEventBusOptions DistributedEventBusOptions { get; }

    public LocalDistributedEventBus(
        ILocalEventBus localEventBus,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<DistributedEventBusOptions> distributedEventBusOptions)
    {
        _localEventBus = localEventBus;
        ServiceScopeFactory = serviceScopeFactory;
        DistributedEventBusOptions = distributedEventBusOptions.Value;
        Subscribe(distributedEventBusOptions.Value.Handlers);
    }

    /// <summary>
    /// 订阅
    /// </summary>
    /// <param name="handlers"></param>
    public virtual void Subscribe(ITypeList<IEventHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            var interfaces = handler.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(@interface))
                    continue;

                var genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    Subscribe(genericArgs[0], new IocEventHandlerFactory(ServiceScopeFactory, handler));
                }
            }
        }
    }

    public virtual IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class => Subscribe(typeof(TEvent), handler);

    public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class => _localEventBus.Subscribe(action);

    public IDisposable Subscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class => _localEventBus.Subscribe(handler);

    public IDisposable Subscribe<TEvent, THandler>() where TEvent : class where THandler : IEventHandler, new() => _localEventBus.Subscribe<TEvent, THandler>();

    public IDisposable Subscribe(Type eventType, IEventHandler handler) => _localEventBus.Subscribe(eventType, handler);

    public IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class => _localEventBus.Subscribe<TEvent>(factory);

    public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory) => _localEventBus.Subscribe(eventType, factory);

    public void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class => _localEventBus.Unsubscribe(action);

    public void Unsubscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class => _localEventBus.Unsubscribe(handler);

    public void Unsubscribe(Type eventType, IEventHandler handler) => _localEventBus.Unsubscribe(eventType, handler);

    public void Unsubscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class => _localEventBus.Unsubscribe<TEvent>(factory);

    public void Unsubscribe(Type eventType, IEventHandlerFactory factory) => _localEventBus.Unsubscribe(eventType, factory);

    public void UnsubscribeAll<TEvent>() where TEvent : class => _localEventBus.UnsubscribeAll<TEvent>();

    public void UnsubscribeAll(Type eventType) => _localEventBus.UnsubscribeAll(eventType);

    public Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true) where TEvent : class => _localEventBus.PublishAsync(eventData, onUnitOfWorkComplete);

    public Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true) => _localEventBus.PublishAsync(eventType, eventData, onUnitOfWorkComplete);
}
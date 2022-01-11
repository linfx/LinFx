using LinFx.Collections;
using LinFx.Extensions.EventBus.Distributed;
using LinFx.Extensions.EventBus.Local;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Uow;
using LinFx.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus;

public abstract class EventBusBase : IEventBus
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected IEventErrorHandler ErrorHandler { get; }

    protected EventBusBase(
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IEventErrorHandler errorHandler)
    {
        ServiceScopeFactory = serviceScopeFactory;
        CurrentTenant = currentTenant;
        UnitOfWorkManager = unitOfWorkManager;
        ErrorHandler = errorHandler;
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {
        return Subscribe(typeof(TEvent), new ActionEventHandler<TEvent>(action));
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent, THandler>()
        where TEvent : class
        where THandler : IEventHandler, new()
    {
        return Subscribe(typeof(TEvent), new TransientEventHandlerFactory<THandler>());
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe(Type eventType, IEventHandler handler)
    {
        return Subscribe(eventType, new SingleInstanceHandlerFactory(handler));
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {
        return Subscribe(typeof(TEvent), factory);
    }

    public abstract IDisposable Subscribe(Type eventType, IEventHandlerFactory factory);

    public abstract void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class;

    /// <inheritdoc/>
    public virtual void Unsubscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
    {
        Unsubscribe(typeof(TEvent), handler);
    }

    public abstract void Unsubscribe(Type eventType, IEventHandler handler);

    /// <inheritdoc/>
    public virtual void Unsubscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {
        Unsubscribe(typeof(TEvent), factory);
    }

    public abstract void Unsubscribe(Type eventType, IEventHandlerFactory factory);

    /// <inheritdoc/>
    public virtual void UnsubscribeAll<TEvent>() where TEvent : class
    {
        UnsubscribeAll(typeof(TEvent));
    }

    /// <inheritdoc/>
    public abstract void UnsubscribeAll(Type eventType);

    /// <inheritdoc/>
    public Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true) where TEvent : class
    {
        return PublishAsync(typeof(TEvent), eventData, onUnitOfWorkComplete);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true)
    {
        if (onUnitOfWorkComplete && UnitOfWorkManager.Current != null)
        {
            AddToUnitOfWork(
                UnitOfWorkManager.Current,
                new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext())
            );
            return;
        }

        await PublishToEventBusAsync(eventType, eventData);
    }

    protected abstract Task PublishToEventBusAsync(Type eventType, object eventData);

    protected abstract void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord);

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="eventData"></param>
    /// <param name="onErrorAction"></param>
    /// <returns></returns>
    public virtual async Task TriggerHandlersAsync(Type eventType, object eventData, Action<EventExecutionErrorContext> onErrorAction = null)
    {
        var exceptions = new List<Exception>();

        await TriggerHandlersAsync(eventType, eventData, exceptions);

        if (exceptions.Any())
        {
            var context = new EventExecutionErrorContext(exceptions, eventType, this);
            onErrorAction?.Invoke(context);
            //await ErrorHandler.HandleAsync(context);
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="eventData"></param>
    /// <param name="exceptions"></param>
    /// <returns></returns>
    protected virtual async Task TriggerHandlersAsync(Type eventType, object eventData, List<Exception> exceptions)
    {
        // �������������ã���ͬ�� ConfigureAwait(false) ��
        // ������Բο� https://blogs.msdn.microsoft.com/benwilli/2017/02/09/an-alternative-to-configureawaitfalse-everywhere/��
        await new SynchronizationContextRemover();

        // �����¼������ͣ��õ����������¼�������������
        foreach (var handlerFactories in GetHandlerFactories(eventType))
        {
            // �������е��¼�������������ͨ�� Factory ����¼������������� Handler �� HandleEventAsync ������
            foreach (var handlerFactory in handlerFactories.EventHandlerFactories)
            {
                await TriggerHandlerAsync(handlerFactory, handlerFactories.EventType, eventData, exceptions);
            }
        }

        // ������ͼ̳��� IEventDataWithInheritableGenericArgument �ӿڣ���ô���ⷺ�Ͳ����Ƿ��и��ࡣ
        // ����и��࣬���ʹ�õ�ǰ���¼����ݣ�Ϊ�丸�෢��һ���¼���
        // Implements generic argument inheritance. See IEventDataWithInheritableGenericArgument
        if (eventType.GetTypeInfo().IsGenericType &&
            eventType.GetGenericArguments().Length == 1 &&
            typeof(IEventDataWithInheritableGenericArgument).IsAssignableFrom(eventType))
        {
            var genericArg = eventType.GetGenericArguments()[0];
            var baseArg = genericArg.GetTypeInfo().BaseType;
            if (baseArg != null)
            {
                // ���������¼����ͣ�ʹ�õ�ǰһ���ķ��Ͷ��壬ֻ�Ƿ��Ͳ���ʹ�û��ࡣ
                var baseEventType = eventType.GetGenericTypeDefinition().MakeGenericType(baseArg);
                var constructorArgs = ((IEventDataWithInheritableGenericArgument)eventData).GetConstructorArgs();
                var baseEventData = Activator.CreateInstance(baseEventType, constructorArgs);
                await PublishToEventBusAsync(baseEventType, baseEventData);
            }
        }
    }

    protected virtual void SubscribeHandlers(ITypeList<IEventHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            var interfaces = handler.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(@interface))
                {
                    continue;
                }

                var genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    Subscribe(genericArgs[0], new IocEventHandlerFactory(ServiceScopeFactory, handler));
                }
            }
        }
    }

    protected abstract IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType);

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="asyncHandlerFactory"></param>
    /// <param name="eventType"></param>
    /// <param name="eventData"></param>
    /// <param name="exceptions"></param>
    /// <returns></returns>
    protected virtual async Task TriggerHandlerAsync(IEventHandlerFactory asyncHandlerFactory, Type eventType, object eventData, List<Exception> exceptions)
    {
        using var eventHandlerWrapper = asyncHandlerFactory.GetHandler();
        try
        {
            // ����¼������������͡�
            var handlerType = eventHandlerWrapper.EventHandler.GetType();

            using (CurrentTenant.Change(GetEventDataTenantId(eventData)))
            {
                // �ж��¼��������Ǳ����¼����Ƿֲ�ʽ�¼���
                if (ReflectionHelper.IsAssignableToGenericType(handlerType, typeof(ILocalEventHandler<>)))
                {
                    // ��÷������塣
                    var method = typeof(ILocalEventHandler<>)
                        .MakeGenericType(eventType)
                        .GetMethod(
                            nameof(ILocalEventHandler<object>.HandleEventAsync),
                            new[] { eventType }
                        );

                    // ʹ�ù���������ʵ�����÷�����
                    await (Task)method.Invoke(eventHandlerWrapper.EventHandler, new[] { eventData });
                }
                else if (ReflectionHelper.IsAssignableToGenericType(handlerType, typeof(IDistributedEventHandler<>)))
                {
                    // ��÷������塣
                    var method = typeof(IDistributedEventHandler<>)
                        .MakeGenericType(eventType)
                        .GetMethod(
                            nameof(IDistributedEventHandler<object>.HandleEventAsync),
                            new[] { eventType }
                        );

                    // ʹ�ù���������ʵ�����÷�����
                    await (Task)method.Invoke(eventHandlerWrapper.EventHandler, new[] { eventData });
                }
                else
                {
                    // ��������ǣ���˵�����Ͳ���ȷ���׳��쳣��
                    throw new Exception("The object instance is not an event handler. Object type: " + handlerType.AssemblyQualifiedName);
                }
            }
        }
        catch (TargetInvocationException ex)
        {
            exceptions.Add(ex.InnerException);
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }
    }

    protected virtual string GetEventDataTenantId(object eventData)
    {
        return eventData switch
        {
            IMultiTenant multiTenantEventData => multiTenantEventData.TenantId,
            IEventDataMayHaveTenantId eventDataMayHaveTenantId when eventDataMayHaveTenantId.IsMultiTenant(out var tenantId) => tenantId,
            _ => CurrentTenant.Id
        };
    }

    protected class EventTypeWithEventHandlerFactories
    {
        public Type EventType { get; }

        public List<IEventHandlerFactory> EventHandlerFactories { get; }

        public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
        {
            EventType = eventType;
            EventHandlerFactories = eventHandlerFactories;
        }
    }

    // Reference from
    // https://blogs.msdn.microsoft.com/benwilli/2017/02/09/an-alternative-to-configureawaitfalse-everywhere/
    protected struct SynchronizationContextRemover : INotifyCompletion
    {
        public bool IsCompleted
        {
            get { return SynchronizationContext.Current == null; }
        }

        public void OnCompleted(Action continuation)
        {
            var prevContext = SynchronizationContext.Current;
            try
            {
                SynchronizationContext.SetSynchronizationContext(null);
                continuation();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(prevContext);
            }
        }

        public SynchronizationContextRemover GetAwaiter()
        {
            return this;
        }

        public void GetResult()
        {
        }
    }
}

﻿using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.EventBus.Distributed;
using LinFx.Extensions.Guids;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.RabbitMQ;
using LinFx.Extensions.Timing;
using LinFx.Extensions.Uow;
using LinFx.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;

namespace LinFx.Extensions.EventBus.RabbitMQ;

/* TODO: How to handle unsubscribe to unbind on RabbitMq (may not be possible for)
 */
/// <summary>
/// RabbitMQ 分步式事件总线
/// </summary>
[Service(ReplaceServices = true)]
[ExposeServices(typeof(IDistributedEventBus), typeof(RabbitMqDistributedEventBus))]
public class RabbitMqDistributedEventBus : DistributedEventBusBase, ISingletonDependency
{
    protected RabbitMqEventBusOptions RabbitMqEventBusOptions { get; }
    protected IConnectionPool ConnectionPool { get; }
    protected IRabbitMqSerializer Serializer { get; }

    //TODO: Accessing to the List<IEventHandlerFactory> may not be thread-safe!
    protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
    protected ConcurrentDictionary<string, Type> EventTypes { get; }
    protected IRabbitMqMessageConsumerFactory MessageConsumerFactory { get; }
    protected IRabbitMqMessageConsumer Consumer { get; private set; }

    private bool _exchangeCreated;

    public RabbitMqDistributedEventBus(
        IOptions<RabbitMqEventBusOptions> options,
        IConnectionPool connectionPool,
        IRabbitMqSerializer serializer,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<DistributedEventBusOptions> distributedEventBusOptions,
        IRabbitMqMessageConsumerFactory messageConsumerFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IGuidGenerator guidGenerator,
        IClock clock)
        : base(
            serviceScopeFactory,
            currentTenant,
            unitOfWorkManager,
            distributedEventBusOptions,
            guidGenerator,
            clock)
    {
        ConnectionPool = connectionPool;
        Serializer = serializer;
        MessageConsumerFactory = messageConsumerFactory;
        RabbitMqEventBusOptions = options.Value;

        HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        EventTypes = new ConcurrentDictionary<string, Type>();
    }

    public void Initialize()
    {
        Consumer = MessageConsumerFactory.Create(
            new ExchangeDeclareConfiguration(
                RabbitMqEventBusOptions.ExchangeName,
                type: RabbitMqEventBusOptions.GetExchangeTypeOrDefault(),
                durable: true
            ),
            new QueueDeclareConfiguration(
                RabbitMqEventBusOptions.ClientName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                prefetchCount: RabbitMqEventBusOptions.PrefetchCount
            ),
            RabbitMqEventBusOptions.ConnectionName
        );

        Consumer.OnMessageReceived(ProcessEventAsync);

        SubscribeHandlers(DistributedEventBusOptions.Handlers);
    }

    private async Task ProcessEventAsync(IModel channel, BasicDeliverEventArgs ea)
    {
        var eventName = ea.RoutingKey;
        var eventType = EventTypes.GetOrDefault(eventName);
        if (eventType == null)
        {
            return;
        }

        var eventBytes = ea.Body.ToArray();

        //if (await AddToInboxAsync(ea.BasicProperties.MessageId, eventName, eventType, eventBytes))
        //{
        //    return;
        //}

        var eventData = Serializer.Deserialize(eventBytes, eventType);

        await TriggerHandlersAsync(eventType, eventData);
    }

    public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        var handlerFactories = GetOrCreateHandlerFactories(eventType);

        if (factory.IsInFactories(handlerFactories))
        {
            return NullDisposable.Instance;
        }

        handlerFactories.Add(factory);

        if (handlerFactories.Count == 1) //TODO: Multi-threading!
        {
            Consumer.BindAsync(EventNameAttribute.GetNameOrDefault(eventType));
        }

        return new EventHandlerFactoryUnregistrar(this, eventType, factory);
    }

    /// <inheritdoc/>
    public override void Unsubscribe<TEvent>(Func<TEvent, Task> action)
    {
        Check.NotNull(action, nameof(action));

        GetOrCreateHandlerFactories(typeof(TEvent)).Locking(factories =>
        {
            factories.RemoveAll(factory =>
            {
                if (factory is not SingleInstanceHandlerFactory singleInstanceFactory)
                    return false;

                if (singleInstanceFactory.HandlerInstance is not ActionEventHandler<TEvent> actionHandler)
                    return false;

                return actionHandler.Action == action;
            });
        });
    }

    /// <inheritdoc/>
    public override void Unsubscribe(Type eventType, IEventHandler handler)
    {
        GetOrCreateHandlerFactories(eventType)
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                        factory is SingleInstanceHandlerFactory &&
                        (factory as SingleInstanceHandlerFactory).HandlerInstance == handler
                );
            });
    }

    /// <inheritdoc/>
    public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
    }

    /// <inheritdoc/>
    public override void UnsubscribeAll(Type eventType)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
    }

    protected async override Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        await PublishAsync(eventType, eventData, null);
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }

    //public override Task PublishFromOutboxAsync(
    //    OutgoingEventInfo outgoingEvent,
    //    OutboxConfig outboxConfig)
    //{
    //    return PublishAsync(outgoingEvent.EventName, outgoingEvent.EventData, null, eventId: outgoingEvent.Id);
    //}

    //public async override Task PublishManyFromOutboxAsync(
    //    IEnumerable<OutgoingEventInfo> outgoingEvents,
    //    OutboxConfig outboxConfig)
    //{
    //    using (var channel = ConnectionPool.Get(AbpRabbitMqEventBusOptions.ConnectionName).CreateModel())
    //    {
    //        var outgoingEventArray = outgoingEvents.ToArray();
    //        channel.ConfirmSelect();

    //        foreach (var outgoingEvent in outgoingEventArray)
    //        {
    //            await PublishAsync(
    //                channel,
    //                outgoingEvent.EventName,
    //                outgoingEvent.EventData,
    //                properties: null,
    //                eventId: outgoingEvent.Id);
    //        }

    //        channel.WaitForConfirmsOrDie();
    //    }
    //}

    //public async override Task ProcessFromInboxAsync(
    //    IncomingEventInfo incomingEvent,
    //    InboxConfig inboxConfig)
    //{
    //    var eventType = EventTypes.GetOrDefault(incomingEvent.EventName);
    //    if (eventType == null)
    //    {
    //        return;
    //    }

    //    var eventData = Serializer.Deserialize(incomingEvent.EventData, eventType);
    //    var exceptions = new List<Exception>();
    //    await TriggerHandlersAsync(eventType, eventData, exceptions, inboxConfig);
    //    if (exceptions.Any())
    //    {
    //        ThrowOriginalExceptions(eventType, exceptions);
    //    }
    //}

    protected override byte[] Serialize(object eventData) => Serializer.Serialize(eventData);

    public Task PublishAsync(
        Type eventType,
        object eventData,
        IBasicProperties properties,
        Dictionary<string, object> headersArguments = null)
    {
        var eventName = EventNameAttribute.GetNameOrDefault(eventType);
        var body = Serializer.Serialize(eventData);

        return PublishAsync(eventName, body, properties, headersArguments);
    }

    protected virtual Task PublishAsync(
        string eventName,
        byte[] body,
        IBasicProperties properties,
        Dictionary<string, object> headersArguments = null,
        Guid? eventId = null)
    {
        using var channel = ConnectionPool.Get(RabbitMqEventBusOptions.ConnectionName).CreateModel();
        return PublishAsync(channel, eventName, body, properties, headersArguments, eventId);
    }

    protected virtual Task PublishAsync(
        IModel channel,
        string eventName,
        byte[] body,
        IBasicProperties properties,
        Dictionary<string, object> headersArguments = null,
        Guid? eventId = null)
    {
        EnsureExchangeExists(channel);

        if (properties == null)
        {
            properties = channel.CreateBasicProperties();
            properties.DeliveryMode = RabbitMqConsts.DeliveryModes.Persistent;
        }

        if (string.IsNullOrEmpty(properties.MessageId))
        {
            properties.MessageId = (eventId ?? GuidGenerator.Create()).ToString("N");
        }

        SetEventMessageHeaders(properties, headersArguments);

        channel.BasicPublish(
            exchange: RabbitMqEventBusOptions.ExchangeName,
            routingKey: eventName,
            mandatory: true,
            basicProperties: properties,
            body: body
        );

        return Task.CompletedTask;
    }

    private void EnsureExchangeExists(IModel channel)
    {
        if (_exchangeCreated)
        {
            return;
        }

        try
        {
            channel.ExchangeDeclarePassive(RabbitMqEventBusOptions.ExchangeName);
        }
        catch (Exception)
        {
            channel.ExchangeDeclare(
                RabbitMqEventBusOptions.ExchangeName,
                RabbitMqEventBusOptions.GetExchangeTypeOrDefault(),
                durable: true
            );
        }
        _exchangeCreated = true;
    }

    private static void SetEventMessageHeaders(IBasicProperties properties, Dictionary<string, object> headersArguments)
    {
        if (headersArguments == null)
        {
            return;
        }

        properties.Headers ??= new Dictionary<string, object>();

        foreach (var header in headersArguments)
        {
            properties.Headers[header.Key] = header.Value;
        }
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType) => HandlerFactories.GetOrAdd(eventType, type =>
    {
        var eventName = EventNameAttribute.GetNameOrDefault(type);
        EventTypes[eventName] = type;
        return new List<IEventHandlerFactory>();
    });

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

        foreach (var handlerFactory in
                 HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
        {
            handlerFactoryList.Add(
                new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
        }

        return handlerFactoryList.ToArray();
    }

    private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
    {
        //Should trigger same type
        if (handlerEventType == targetEventType)
        {
            return true;
        }

        //TODO: Support inheritance? But it does not support on subscription to RabbitMq!
        //Should trigger for inherited types
        if (handlerEventType.IsAssignableFrom(targetEventType))
        {
            return true;
        }

        return false;
    }
}

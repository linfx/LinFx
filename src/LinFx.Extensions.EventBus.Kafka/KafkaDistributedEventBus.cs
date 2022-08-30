using Confluent.Kafka;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.EventBus.Distributed;
using LinFx.Extensions.Guids;
using LinFx.Extensions.Kafka;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Timing;
using LinFx.Extensions.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace LinFx.Extensions.EventBus.Kafka;

[Service(ReplaceServices = true)]
[ExposeServices(typeof(IDistributedEventBus), typeof(KafkaDistributedEventBus))]
public class KafkaDistributedEventBus : DistributedEventBusBase, ISingletonDependency
{
    protected KafkaEventBusOptions KafkaEventBusOptions { get; }
    protected IKafkaMessageConsumerFactory MessageConsumerFactory { get; }
    protected IKafkaSerializer Serializer { get; }
    protected IProducerPool ProducerPool { get; }
    protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
    protected ConcurrentDictionary<string, Type> EventTypes { get; }
    protected IKafkaMessageConsumer Consumer { get; private set; }

    public KafkaDistributedEventBus(
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<KafkaEventBusOptions> abpKafkaEventBusOptions,
        IKafkaMessageConsumerFactory messageConsumerFactory,
        IOptions<DistributedEventBusOptions> abpDistributedEventBusOptions,
        IKafkaSerializer serializer,
        IProducerPool producerPool,
        IGuidGenerator guidGenerator,
        IClock clock,
        IEventHandlerInvoker eventHandlerInvoker)
        : base(
            serviceScopeFactory,
            currentTenant,
            unitOfWorkManager,
            abpDistributedEventBusOptions,
            guidGenerator,
            clock,
            eventHandlerInvoker)
    {
        KafkaEventBusOptions = abpKafkaEventBusOptions.Value;
        MessageConsumerFactory = messageConsumerFactory;
        Serializer = serializer;
        ProducerPool = producerPool;

        HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        EventTypes = new ConcurrentDictionary<string, Type>();
    }

    public void Initialize()
    {
        Consumer = MessageConsumerFactory.Create(KafkaEventBusOptions.TopicName, KafkaEventBusOptions.GroupId, KafkaEventBusOptions.ConnectionName);
        Consumer.OnMessageReceived(ProcessEventAsync);
        base.SubscribeHandlers(Distributed.DistributedEventBusOptions.Handlers);
    }

    private async Task ProcessEventAsync(Message<string, byte[]> message)
    {
        var eventName = message.Key;
        var eventType = EventTypes.GetOrDefault(eventName);
        if (eventType == null)
            return;

        var messageId = message.GetMessageId();

        if (await AddToInboxAsync(messageId, eventName, eventType, message.Value))
            return;

        var eventData = Serializer.Deserialize(message.Value, eventType);
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
                var singleInstanceFactory = factory as SingleInstanceHandlerFactory;
                if (singleInstanceFactory == null)
                    return false;

                var actionHandler = singleInstanceFactory.HandlerInstance as ActionEventHandler<TEvent>;
                if (actionHandler == null)
                    return false;

                return actionHandler.Action == action;
            });
        });
    }

    /// <inheritdoc/>
    public override void Unsubscribe(Type eventType, IEventHandler handler)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories =>
        {
            factories.RemoveAll(factory => factory is SingleInstanceHandlerFactory handlerFactory && handlerFactory.HandlerInstance == handler);
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
        await PublishAsync(
            KafkaEventBusOptions.TopicName,
            eventType,
            eventData,
            new Headers { { "messageId", System.Text.Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")) } }
        );
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }

    public override Task PublishFromOutboxAsync(
        OutgoingEventInfo outgoingEvent,
        OutboxConfig outboxConfig)
    {
        return PublishAsync(
            KafkaEventBusOptions.TopicName,
            outgoingEvent.EventName,
            outgoingEvent.EventData,
            new Headers
            {
                    { "messageId", System.Text.Encoding.UTF8.GetBytes(outgoingEvent.Id.ToString("N")) }
            }
        );
    }

    public override Task PublishManyFromOutboxAsync(IEnumerable<OutgoingEventInfo> outgoingEvents, OutboxConfig outboxConfig)
    {
        var producer = ProducerPool.Get(KafkaEventBusOptions.ConnectionName);
        var outgoingEventArray = outgoingEvents.ToArray();

        foreach (var outgoingEvent in outgoingEventArray)
        {
            var messageId = outgoingEvent.Id.ToString("N");
            var headers = new Headers
            {
                { "messageId", System.Text.Encoding.UTF8.GetBytes(messageId)}
            };

            producer.Produce(
                KafkaEventBusOptions.TopicName,
                new Message<string, byte[]>
                {
                    Key = outgoingEvent.EventName,
                    Value = outgoingEvent.EventData,
                    Headers = headers
                });
        }

        return Task.CompletedTask;
    }

    public async override Task ProcessFromInboxAsync(
        IncomingEventInfo incomingEvent,
        InboxConfig inboxConfig)
    {
        var eventType = EventTypes.GetOrDefault(incomingEvent.EventName);
        if (eventType == null)
        {
            return;
        }

        var eventData = Serializer.Deserialize(incomingEvent.EventData, eventType);
        var exceptions = new List<Exception>();
        await TriggerHandlersAsync(eventType, eventData, exceptions, inboxConfig);
        if (exceptions.Any())
        {
            ThrowOriginalExceptions(eventType, exceptions);
        }
    }

    protected override byte[] Serialize(object eventData)
    {
        return Serializer.Serialize(eventData);
    }

    private Task PublishAsync(string topicName, Type eventType, object eventData, Headers headers)
    {
        var eventName = EventNameAttribute.GetNameOrDefault(eventType);
        var body = Serializer.Serialize(eventData);

        return PublishAsync(topicName, eventName, body, headers);
    }

    private Task<DeliveryResult<string, byte[]>> PublishAsync(
        string topicName,
        string eventName,
        byte[] body,
        Headers headers)
    {
        var producer = ProducerPool.Get(KafkaEventBusOptions.ConnectionName);

        return producer.ProduceAsync(
            topicName,
            new Message<string, byte[]>
            {
                Key = eventName,
                Value = body,
                Headers = headers
            });
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
    {
        return HandlerFactories.GetOrAdd(
            eventType,
            type =>
            {
                var eventName = EventNameAttribute.GetNameOrDefault(type);
                EventTypes[eventName] = type;
                return new List<IEventHandlerFactory>();
            }
        );
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

        foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key))
        )
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
            return true;

        //Should trigger for inherited types
        if (handlerEventType.IsAssignableFrom(targetEventType))
            return true;

        return false;
    }
}

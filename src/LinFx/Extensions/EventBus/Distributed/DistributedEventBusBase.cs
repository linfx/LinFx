using LinFx.Extensions.Guids;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Timing;
using LinFx.Extensions.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.EventBus.Distributed;

public abstract class DistributedEventBusBase : EventBusBase, IDistributedEventBus
{
    protected IGuidGenerator GuidGenerator { get; }
    protected IClock Clock { get; }
    protected DistributedEventBusOptions DistributedEventBusOptions { get; }

    protected DistributedEventBusBase(
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<DistributedEventBusOptions> abpDistributedEventBusOptions,
        IGuidGenerator guidGenerator,
        IClock clock
    ) : base(
        serviceScopeFactory,
        currentTenant,
        unitOfWorkManager)
    {
        Clock = clock;
        GuidGenerator = guidGenerator;
        DistributedEventBusOptions = abpDistributedEventBusOptions.Value;
    }

    public IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    //public override Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true)
    //{
    //    return PublishAsync(eventType, eventData, onUnitOfWorkComplete, useOutbox: true);
    //}

    public Task PublishAsync<TEvent>(
        TEvent eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true)
        where TEvent : class
    {
        return PublishAsync(typeof(TEvent), eventData, onUnitOfWorkComplete, useOutbox);
    }

    public async Task PublishAsync(
        Type eventType,
        object eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true)
    {
        if (onUnitOfWorkComplete && UnitOfWorkManager.Current != null)
        {
            //AddToUnitOfWork(
            //    UnitOfWorkManager.Current,
            //    new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext(), useOutbox)
            //);
            return;
        }

        //if (useOutbox)
        //{
        //    if (await AddToOutboxAsync(eventType, eventData))
        //    {
        //        return;
        //    }
        //}

        await PublishToEventBusAsync(eventType, eventData);
    }

    //public abstract Task PublishFromOutboxAsync(
    //    OutgoingEventInfo outgoingEvent,
    //    OutboxConfig outboxConfig
    //);

    //public abstract Task PublishManyFromOutboxAsync(
    //    IEnumerable<OutgoingEventInfo> outgoingEvents,
    //    OutboxConfig outboxConfig
    //);

    //public abstract Task ProcessFromInboxAsync(
    //    IncomingEventInfo incomingEvent,
    //    InboxConfig inboxConfig);

    //private async Task<bool> AddToOutboxAsync(Type eventType, object eventData)
    //{
    //    var unitOfWork = UnitOfWorkManager.Current;
    //    if (unitOfWork == null)
    //    {
    //        return false;
    //    }

    //    foreach (var outboxConfig in DistributedEventBusOptions.Outboxes.Values.OrderBy(x => x.Selector is null))
    //    {
    //        if (outboxConfig.Selector == null || outboxConfig.Selector(eventType))
    //        {
    //            var eventOutbox =
    //                (IEventOutbox)unitOfWork.ServiceProvider.GetRequiredService(outboxConfig.ImplementationType);
    //            var eventName = EventNameAttribute.GetNameOrDefault(eventType);
    //            await eventOutbox.EnqueueAsync(
    //                new OutgoingEventInfo(
    //                    GuidGenerator.Create(),
    //                    eventName,
    //                    Serialize(eventData),
    //                    Clock.Now
    //                )
    //            );
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //protected async Task<bool> AddToInboxAsync(
    //    string messageId,
    //    string eventName,
    //    Type eventType,
    //    byte[] eventBytes)
    //{
    //    if (DistributedEventBusOptions.Inboxes.Count <= 0)
    //    {
    //        return false;
    //    }

    //    using (var scope = ServiceScopeFactory.CreateScope())
    //    {
    //        foreach (var inboxConfig in DistributedEventBusOptions.Inboxes.Values.OrderBy(x => x.EventSelector is null))
    //        {
    //            if (inboxConfig.EventSelector == null || inboxConfig.EventSelector(eventType))
    //            {
    //                var eventInbox =
    //                    (IEventInbox)scope.ServiceProvider.GetRequiredService(inboxConfig.ImplementationType);

    //                if (!messageId.IsNullOrEmpty())
    //                {
    //                    if (await eventInbox.ExistsByMessageIdAsync(messageId))
    //                    {
    //                        continue;
    //                    }
    //                }

    //                await eventInbox.EnqueueAsync(
    //                    new IncomingEventInfo(
    //                        GuidGenerator.Create(),
    //                        messageId,
    //                        eventName,
    //                        eventBytes,
    //                        Clock.Now
    //                    )
    //                );
    //            }
    //        }
    //    }

    //    return true;
    //}

    protected abstract byte[] Serialize(object eventData);
}

using LinFx.Domain.Entities.Events.Distributed;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.EventBus;
using LinFx.Extensions.EventBus.Distributed;
using LinFx.Extensions.EventBus.Local;
using LinFx.Extensions.Uow;
using LinFx.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace LinFx.Domain.Entities.Events;

/// <summary>
/// Used to trigger entity change events.
/// </summary>
[Service]
public class EntityChangeEventHelper : IEntityChangeEventHelper
{
    private const string UnitOfWorkEventRecordEntityPropName = "_Entity";

    public ILogger<EntityChangeEventHelper> Logger { get; set; }
    public ILocalEventBus LocalEventBus { get; set; }
    public IDistributedEventBus DistributedEventBus { get; set; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IEntityToEtoMapper EntityToEtoMapper { get; }
    protected DistributedEntityEventOptions DistributedEntityEventOptions { get; }

    public EntityChangeEventHelper(
        IUnitOfWorkManager unitOfWorkManager,
        IEntityToEtoMapper entityToEtoMapper,
        IOptions<DistributedEntityEventOptions> distributedEntityEventOptions)
    {
        UnitOfWorkManager = unitOfWorkManager;
        EntityToEtoMapper = entityToEtoMapper;
        DistributedEntityEventOptions = distributedEntityEventOptions.Value;

        LocalEventBus = NullLocalEventBus.Instance;
        DistributedEventBus = NullDistributedEventBus.Instance;
        Logger = NullLogger<EntityChangeEventHelper>.Instance;
    }

    public virtual void PublishEntityCreatingEvent(object entity) => TriggerEventWithEntity(LocalEventBus, typeof(EntityCreatedEventData<>), entity, entity);

    public virtual void PublishEntityCreatedEvent(object entity)
    {
        TriggerEventWithEntity(LocalEventBus, typeof(EntityCreatedEventData<>), entity, entity);

        if (ShouldPublishDistributedEventForEntity(entity))
        {
            var eto = EntityToEtoMapper.Map(entity);
            if (eto != null)
                TriggerEventWithEntity(DistributedEventBus, typeof(EntityCreatedEto<>), eto, entity);
        }
    }

    private bool ShouldPublishDistributedEventForEntity(object entity)
    {
        return DistributedEntityEventOptions
            .AutoEventSelectors
            .IsMatch(
                ProxyHelper
                    .UnProxy(entity)
                    .GetType()
            );
    }

    public virtual void PublishEntityUpdatingEvent(object entity) => TriggerEventWithEntity(LocalEventBus, typeof(EntityUpdatedEventData<>), entity, entity);

    public virtual void PublishEntityUpdatedEvent(object entity)
    {
        TriggerEventWithEntity(LocalEventBus, typeof(EntityUpdatedEventData<>), entity, entity);

        if (ShouldPublishDistributedEventForEntity(entity))
        {
            var eto = EntityToEtoMapper.Map(entity);
            if (eto != null)
            {
                TriggerEventWithEntity(
                    DistributedEventBus,
                    typeof(EntityUpdatedEto<>),
                    eto,
                    entity
                );
            }
        }
    }

    public virtual void PublishEntityDeletingEvent(object entity) => TriggerEventWithEntity(LocalEventBus, typeof(EntityDeletedEventData<>), entity, entity);

    public virtual void PublishEntityDeletedEvent(object entity)
    {
        TriggerEventWithEntity(LocalEventBus, typeof(EntityDeletedEventData<>), entity, entity);

        if (ShouldPublishDistributedEventForEntity(entity))
        {
            var eto = EntityToEtoMapper.Map(entity);
            if (eto != null)
            {
                TriggerEventWithEntity(DistributedEventBus, typeof(EntityDeletedEto<>), eto, entity);
            }
        }
    }

    protected virtual void TriggerEventWithEntity(IEventBus eventPublisher, Type genericEventType, object entityOrEto, object originalEntity)
    {
        var entityType = ProxyHelper.UnProxy(entityOrEto).GetType();
        var eventType = genericEventType.MakeGenericType(entityType);
        var eventData = Activator.CreateInstance(eventType, entityOrEto);
        var currentUow = UnitOfWorkManager.Current;

        if (currentUow == null)
        {
            Logger.LogWarning("UnitOfWorkManager.Current is null! Can not publish the event.");
            return;
        }

        var eventRecord = new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext())
        {
            Properties = { { UnitOfWorkEventRecordEntityPropName, originalEntity } }
        };

        /* We are trying to eliminate same events for the same entity.
         * In this way, for example, we don't trigger update event for an entity multiple times
         * even if it is updated multiple times in the current UOW.
         */
        if (eventPublisher == DistributedEventBus)
            currentUow.AddOrReplaceDistributedEvent(eventRecord, otherRecord => IsSameEntityEventRecord(eventRecord, otherRecord));
        else
            currentUow.AddOrReplaceLocalEvent(eventRecord, otherRecord => IsSameEntityEventRecord(eventRecord, otherRecord));
    }

    public bool IsSameEntityEventRecord(UnitOfWorkEventRecord record1, UnitOfWorkEventRecord record2)
    {
        if (record1.EventType != record2.EventType)
            return false;

        var record1OriginalEntity = record1.Properties.GetOrDefault(UnitOfWorkEventRecordEntityPropName) as IEntity;
        var record2OriginalEntity = record2.Properties.GetOrDefault(UnitOfWorkEventRecordEntityPropName) as IEntity;

        if (record1OriginalEntity == null || record2OriginalEntity == null)
            return false;

        return EntityHelper.EntityEquals(record1OriginalEntity, record2OriginalEntity);
    }
}

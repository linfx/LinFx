using LinFx.Extensions.Uow;
using System.Collections.ObjectModel;

namespace LinFx.Domain.Entities;

[Serializable]
public abstract class BasicAggregateRoot : Entity,
    IAggregateRoot,
    IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents = new Collection<DomainEventRecord>();

    public virtual IEnumerable<DomainEventRecord> GetLocalEvents() => _localEvents;

    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents() => _distributedEvents;

    public virtual void ClearLocalEvents() => _localEvents.Clear();

    public virtual void ClearDistributedEvents() => _distributedEvents.Clear();

    protected virtual void AddLocalEvent(object eventData) => _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));

    protected virtual void AddDistributedEvent(object eventData) => _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
}

[Serializable]
public abstract class BasicAggregateRoot<TKey> : Entity<TKey>,
    IAggregateRoot<TKey>,
    IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents = new Collection<DomainEventRecord>();

    protected BasicAggregateRoot() { }

    protected BasicAggregateRoot(TKey id)
        : base(id)
    { }

    /// <summary>
    /// 获得所有本地事件
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<DomainEventRecord> GetLocalEvents() => _localEvents;

    /// <summary>
    /// 获得所有分布式事件
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents() => _distributedEvents;

    public virtual void ClearLocalEvents() => _localEvents.Clear();

    public virtual void ClearDistributedEvents() => _distributedEvents.Clear();

    /// <summary>
    /// 添加本地事件
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void AddLocalEvent(object eventData) => _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));

    /// <summary>
    /// 填加分步式事件
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void AddDistributedEvent(object eventData) => _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
}

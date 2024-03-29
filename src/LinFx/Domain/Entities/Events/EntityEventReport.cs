namespace LinFx.Domain.Entities.Events;

/// <summary>
/// 领域事件报告
/// </summary>
public class EntityEventReport
{
    /// <summary>
    /// 领域事件
    /// </summary>
    public List<DomainEventEntry> DomainEvents { get; }

    public List<DomainEventEntry> DistributedEvents { get; }

    public EntityEventReport()
    {
        DomainEvents = [];
        DistributedEvents = [];
    }

    public override string ToString() => $"[{nameof(EntityEventReport)}] DomainEvents: {DomainEvents.Count}, DistributedEvents: {DistributedEvents.Count}";
}

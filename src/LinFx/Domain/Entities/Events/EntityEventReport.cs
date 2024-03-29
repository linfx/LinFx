namespace LinFx.Domain.Entities.Events;

/// <summary>
/// �����¼�����
/// </summary>
public class EntityEventReport
{
    /// <summary>
    /// �����¼�
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

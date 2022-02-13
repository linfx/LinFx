using System.Collections.Generic;

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
        DomainEvents = new List<DomainEventEntry>();
        DistributedEvents = new List<DomainEventEntry>();
    }

    public override string ToString()
    {
        return $"[{nameof(EntityEventReport)}] DomainEvents: {DomainEvents.Count}, DistributedEvents: {DistributedEvents.Count}";
    }
}

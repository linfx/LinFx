using System.Collections.Generic;

namespace LinFx.Domain.Entities.Events
{
    public class EntityEventReport
    {
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
}
using MediatR;
using System.Collections.Generic;

namespace LinFx.Domain.Models
{
    /// <summary>
    /// 聚合根
    /// </summary>
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        private List<INotification> _domainEvents;

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public IEnumerable<INotification> GetDomainEvents()
        {
            return _domainEvents;
        }
    }

    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {
        private List<INotification> _domainEvents;

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public IEnumerable<INotification> GetDomainEvents()
        {
            return _domainEvents;
        }
    }
}

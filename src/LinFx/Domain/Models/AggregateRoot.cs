using MediatR;
using System.Collections.Generic;

namespace LinFx.Domain.Models
{
    /// <summary>
    /// 聚合根
    /// </summary>
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        private ICollection<INotification> _domainEvents;

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents ??= new List<INotification>();
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

    /// <summary>
    /// 聚合根
    /// </summary>
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {
        private ICollection<INotification> _domainEvents;

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents ??= new List<INotification>();
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

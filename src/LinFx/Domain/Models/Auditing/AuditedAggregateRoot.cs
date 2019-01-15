using System;

namespace LinFx.Domain.Models.Auditing
{
    public class AuditedAggregateRoot
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual Guid? LastModifierId { get; set; }
    }

    public abstract class AuditedEntity<TKey> : CreationAuditedEntity<TKey>
    {
        /// <inheritdoc />
        public virtual DateTime? LastModificationTime { get; set; }

        /// <inheritdoc />
        public virtual Guid? LastModifierId { get; set; }
    }
}

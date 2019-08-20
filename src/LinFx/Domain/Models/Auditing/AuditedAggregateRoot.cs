using LinFx.Extensions.Auditing;
using System;

namespace LinFx.Domain.Models.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAuditedObject"/> for aggregate roots.
    /// </summary>
    public abstract class AuditedAggregateRoot : CreationAuditedAggregateRoot, IAuditedObject
    {
        /// <inheritdoc />
        public virtual DateTimeOffset? LastModificationTime { get; set; }

        /// <inheritdoc />
        public virtual string LastModifierId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAuditedObject"/> for aggregate roots.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public abstract class AuditedAggregateRoot<TKey> : CreationAuditedAggregateRoot<TKey>, IAuditedObject
    {
        /// <inheritdoc />
        public virtual DateTimeOffset? LastModificationTime { get; set; }

        /// <inheritdoc />
        public virtual string LastModifierId { get; set; }
    }
}

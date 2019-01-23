using System;

namespace LinFx.Extensions.Auditing
{
    /// <summary>
    /// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited entities.
    /// </summary>
    public abstract class FullAuditedEntity : AuditedEntity, IFullAuditedObject
    {
        /// <inheritdoc />
        public virtual bool IsDeleted { get; set; }

        /// <inheritdoc />
        public virtual Guid? DeleterId { get; set; }

        /// <inheritdoc />
        public virtual DateTime? DeletionTime { get; set; }
    }

    /// <summary>
    /// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IFullAuditedObject
    {
        /// <inheritdoc />
        public virtual bool IsDeleted { get; set; }

        /// <inheritdoc />
        public virtual Guid? DeleterId { get; set; }

        /// <inheritdoc />
        public virtual DateTime? DeletionTime { get; set; }
    }
}

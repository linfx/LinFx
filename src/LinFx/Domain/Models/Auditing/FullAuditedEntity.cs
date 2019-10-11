using LinFx.Extensions.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Domain.Models.Auditing
{
    /// <summary>
    /// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited entities.
    /// </summary>
    public abstract class FullAuditedEntity : AuditedEntity, IFullAuditedObject
    {
        /// <inheritdoc />
        public virtual bool IsDeleted { get; set; }

        /// <inheritdoc />
        [StringLength(50)]
        public virtual string DeleterId { get; set; }

        /// <inheritdoc />
        public virtual DateTimeOffset? DeletionTime { get; set; }
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
        [StringLength(50)]
        public virtual string DeleterId { get; set; }

        /// <inheritdoc />
        public virtual DateTimeOffset? DeletionTime { get; set; }
    }
}

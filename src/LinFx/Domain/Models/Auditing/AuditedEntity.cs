using LinFx.Extensions.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Domain.Models.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAuditedObject"/>.
    /// </summary>
    public abstract class AuditedEntity : CreationAuditedEntity, IAuditedObject
    {
        /// <inheritdoc />
        public virtual DateTimeOffset? LastModificationTime { get; set; }

        /// <inheritdoc />
        [StringLength(50)]
        public virtual string LastModifierId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAuditedObject"/>.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public abstract class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IAuditedObject
    {
        /// <inheritdoc />
        public virtual DateTimeOffset? LastModificationTime { get; set; }

        /// <inheritdoc />
        [StringLength(50)]
        public virtual string LastModifierId { get; set; }
    }
}
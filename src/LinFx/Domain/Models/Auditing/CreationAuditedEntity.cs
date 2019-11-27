using LinFx.Domain.Models;
using LinFx.Extensions.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Domain.Models.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAuditedObject" /> for an entity.
    /// </summary>
    [Serializable]
    public abstract class CreationAuditedEntity : Entity, ICreationAuditedObject
    {
        /// <inheritdoc />
        public virtual DateTimeOffset CreationTime { get; set; }

        /// <inheritdoc />
        [StringLength(50)]
        public virtual string CreatorId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for an entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAuditedObject
    {
        /// <inheritdoc />
        public virtual DateTimeOffset CreationTime { get; set; }

        /// <inheritdoc />
        [StringLength(50)]
        public virtual string CreatorId { get; set; }
    }
}

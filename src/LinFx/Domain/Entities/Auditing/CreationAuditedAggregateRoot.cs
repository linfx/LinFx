using LinFx;
using LinFx.Domain;
using LinFx.Domain.Entities;
using LinFx.Extensions.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for aggregate roots.
    /// </summary>
    public abstract class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedObject
    {
        /// <inheritdoc />
        public virtual DateTimeOffset CreationTime { get; set; }

        /// <inheritdoc />
        [StringLength(32)]
        public virtual string CreatorId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for aggregate roots.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public abstract class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditedObject
    {
        /// <inheritdoc />
        public virtual DateTimeOffset CreationTime { get; set; }

        /// <inheritdoc />
        [StringLength(32)]
        public virtual string CreatorId { get; set; }
    }
}

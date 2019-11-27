using LinFx.Domain.Models;
using LinFx.Extensions.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Domain.Models.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for aggregate roots.
    /// </summary>
    public abstract class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedObject
    {
        /// <inheritdoc />
        public virtual DateTimeOffset CreationTime { get; set; }

        /// <inheritdoc />
        [StringLength(50)]
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
        [StringLength(50)]
        public virtual string CreatorId { get; set; }
    }
}

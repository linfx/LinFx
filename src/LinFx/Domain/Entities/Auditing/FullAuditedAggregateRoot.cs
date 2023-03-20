using LinFx.Extensions.Auditing;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Domain.Entities.Auditing;

/// <summary>
/// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited aggregate roots.
/// </summary>
public abstract class FullAuditedAggregateRoot : AuditedAggregateRoot, IFullAuditedObject
{
    public virtual bool IsDeleted { get; set; }

    [StringLength(32)]
    public virtual string DeleterId { get; set; }

    public virtual DateTimeOffset? DeletionTime { get; set; }
}

/// <summary>
/// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited aggregate roots.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
public abstract class FullAuditedAggregateRoot<TKey> : AuditedAggregateRoot<TKey>, IFullAuditedObject
{
    public virtual bool IsDeleted { get; set; }

    [StringLength(32)]
    public virtual string DeleterId { get; set; }

    public virtual DateTimeOffset? DeletionTime { get; set; }
}

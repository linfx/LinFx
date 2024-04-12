using LinFx.Extensions.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinFx.Domain.Entities.Auditing;

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for aggregate roots.
/// </summary>
public abstract class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedObject
{
    /// <inheritdoc />
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual DateTimeOffset CreationTime { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    [StringLength(64)]
    public virtual string? CreatorId { get; set; }
}

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for aggregate roots.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
public abstract class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditedObject
{
    /// <inheritdoc />
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual DateTimeOffset CreationTime { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    [StringLength(64)]
    public virtual string? CreatorId { get; set; }
}

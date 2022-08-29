using LinFx.Extensions.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinFx.Domain.Entities.Auditing;

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObject" /> for an entity.
/// </summary>
[Serializable]
public abstract class CreationAuditedEntity : Entity, ICreationAuditedObject
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual DateTimeOffset CreationTime { get; set; }

    [StringLength(50)]
    public virtual string CreatorId { get; set; } = string.Empty;
}

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for an entity.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
[Serializable]
public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAuditedObject
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual DateTimeOffset CreationTime { get; set; } = DateTimeOffset.UtcNow;

    [StringLength(50)]
    public virtual string CreatorId { get; set; } = string.Empty;
}

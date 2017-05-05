using System;

namespace LinFx.Domain.Entities.Auditing
{
    /// <summary>
    /// A shortcut of <see cref="FullAuditedEntity{TPrimaryKey}"/> for most used primary key type (<see cref="string"/>).
    /// </summary>
    public abstract class FullAuditedEntity : FullAuditedEntity<string>, IEntity
    {
    }

    /// <summary>
    /// Implements <see cref="IFullAudited"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public abstract class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>, IFullAudited
    {
        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        public virtual bool IsDeleted { get; set; }
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        public virtual string DeleterUserId { get; set; }
        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        public virtual DateTime? DeleteionTime { get; set; }
    }
}

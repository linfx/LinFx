using System;

namespace LinFx.Domain.Entities.Auditing
{
    /// <summary>
    /// A shortcut of <see cref="AuditedEntity{TPrimaryKey}"/> for most used primary key type (<see cref="string"/>).
    /// </summary>
    public abstract class AuditedEntity : AuditedEntity<string>, IEntity
    {
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited"/>.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public abstract class AuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>, IAudited
    {
        /// <summary>
        /// Last modification date of this entity.
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// Last modifier user of this entity.
        /// </summary>
        public virtual string LastModifierUserId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited{TUser}"/>.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public abstract class AuditedEntity<TPrimaryKey, TUser> : AuditedEntity<TPrimaryKey>, IAudited<TUser>
        where TUser : IEntity
    {
        /// <summary>
        /// Reference to the creator user of this entity.
        /// </summary>
        //[ForeignKey("CreatorUserId")]
        public virtual TUser CreatorUser { get; set; }
        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// </summary>
        //[ForeignKey("LastModifierUserId")]
        public virtual TUser LastModifierUser { get; set; }
    }
}

using System;

namespace LinFx.Domain.Entities.Auditing
{
    #region AuditedEntity

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

    #endregion

    #region CreationAuditedEntity

    /// <summary>
    /// A shortcut of <see cref="CreationAuditedEntity{TPrimaryKey}"/> for most used primary key type (<see cref="string"/>).
    /// </summary>
    public abstract class CreationAuditedEntity : CreationAuditedEntity<string>, IEntity
    {
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited"/>.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreationAudited
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
        /// <summary>
        /// Creator of this entity.
        /// </summary>
        public virtual string CreatorUserId { get; set; }
        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreationAuditedEntity()
        {
            CreationTime = Clock.Now;
        }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited{TUser}"/>.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public abstract class CreationAuditedEntity<TPrimaryKey, TUser> : CreationAuditedEntity<TPrimaryKey>, ICreationAudited<TUser> where TUser : IEntity
    {
        /// <summary>
        /// Reference to the creator user of this entity.
        /// </summary>
        //[ForeignKey("CreatorUserId")]
        public virtual TUser CreatorUser { get; set; }
    }

    #endregion

    #region FullAuditedEntity

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

    #endregion
}
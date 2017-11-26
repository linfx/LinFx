using System;

namespace LinFx.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities which must be audited.
    /// Related properties automatically set when saving/updating <see cref="Entity"/> objects.
    /// </summary>
    public interface IAudited : ICreationAudited, IModificationAudited
    {
    }

    /// <summary>
    /// Adds navigation properties to <see cref="IAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IAudited<TUser> : IAudited, ICreationAudited<TUser>, IModificationAudited<TUser>
        where TUser : IEntity
    {
    }

    /// <summary>
    /// This interface is implemented by entities that is wanted to store modification information (who and when modified lastly).
    /// Properties are automatically set when updating the <see cref="IEntity"/>.
    /// </summary>
    public interface IModificationAudited : IHasModificationTime
    {
        /// <summary>
        /// Last modifier user for this entity.
        /// </summary>
        long LastModifierUserId { get; set; }
    }

    /// <summary>
    /// An entity can implement this interface if <see cref="LastModificationTime"/> of this entity must be stored.
    /// <see cref="LastModificationTime"/> is automatically set when updating <see cref="Entity"/>.
    /// </summary>
    public interface IHasModificationTime
    {
        /// <summary>
        /// The last modified time for this entity.
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }

    /// <summary>
    /// Adds navigation properties to <see cref="IModificationAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IModificationAudited<TUser> : IModificationAudited
        where TUser : IEntity
    {
        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// </summary>
        TUser LastModifierUser { get; set; }
    }

    /// <summary>
    /// This interface is implemented by entities that is wanted to store creation information (who and when created).
    /// Creation time and creator user are automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface ICreationAudited : IHasCreationTime
    {
        /// <summary>
        /// Id of the creator user of this entity.
        /// </summary>
        long CreatorUserId { get; set; }
    }

    /// <summary>
    /// Adds navigation properties to <see cref="ICreationAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface ICreationAudited<TUser> : ICreationAudited
        where TUser : IEntity
    {
        /// <summary>
        /// Reference to the creator user of this entity.
        /// </summary>
        TUser CreatorUser { get; set; }
    }

    /// <summary>
    /// An entity can implement this interface if <see cref="CreationTime"/> of this entity must be stored.
    /// <see cref="CreationTime"/> is automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        DateTime CreationTime { get; set; }
    }

    /// <summary>
    /// This interface is implemented by entities which wanted to store deletion information (who and when deleted).
    /// </summary>
    public interface IDeletionAudited : IHasDeletionTime
    {
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        long DeleterUserId { get; set; }
    }

    /// <summary>
    /// Adds navigation properties to <see cref="IDeletionAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IDeletionAudited<TUser> : IDeletionAudited
        where TUser : IEntity
    {
        /// <summary>
        /// Reference to the deleter user of this entity.
        /// </summary>
        TUser DeleterUser { get; set; }
    }

    /// <summary>
    /// An entity can implement this interface if <see cref="DeleteionTime"/> of this entity must be stored.
    /// <see cref="DeleteionTime"/> is automatically set when deleting <see cref="Entity"/>.
    /// </summary>
    public interface IHasDeletionTime : ISoftDelete
    {
        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        DateTime? DeleteionTime { get; set; }
    }

    /// <summary>
    /// This interface ads <see cref="IDeletionAudited"/> to <see cref="IAudited"/> for a fully audited entity.
    /// </summary>
    public interface IFullAudited : IAudited, IDeletionAudited
    {
    }

    /// <summary>
    /// Adds navigation properties to <see cref="IFullAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IFullAudited<TUser> : IAudited<TUser>, IFullAudited, IDeletionAudited<TUser>
        where TUser : IEntity
    {
    }
}

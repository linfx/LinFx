﻿using LinFx.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Domain.Repositories;

public interface IBasicRepository<TEntity> : IReadOnlyBasicRepository<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Inserts a new entity.
    /// </summary>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <param name="entity">Inserted entity</param>
    ValueTask<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts multiple new entities.
    /// </summary>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <param name="entities">Entities to be inserted.</param>
    /// <returns>Awaitable <see cref="Task"/>.</returns>
    Task InsertManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <param name="entity">Entity</param>
    ValueTask<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates multiple entities.
    /// </summary>
    /// <param name="entities">Entities to be updated.</param>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Awaitable <see cref="Task"/>.</returns>
    Task UpdateManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">Entity to be deleted</param>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes multiple entities.
    /// </summary>
    /// <param name="entities">Entities to be deleted.</param>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Awaitable <see cref="Task"/>.</returns>
    Task DeleteManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
}

public interface IBasicRepository<TEntity, TKey> : IBasicRepository<TEntity>, IReadOnlyBasicRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// Deletes an entity by primary key.
    /// </summary>
    /// <param name="id">Primary key of the entity</param>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);  //TODO: Return true if deleted

    /// <summary>
    /// Deletes multiple entities by primary keys.
    /// </summary>
    /// <param name="ids">Primary keys of the each entity.</param>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Awaitable <see cref="Task"/>.</returns>
    Task DeleteManyAsync([NotNull] IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default);
}

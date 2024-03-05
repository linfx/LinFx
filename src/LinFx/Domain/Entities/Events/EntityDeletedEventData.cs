namespace LinFx.Domain.Entities.Events;

/// <summary>
/// This type of event can be used to notify just after deletion of an Entity.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <remarks>
/// Constructor.
/// </remarks>
/// <param name="entity">The entity which is deleted</param>
[Serializable]
public class EntityDeletedEventData<TEntity>(TEntity entity) : EntityChangedEventData<TEntity>(entity)
{
}

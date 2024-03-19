namespace LinFx.Domain.Entities.Events;

/// <summary>
/// This type of event can be used to notify just after update of an Entity.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <remarks>
/// Constructor.
/// </remarks>
/// <param name="entity">The entity which is updated</param>
[Serializable]
public class EntityUpdatedEventData<TEntity>(TEntity entity) : EntityChangedEventData<TEntity>(entity)
{
}

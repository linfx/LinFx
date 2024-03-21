using LinFx.Extensions.EventBus;

namespace LinFx.Domain.Entities.Events;

/// <summary>
/// Used to pass data for an event that is related to with an <see cref="IEntity"/> object.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <remarks>
/// Constructor.
/// </remarks>
/// <param name="entity">Related entity with this event</param>
[Serializable]
public class EntityEventData<TEntity>(TEntity entity) : IEventDataWithInheritableGenericArgument, IEventDataMayHaveTenantId
{
    /// <summary>
    /// Related entity with this event.
    /// </summary>
    public TEntity Entity { get; } = entity;

    public virtual object[] GetConstructorArgs() => new object[] { Entity! };

    public virtual bool IsMultiTenant(out string? tenantId)
    {
        if (Entity is IMultiTenant multiTenantEntity)
        {
            tenantId = multiTenantEntity.TenantId;
            return true;
        }

        tenantId = default;
        return false;
    }
}

using LinFx.Extensions.EventBus;

namespace LinFx.Domain.Entities.Events.Distributed;

[Serializable]
//[GenericEventName(Postfix = ".Updated")]
public class EntityUpdatedEto<TEntityEto> : IEventDataMayHaveTenantId
{
    public TEntityEto Entity { get; set; }

    public EntityUpdatedEto(TEntityEto entity)
    {
        Entity = entity;
    }

    public virtual bool IsMultiTenant(out string tenantId)
    {
        if (Entity is IMultiTenant multiTenantEntity)
        {
            tenantId = multiTenantEntity.TenantId;
            return true;
        }

        tenantId = null;
        return false;
    }
}

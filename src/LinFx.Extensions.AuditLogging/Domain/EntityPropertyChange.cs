using LinFx.Domain.Entities;
using LinFx.Extensions.Auditing;
using LinFx.Utils;

namespace LinFx.Extensions.AuditLogging;

/// <summary>
/// 属性变化
/// </summary>
[DisableAuditing]
public class EntityPropertyChange : Entity<string>, IMultiTenant
{
    public virtual string? TenantId { get; protected set; }

    public virtual string EntityChangeId { get; protected set; }

    public virtual string NewValue { get; protected set; }

    public virtual string OriginalValue { get; protected set; }

    public virtual string PropertyName { get; protected set; }

    public virtual string PropertyTypeFullName { get; protected set; }

    protected EntityPropertyChange() { }

    public EntityPropertyChange(string entityChangeId, EntityPropertyChangeInfo entityChangeInfo, string? tenantId = default)
    {
        Id = IDUtils.NewIdString();
        TenantId = tenantId;
        EntityChangeId = entityChangeId;
        NewValue = entityChangeInfo.NewValue.Truncate(EntityPropertyChangeConsts.MaxNewValueLength);
        OriginalValue = entityChangeInfo.OriginalValue.Truncate(EntityPropertyChangeConsts.MaxOriginalValueLength);
        PropertyName = entityChangeInfo.PropertyName.TruncateFromBeginning(EntityPropertyChangeConsts.MaxPropertyNameLength);
        PropertyTypeFullName = entityChangeInfo.PropertyTypeFullName.TruncateFromBeginning(EntityPropertyChangeConsts.MaxPropertyTypeFullNameLength);
    }
}

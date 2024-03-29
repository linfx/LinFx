using LinFx.Domain.Entities;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.ObjectExtending;
using LinFx.Utils;

namespace LinFx.Extensions.AuditLogging;

/// <summary>
/// 实体变化
/// </summary>
[DisableAuditing]
public class EntityChange : Entity<string>, IMultiTenant, IHasExtraProperties
{
    public virtual string AuditLogId { get; protected set; }

    public virtual string? TenantId { get; protected set; }

    public virtual DateTime ChangeTime { get; protected set; }

    public virtual EntityChangeType ChangeType { get; protected set; }

    public virtual string EntityTenantId { get; protected set; }

    public virtual string EntityId { get; protected set; }

    public virtual string EntityTypeFullName { get; protected set; }

    public virtual ICollection<EntityPropertyChange> PropertyChanges { get; protected set; }

    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

    protected EntityChange()
    {
        ExtraProperties = [];
    }

    public EntityChange(
        string auditLogId,
        EntityChangeInfo entityChangeInfo,
        string? tenantId = null)
    {
        Id = IDUtils.NewIdString();
        AuditLogId = auditLogId;
        TenantId = tenantId;
        ChangeTime = entityChangeInfo.ChangeTime;
        ChangeType = entityChangeInfo.ChangeType;
        EntityId = entityChangeInfo.EntityId.Truncate(EntityChangeConsts.MaxEntityTypeFullNameLength);
        EntityTypeFullName = entityChangeInfo.EntityTypeFullName.TruncateFromBeginning(EntityChangeConsts.MaxEntityTypeFullNameLength);

        PropertyChanges = entityChangeInfo
                              .PropertyChanges?
                              .Select(p => new EntityPropertyChange(Id, p, tenantId))
                              .ToList() ?? [];

        ExtraProperties = [];
        if (entityChangeInfo.ExtraProperties != null)
        {
            foreach (var pair in entityChangeInfo.ExtraProperties)
            {
                ExtraProperties.Add(pair.Key, pair.Value);
            }
        }
    }
}

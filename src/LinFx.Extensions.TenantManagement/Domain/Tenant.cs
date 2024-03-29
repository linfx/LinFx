using LinFx.Domain.Entities.Auditing;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 租户
/// </summary>
public class Tenant : FullAuditedAggregateRoot<string>
{
    /// <summary>
    /// 租户名称
    /// </summary>
    public virtual string Name { get; set; } = string.Empty;

    public Tenant() { }

    public Tenant(string id, string name)
    {
        Id = id;
        SetName(name);
    }

    protected internal virtual void SetName(string name) => Name = name;
}

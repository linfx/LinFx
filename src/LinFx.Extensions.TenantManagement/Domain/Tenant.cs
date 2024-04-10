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
    public virtual required string Name { get; set; }
}

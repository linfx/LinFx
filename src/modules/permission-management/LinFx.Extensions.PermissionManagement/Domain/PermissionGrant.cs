using LinFx.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 权限授权
/// </summary>
public class PermissionGrant : Entity<long>, IMultiTenant
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [StringLength(36)]
    public virtual string TenantId { get; protected set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [StringLength(64)]
    public virtual string Name { get; protected set; }

    /// <summary>
    /// 权限提供器
    /// </summary>
    [Required]
    [StringLength(64)]
    public virtual string ProviderName { get; protected set; }

    /// <summary>
    /// 关键值
    /// </summary>
    [StringLength(64)]
    public virtual string ProviderKey { get; protected internal set; }

    protected PermissionGrant() { }

    public PermissionGrant(long id, string name, string providerName, string providerKey, string tenantId = default)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ProviderName = providerName;
        ProviderKey = providerKey;
        TenantId = tenantId;
    }
}

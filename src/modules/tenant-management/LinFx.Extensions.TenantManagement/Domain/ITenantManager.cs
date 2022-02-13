using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 租户管理器
/// </summary>
public interface ITenantManager
{
    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [NotNull]
    Task<Tenant> CreateAsync([NotNull] string name);

    /// <summary>
    /// 修改租户名称
    /// </summary>
    /// <param name="tenant"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task ChangeNameAsync([NotNull] Tenant tenant, [NotNull] string name);
}

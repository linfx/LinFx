using LinFx.Extensions.DependencyInjection;

namespace LinFx.Extensions.MultiTenancy;

/// <summary>
/// 租户解析上下文
/// </summary>
public interface ITenantResolveContext : IServiceProviderAccessor
{
    /// <summary>
    /// 租户名称or名称
    /// </summary>
    string? TenantIdOrName { get; set; }

    /// <summary>
    /// 是否处理
    /// </summary>
    bool Handled { get; set; }
}
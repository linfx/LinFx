namespace LinFx.Extensions.MultiTenancy;

/// <summary>
/// 上下文
/// </summary>
public class TenantResolveContext(IServiceProvider serviceProvider) : ITenantResolveContext
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public string? TenantIdOrName { get; set; }

    public bool Handled { get; set; }

    public bool HasResolvedTenantOrHost() => Handled || TenantIdOrName != null;
}
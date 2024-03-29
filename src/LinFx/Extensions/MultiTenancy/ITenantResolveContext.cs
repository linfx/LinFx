using LinFx.Extensions.DependencyInjection;

namespace LinFx.Extensions.MultiTenancy;

public interface ITenantResolveContext : IServiceProviderAccessor
{
    string? TenantIdOrName { get; set; }

    bool Handled { get; set; }
}
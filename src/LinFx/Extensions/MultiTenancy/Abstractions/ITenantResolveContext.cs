using LinFx.Extensions.DependencyInjection;

namespace LinFx.Extensions.MultiTenancy
{
    public interface ITenantResolveContext : IServiceProviderAccessor
    {
        [CanBeNull]
        string TenantIdOrName { get; set; }

        bool Handled { get; set; }
    }
}
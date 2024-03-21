using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Features;
using LinFx.Security.Claims;

namespace LinFx.Extensions.FeatureManagement;

public class EditionFeatureManagementProvider(IFeatureManagementStore store, ICurrentPrincipalAccessor principalAccessor) : FeatureManagementProvider(store), ITransientDependency
{
    public override string Name => EditionFeatureValueProvider.ProviderName;

    protected ICurrentPrincipalAccessor PrincipalAccessor { get; } = principalAccessor;

    protected override Task<string> NormalizeProviderKeyAsync(string providerKey)
    {
        if (providerKey != null)
        {
            return Task.FromResult(providerKey);
        }

        return Task.FromResult(PrincipalAccessor.Principal?.FindEditionId()!);
    }
}

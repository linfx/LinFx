using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Features;
using LinFx.Security.Claims;

namespace LinFx.Extensions.FeatureManagement;

public class DefaultValueFeatureManagementProvider : IFeatureManagementProvider, ISingletonDependency
{
    public string Name => DefaultValueFeatureValueProvider.ProviderName;

    public bool Compatible(string providerName) => providerName == Name;

    public Task<IAsyncDisposable> HandleContextAsync(string providerName, string providerKey) => Task.FromResult<IAsyncDisposable>(result: NullAsyncDisposable.Instance);

    public virtual Task<string?> GetOrNullAsync(FeatureDefinition feature, string providerKey) => Task.FromResult(feature.DefaultValue);

    public virtual Task SetAsync(FeatureDefinition feature, string value, string providerKey)
    {
        throw new LinFxException($"Can not set default value of a feature. It is only possible while defining the feature in a {typeof(IFeatureDefinitionProvider)} implementation.");
    }

    public virtual Task ClearAsync(FeatureDefinition feature, string providerKey)
    {
        throw new LinFxException($"Can not clear default value of a feature. It is only possible while defining the feature in a {typeof(IFeatureDefinitionProvider)} implementation.");
    }
}

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

public class IdentityFeatureManagementProvider(IFeatureManagementStore store, ICurrentPrincipalAccessor principalAccessor) : FeatureManagementProvider(store), ITransientDependency
{
    public override string Name => IdentityFeatureValueProvider.ProviderName;

    protected ICurrentPrincipalAccessor PrincipalAccessor { get; } = principalAccessor;

    protected override Task<string> NormalizeProviderKeyAsync(string providerKey)
    {
        if (providerKey != null)
        {
            return Task.FromResult(providerKey);
        }
        return Task.FromResult(PrincipalAccessor.Principal?.FindUserId()!);
    }
}

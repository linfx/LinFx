using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Features;

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

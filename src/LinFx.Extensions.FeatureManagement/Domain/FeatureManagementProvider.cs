using LinFx.Extensions.Features;

namespace LinFx.Extensions.FeatureManagement;

public abstract class FeatureManagementProvider : IFeatureManagementProvider
{
    public abstract string Name { get; }

    protected IFeatureManagementStore Store { get; }

    protected FeatureManagementProvider(IFeatureManagementStore store) => Store = store;

    public virtual bool Compatible(string providerName) => providerName == Name;

    public virtual Task<IAsyncDisposable> HandleContextAsync(string providerName, string providerKey) => Task.FromResult<IAsyncDisposable>(NullAsyncDisposable.Instance);

    public virtual async Task<string?> GetOrNullAsync(FeatureDefinition feature, string providerKey) => await Store.GetOrNullAsync(feature.Name, Name, await NormalizeProviderKeyAsync(providerKey));

    public virtual async Task SetAsync(FeatureDefinition feature, string value, string providerKey) => await Store.SetAsync(feature.Name, value, Name, await NormalizeProviderKeyAsync(providerKey));

    public virtual async Task ClearAsync(FeatureDefinition feature, string providerKey) => await Store.DeleteAsync(feature.Name, Name, await NormalizeProviderKeyAsync(providerKey));

    protected virtual Task<string> NormalizeProviderKeyAsync(string providerKey) => Task.FromResult(providerKey);
}

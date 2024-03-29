using LinFx.Security.Claims;

namespace LinFx.Extensions.Features;

public class DefaultValueFeatureValueProvider(IFeatureStore settingStore) : FeatureValueProvider(settingStore)
{
    public const string ProviderName = "D";

    public override string Name => ProviderName;

    public override Task<string?> GetOrNullAsync(FeatureDefinition setting) => Task.FromResult(setting.DefaultValue);
}

public class EditionFeatureValueProvider(IFeatureStore featureStore, ICurrentPrincipalAccessor principalAccessor) : FeatureValueProvider(featureStore)
{
    public const string ProviderName = "E";

    public override string Name => ProviderName;

    protected ICurrentPrincipalAccessor PrincipalAccessor = principalAccessor;

    public override async Task<string?> GetOrNullAsync(FeatureDefinition feature)
    {
        var key = PrincipalAccessor.Principal?.FindEditionId();
        if (key == null)
            return null;

        return await FeatureStore.GetOrNullAsync(feature.Name, Name, key);
    }
}

public class IdentityFeatureValueProvider(IFeatureStore featureStore, ICurrentPrincipalAccessor principalAccessor) : FeatureValueProvider(featureStore)
{
    public const string ProviderName = "U";

    public override string Name => ProviderName;

    protected ICurrentPrincipalAccessor PrincipalAccessor = principalAccessor;

    public override async Task<string?> GetOrNullAsync(FeatureDefinition feature)
    {
        var key = PrincipalAccessor.Principal?.FindUserId();
        if (key == null)
            return null;

        return await FeatureStore.GetOrNullAsync(feature.Name, Name, key);
    }
}
using LinFx.Security.Claims;

namespace LinFx.Extensions.Features;

public class EditionFeatureValueProvider(IFeatureStore featureStore, ICurrentPrincipalAccessor principalAccessor) : FeatureValueProvider(featureStore)
{
    public const string ProviderName = "E";

    public override string Name => ProviderName;

    protected ICurrentPrincipalAccessor PrincipalAccessor = principalAccessor;

    public override async Task<string?> GetOrNullAsync(FeatureDefinition feature)
    {
        var editionId = PrincipalAccessor.Principal?.FindEditionId();
        if (editionId == null)
            return null;

        return await FeatureStore.GetOrNullAsync(feature.Name, Name, editionId);
    }
}
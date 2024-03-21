namespace LinFx.Extensions.Features;

public class DefaultValueFeatureValueProvider(IFeatureStore settingStore) : FeatureValueProvider(settingStore)
{
    public const string ProviderName = "D";

    public override string Name => ProviderName;

    public override Task<string?> GetOrNullAsync(FeatureDefinition setting) => Task.FromResult(setting.DefaultValue);
}

namespace LinFx.Extensions.FeatureManagement;

[Serializable]
public class FeatureValueProviderInfo(string name, string key)
{
    public string Name { get; } = name;

    public string Key { get; } = key;
}

using LinFx;

namespace Volo.Abp.FeatureManagement;

[Serializable]
public class FeatureNameValueWithGrantedProvider : NameValue
{
    public FeatureValueProviderInfo Provider { get; set; }

    public FeatureNameValueWithGrantedProvider(string name, string value)
    {
        Name = name;
        Value = value;
    }
}

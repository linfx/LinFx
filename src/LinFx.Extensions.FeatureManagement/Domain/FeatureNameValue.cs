namespace LinFx.Extensions.FeatureManagement;

[Serializable]
public class FeatureNameValue : NameValue
{
    public FeatureNameValue() { }

    public FeatureNameValue(string name, string value) : base(name, value) { }
}

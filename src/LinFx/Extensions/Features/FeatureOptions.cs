using LinFx.Collections;

namespace LinFx.Extensions.Features;

public class FeatureOptions
{
    public ITypeList<IFeatureDefinitionProvider> DefinitionProviders { get; }

    public ITypeList<IFeatureValueProvider> ValueProviders { get; }

    public HashSet<string> DeletedFeatures { get; }

    public HashSet<string> DeletedFeatureGroups { get; }

    public FeatureOptions()
    {
        DefinitionProviders = new TypeList<IFeatureDefinitionProvider>();
        ValueProviders = new TypeList<IFeatureValueProvider>();

        DeletedFeatures = [];
        DeletedFeatureGroups = [];
    }
}

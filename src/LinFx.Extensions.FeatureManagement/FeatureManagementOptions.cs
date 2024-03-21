using LinFx.Collections;

namespace LinFx.Extensions.FeatureManagement;

public class FeatureManagementOptions
{
    public TypeList<IFeatureManagementProvider> Providers { get; }

    public Dictionary<string, string> ProviderPolicies { get; }

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveStaticFeaturesToDatabase { get; set; } = true;

    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsDynamicFeatureStoreEnabled { get; set; }

    public FeatureManagementOptions()
    {
        Providers = [];
        ProviderPolicies = [];
    }
}

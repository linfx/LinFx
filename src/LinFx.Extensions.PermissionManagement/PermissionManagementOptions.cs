using LinFx.Collections;

namespace LinFx.Extensions.PermissionManagement;

public class PermissionManagementOptions
{
    public TypeList<PermissionManagementProvider> ManagementProviders { get; } = [];

    public Dictionary<string, string> ProviderPolicies { get; } = [];
}
using LinFx.Collections;
using LinFx.Extensions.PermissionManagement.Application;

namespace LinFx.Extensions.PermissionManagement;

public class PermissionManagementOptions
{
    public TypeList<IPermissionManagementProvider> ManagementProviders { get; } = [];

    public Dictionary<string, string> ProviderPolicies { get; } = [];
}
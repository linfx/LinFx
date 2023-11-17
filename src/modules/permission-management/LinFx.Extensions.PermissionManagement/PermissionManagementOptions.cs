using LinFx.Collections;
using LinFx.Extensions.PermissionManagement.Application;

namespace LinFx.Extensions.PermissionManagement;

public class PermissionManagementOptions
{
    public ITypeList<IPermissionManagementProvider> ManagementProviders { get; } = new TypeList<IPermissionManagementProvider>();

    public Dictionary<string, string> ProviderPolicies { get; } = new Dictionary<string, string>();
}
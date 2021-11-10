using LinFx.Collections;
using System.Collections.Generic;

namespace LinFx.Extensions.PermissionManagement
{
    public class PermissionManagementOptions
    {
        public ITypeList<IPermissionManagementProvider> ManagementProviders { get; } = new TypeList<IPermissionManagementProvider>();

        public Dictionary<string, string> ProviderPolicies { get; } = new Dictionary<string, string>();
    }
}
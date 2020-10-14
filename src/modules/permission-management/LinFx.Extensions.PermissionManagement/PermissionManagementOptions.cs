using LinFx.Collections;
using System.Collections.Generic;

namespace LinFx.Extensions.PermissionManagement
{
    public class PermissionManagementOptions
    {
        //TODO: rename to Providers
        public ITypeList<IPermissionManagementProvider> ManagementProviders { get; }

        public Dictionary<string, string> ProviderPolicies { get; }

        public PermissionManagementOptions()
        {
            ManagementProviders = new TypeList<IPermissionManagementProvider>();
            ProviderPolicies = new Dictionary<string, string>();
        }
    }
}
using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.Guids;
using LinFx.Extensions.MultiTenancy;

namespace LinFx.Extensions.PermissionManagement
{
    public class UserPermissionManagementProvider : PermissionManagementProvider
    {
        public override string Name => UserPermissionValueProvider.ProviderName;

        public UserPermissionManagementProvider(
            IPermissionGrantRepository permissionGrantRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
            : base(
                permissionGrantRepository,
                guidGenerator,
                currentTenant)
        {
        }
    }
}
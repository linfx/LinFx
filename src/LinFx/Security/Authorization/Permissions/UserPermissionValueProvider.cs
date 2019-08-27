using LinFx.Security.Claims;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    public class UserPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "User";

        public override string Name => ProviderName;

        public UserPermissionValueProvider(IPermissionStore permissionStore)
            : base(permissionStore)
        {
        }

        public override async Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionValueCheckContext context)
        {
            var userId = context.Principal?.FindFirst(ClaimTypes.Id)?.Value;

            if (userId == null)
            {
                return PermissionValueProviderGrantInfo.NonGranted;
            }

            if (await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, userId))
            {
                return new PermissionValueProviderGrantInfo(true, userId);
            }

            return PermissionValueProviderGrantInfo.NonGranted;
        }
    }
}

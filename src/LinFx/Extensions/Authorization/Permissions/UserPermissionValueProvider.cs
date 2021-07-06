using LinFx.Security.Claims;
using System.Threading.Tasks;

namespace LinFx.Extensions.Authorization.Permissions
{
    /// <summary>
    /// 用户授权提供者
    /// </summary>
    public class UserPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "U";

        public override string Name => ProviderName;

        public UserPermissionValueProvider(IPermissionStore permissionStore)
            : base(permissionStore) { }

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            var userId = context.Principal?.FindFirst(ClaimTypes.Id)?.Value;
            if (userId == null)
                return PermissionGrantResult.Undefined;

            return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, userId)
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }
    }
}

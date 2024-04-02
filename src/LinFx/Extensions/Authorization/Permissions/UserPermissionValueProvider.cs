using LinFx.Security.Claims;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 用户授权提供者
/// </summary>
public class UserPermissionValueProvider(IPermissionStore permissionStore) : PermissionValueProvider(permissionStore)
{
    public const string ProviderName = "U";

    public override string Name => ProviderName;

    public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var id = context.Principal?.FindFirst(ClaimTypes.Id)?.Value;
        if (id == null)
            return PermissionGrantResult.Undefined;

        return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, id) ? PermissionGrantResult.Granted : PermissionGrantResult.Undefined;
    }
}

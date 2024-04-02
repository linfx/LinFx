using LinFx.Security.Claims;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 客户端提供者
/// </summary>
public class ClientPermissionValueProvider(IPermissionStore permissionStore) : PermissionValueProvider(permissionStore)
{
    public const string ProviderName = "C";

    public override string Name => ProviderName;

    public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var id = context.Principal?.FindFirst(ClaimTypes.ClientId)?.Value;
        if (id == null)
            return PermissionGrantResult.Undefined;

        return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, id) ? PermissionGrantResult.Granted : PermissionGrantResult.Undefined;
    }
}

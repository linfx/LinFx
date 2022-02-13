using LinFx.Security.Claims;
using System.Threading.Tasks;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// 客户端提供者
/// </summary>
public class ClientPermissionValueProvider : PermissionValueProvider
{
    public const string ProviderName = "C";

    public override string Name => ProviderName;

    public ClientPermissionValueProvider(IPermissionStore permissionStore)
        : base(permissionStore) { }

    public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var clientId = context.Principal?.FindFirst(ClaimTypes.ClientId)?.Value;
        if (clientId == null)
            return PermissionGrantResult.Undefined;

        return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, clientId)
            ? PermissionGrantResult.Granted
            : PermissionGrantResult.Undefined;
    }
}

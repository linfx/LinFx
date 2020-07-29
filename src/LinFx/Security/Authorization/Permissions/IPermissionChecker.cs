using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    /// <summary>
    /// 权限检查器
    /// </summary>
    public interface IPermissionChecker
    {
        Task<PermissionGrantInfo> CheckAsync([NotNull] string name);

        Task<PermissionGrantInfo> CheckAsync(ClaimsPrincipal claimsPrincipal, [NotNull] string name);
    }
}
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    public interface IPermissionChecker
    {
        Task<PermissionGrantInfo> CheckAsync([NotNull]string name);

        Task<PermissionGrantInfo> CheckAsync([CanBeNull] ClaimsPrincipal claimsPrincipal, [NotNull]string name);
    }
}
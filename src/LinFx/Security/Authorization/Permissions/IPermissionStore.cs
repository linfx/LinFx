using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    public interface IPermissionStore
    {
        Task<bool> IsGrantedAsync(
            [NotNull] string name,
            [CanBeNull] string providerName,
            [CanBeNull] string providerKey
        );
    }
}
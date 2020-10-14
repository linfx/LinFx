using LinFx.Extensions.Authorization.Permissions;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement
{
    public interface IPermissionManagementProvider
    {
        string Name { get; }

        Task<PermissionValueProviderGrantInfo> CheckAsync([NotNull] string name, [NotNull] string providerName, [NotNull] string providerKey);

        Task SetAsync([NotNull] string name, [NotNull] string providerKey, bool isGranted);
    }
}
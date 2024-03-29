using LinFx.Extensions.Authorization.Permissions;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.PermissionManagement.Application;

public interface IPermissionManagementProvider
{
    string Name { get; }

    Task<PermissionValueProviderGrantInfo> CheckAsync(
        [NotNull] string name,
        [NotNull] string providerName,
        [NotNull] string providerKey
    );

    Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(
        [NotNull] string[] names,
        [NotNull] string providerName,
        [NotNull] string providerKey
    );

    Task SetAsync(
        [NotNull] string name,
        [NotNull] string providerKey,
        bool isGranted
    );
}

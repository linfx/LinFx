using LinFx.Extensions.Authorization.Permissions;

namespace LinFx.Extensions.PermissionManagement;

public interface IPermissionManagementProvider
{
    string Name { get; }

    Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey);

    Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey);

    Task SetAsync(string name, string providerKey, bool isGranted);
}

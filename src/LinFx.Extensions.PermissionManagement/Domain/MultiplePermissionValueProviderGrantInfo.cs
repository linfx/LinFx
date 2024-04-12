using LinFx.Extensions.Authorization.Permissions;

namespace LinFx.Extensions.PermissionManagement;

public class MultiplePermissionValueProviderGrantInfo
{
    public Dictionary<string, PermissionValueProviderGrantInfo> Result { get; } = [];

    public MultiplePermissionValueProviderGrantInfo(string[] names)
    {
        foreach (var name in names)
        {
            Result.Add(name, PermissionValueProviderGrantInfo.NonGranted);
        }
    }
}

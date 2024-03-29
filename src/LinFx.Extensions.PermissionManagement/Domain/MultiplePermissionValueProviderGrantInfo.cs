using LinFx.Extensions.Authorization.Permissions;

namespace LinFx.Extensions.PermissionManagement;

public class MultiplePermissionValueProviderGrantInfo
{
    public Dictionary<string, PermissionValueProviderGrantInfo> Result { get; }

    public MultiplePermissionValueProviderGrantInfo()
    {
        Result = [];
    }

    public MultiplePermissionValueProviderGrantInfo(string[] names)
    {
        Result = [];

        foreach (var name in names)
        {
            Result.Add(name, PermissionValueProviderGrantInfo.NonGranted);
        }
    }
}

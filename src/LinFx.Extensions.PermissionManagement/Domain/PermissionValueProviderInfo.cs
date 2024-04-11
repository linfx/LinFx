namespace LinFx.Extensions.PermissionManagement;

public class PermissionValueProviderInfo(string name, string key)
{
    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    public string Key { get; } = key ?? throw new ArgumentNullException(nameof(key));
}

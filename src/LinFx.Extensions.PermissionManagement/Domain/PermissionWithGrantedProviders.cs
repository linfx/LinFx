namespace LinFx.Extensions.PermissionManagement;

public class PermissionWithGrantedProviders(string name, bool isGranted)
{
    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    public bool IsGranted { get; set; } = isGranted;

    public List<PermissionValueProviderInfo> Providers { get; set; } = [];
}

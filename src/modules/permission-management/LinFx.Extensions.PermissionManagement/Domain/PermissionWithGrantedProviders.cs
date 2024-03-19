using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.PermissionManagement;

public class PermissionWithGrantedProviders
{
    public string Name { get; }

    public bool IsGranted { get; set; }

    public List<PermissionValueProviderInfo> Providers { get; set; }

    public PermissionWithGrantedProviders([NotNull] string name, bool isGranted)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IsGranted = isGranted;
        Providers = [];
    }
}

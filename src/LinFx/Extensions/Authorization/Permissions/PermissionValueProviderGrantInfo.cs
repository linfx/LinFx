namespace LinFx.Extensions.Authorization.Permissions;

public class PermissionValueProviderGrantInfo(bool isGranted, string? providerKey = default)
{
    public static PermissionValueProviderGrantInfo NonGranted { get; } = new PermissionValueProviderGrantInfo(false);

    public virtual bool IsGranted { get; } = isGranted;

    public virtual string? ProviderKey { get; } = providerKey;
}

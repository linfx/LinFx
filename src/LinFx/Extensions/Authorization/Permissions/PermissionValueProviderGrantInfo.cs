namespace LinFx.Extensions.Authorization.Permissions
{
    public class PermissionValueProviderGrantInfo
    {
        public static PermissionValueProviderGrantInfo NonGranted { get; } = new PermissionValueProviderGrantInfo(false);

        public virtual bool IsGranted { get; }

        public virtual string ProviderKey { get; }

        public PermissionValueProviderGrantInfo(bool isGranted, string providerKey = null)
        {
            IsGranted = isGranted;
            ProviderKey = providerKey;
        }
    }
}
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Security.Authorization.Permissions
{
    public class PermissionGrantInfo
    {
        /// <summary>
        /// 资源唯一ID
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 是否授权
        /// </summary>
        public bool IsGranted { get; }

        public string ProviderName { get; }

        public string ProviderKey { get; }

        public PermissionGrantInfo([NotNull] string name, bool isGranted, [CanBeNull] string providerName = null, [CanBeNull] string providerKey = null)
        {
            Check.NotNull(name, nameof(name));

            Name = name;
            IsGranted = isGranted;
            ProviderName = providerName;
            ProviderKey = providerKey;
        }
    }
}

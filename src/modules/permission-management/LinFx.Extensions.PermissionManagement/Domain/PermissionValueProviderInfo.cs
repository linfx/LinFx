using System;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.PermissionManagement
{
    public class PermissionValueProviderInfo
    {
        public string Name { get; }

        public string Key { get; }

        public PermissionValueProviderInfo([NotNull] string name, [NotNull] string key)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }
    }
}
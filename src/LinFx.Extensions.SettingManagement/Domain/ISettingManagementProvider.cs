using JetBrains.Annotations;
using LinFx.Extensions.Setting;

namespace LinFx.Extensions.SettingManagement
{
    public interface ISettingManagementProvider
    {
        string Name { get; }

        Task<string> GetOrNullAsync([NotNull] SettingDefinition setting, [CanBeNull] string providerKey);

        Task SetAsync([NotNull] SettingDefinition setting, [NotNull] string value, [CanBeNull] string providerKey);

        Task ClearAsync([NotNull] SettingDefinition setting, [CanBeNull] string providerKey);
    }
}

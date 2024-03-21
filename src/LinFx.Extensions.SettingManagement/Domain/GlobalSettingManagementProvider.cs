using LinFx.Extensions.Setting;

namespace LinFx.Extensions.SettingManagement
{
    [Service]
    public class GlobalSettingManagementProvider : SettingManagementProvider
    {
        public override string Name => GlobalSettingValueProvider.ProviderName;

        public GlobalSettingManagementProvider(ISettingManagementStore settingManagementStore)
            : base(settingManagementStore)
        {
        }

        protected override string NormalizeProviderKey(string providerKey)
        {
            return null;
        }
    }
}
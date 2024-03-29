using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Setting;

namespace LinFx.Extensions.SettingManagement
{
    [Service]
    public class TenantSettingManagementProvider : SettingManagementProvider
    {
        public override string Name => TenantSettingValueProvider.ProviderName;

        protected ICurrentTenant CurrentTenant { get; }

        public TenantSettingManagementProvider(
            ISettingManagementStore settingManagementStore,
            ICurrentTenant currentTenant)
            : base(settingManagementStore)
        {
            CurrentTenant = currentTenant;
        }

        protected override string NormalizeProviderKey(string providerKey)
        {
            if (providerKey != null)
            {
                return providerKey;
            }

            return CurrentTenant.Id?.ToString();
        }
    }
}
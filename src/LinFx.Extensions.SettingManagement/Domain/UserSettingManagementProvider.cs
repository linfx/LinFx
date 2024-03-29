using LinFx.Extensions.Setting;
using LinFx.Security.Users;

namespace LinFx.Extensions.SettingManagement
{
    [Service]
    public class UserSettingManagementProvider : SettingManagementProvider
    {
        public override string Name => UserSettingValueProvider.ProviderName;

        protected ICurrentUser CurrentUser { get; }

        public UserSettingManagementProvider(
            ISettingManagementStore settingManagementStore,
            ICurrentUser currentUser)
            : base(settingManagementStore)
        {
            CurrentUser = currentUser;
        }

        protected override string NormalizeProviderKey(string providerKey)
        {
            if (providerKey != null)
            {
                return providerKey;
            }

            return CurrentUser.Id?.ToString();
        }
    }
}
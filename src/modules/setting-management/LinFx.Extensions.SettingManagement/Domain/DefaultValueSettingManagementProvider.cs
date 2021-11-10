using LinFx.Extensions.Setting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.SettingManagement
{
    [Service(ServiceLifetime.Singleton)]
    public class DefaultValueSettingManagementProvider : ISettingManagementProvider
    {
        public string Name => DefaultValueSettingValueProvider.ProviderName;

        public virtual Task<string> GetOrNullAsync(SettingDefinition setting, string providerKey)
        {
            return Task.FromResult(setting.DefaultValue);
        }

        public virtual Task SetAsync(SettingDefinition setting, string value, string providerKey)
        {
            throw new Exception($"Can not set default value of a setting. It is only possible while defining the setting in a {typeof(ISettingDefinitionProvider)} implementation.");
        }

        public virtual Task ClearAsync(SettingDefinition setting, string providerKey)
        {
            throw new Exception($"Can not clear default value of a setting. It is only possible while defining the setting in a {typeof(ISettingDefinitionProvider)} implementation.");
        }
    }
}
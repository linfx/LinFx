using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Setting;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.SettingManagement
{
    [Service]
    public class ConfigurationSettingManagementProvider : ISettingManagementProvider
    {
        public string Name => ConfigurationSettingValueProvider.ProviderName;

        protected IConfiguration Configuration { get; }

        public ConfigurationSettingManagementProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual Task<string> GetOrNullAsync(SettingDefinition setting, string providerKey)
        {
            return Task.FromResult(Configuration[ConfigurationSettingValueProvider.ConfigurationNamePrefix + setting.Name]);
        }

        public virtual Task SetAsync(SettingDefinition setting, string value, string providerKey)
        {
            throw new Exception($"Can not set a setting value to the application configuration.");
        }

        public virtual Task ClearAsync(SettingDefinition setting, string providerKey)
        {
            throw new Exception($"Can not set a setting value to the application configuration.");
        }
    }
}
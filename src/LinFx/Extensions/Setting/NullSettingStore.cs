using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.Setting
{
    [Service(ServiceLifetime.Singleton)]
    public class NullSettingStore : ISettingStore
    {
        public ILogger<NullSettingStore> Logger { get; set; }

        public NullSettingStore()
        {
            Logger = NullLogger<NullSettingStore>.Instance;
        }

        public Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            return Task.FromResult((string)null);
        }

        public Task<List<SettingValue>> GetAllAsync(string[] names, string providerName, string providerKey)
        {
            return Task.FromResult(names.Select(x => new SettingValue(x, null)).ToList());
        }
    }
}

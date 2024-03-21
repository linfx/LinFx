using JetBrains.Annotations;
using LinFx.Extensions.Setting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.SettingManagement
{
    public interface ISettingManager
    {
        Task<string> GetOrNullAsync([NotNull] string name, [NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

        Task<List<SettingValue>> GetAllAsync([NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

        Task SetAsync([NotNull] string name, [CanBeNull] string value, [NotNull] string providerName, [CanBeNull] string providerKey, bool forceToSet = false);
    }
}
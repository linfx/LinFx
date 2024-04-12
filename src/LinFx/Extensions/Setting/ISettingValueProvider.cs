using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Setting
{
    public interface ISettingValueProvider
    {
        string Name { get; }

        Task<string> GetOrNullAsync([NotNull] SettingDefinition setting);

        Task<List<SettingValue>> GetAllAsync([NotNull] SettingDefinition[] settings);
    }
}

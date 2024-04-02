using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Setting
{
    public interface ISettingStore
    {
        Task<string> GetOrNullAsync(
            [NotNull] string name,
            [AllowNull] string providerName,
            [AllowNull] string providerKey
        );

        Task<List<SettingValue>> GetAllAsync(
            [NotNull] string[] names,
            [AllowNull] string providerName,
            [AllowNull] string providerKey
        );
    }
}

using LinFx.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.SettingManagement
{
    public interface ISettingRepository : IBasicRepository<Setting, string>
    {
        Task<Setting> FindAsync(
            string name,
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default);

        Task<List<Setting>> GetListAsync(
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default);

        Task<List<Setting>> GetListAsync(
            string[] names,
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default);
    }
}

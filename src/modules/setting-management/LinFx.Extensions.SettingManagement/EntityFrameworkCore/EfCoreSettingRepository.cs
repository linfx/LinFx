using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.EntityFrameworkCore.Repositories;
using LinFx.Extensions.SettingManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.SettingManagement.EntityFrameworkCore
{
    public class EfCoreSettingRepository : EfCoreRepository<ISettingManagementDbContext, Setting, string>, ISettingRepository
    {
        public EfCoreSettingRepository(IServiceProvider serviceProvider, IDbContextProvider<ISettingManagementDbContext> dbContextProvider)
            : base(serviceProvider, dbContextProvider)
        {
        }

        public virtual async Task<Setting> FindAsync(
            string name,
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(
                    s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Setting>> GetListAsync(
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(
                    s => s.ProviderName == providerName && s.ProviderKey == providerKey
                ).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Setting>> GetListAsync(
            string[] names,
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(
                    s => names.Contains(s.Name) && s.ProviderName == providerName && s.ProviderKey == providerKey
                ).ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}

using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement.EntityFrameworkCore;

[Service]
public class EfPermissionGrantRepository : EfRepository<PermissionManagementDbContext, PermissionGrant, long>, IPermissionGrantRepository
{
    public EfPermissionGrantRepository(
        IServiceProvider serviceProvider,
        IDbContextProvider<PermissionManagementDbContext> dbContextProvider)
        : base(serviceProvider, dbContextProvider)
    {
    }

    public virtual async Task<PermissionGrant> FindAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(s =>
                s.Name == name &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey,
                GetCancellationToken(cancellationToken)
            );
    }

    public virtual async Task<List<PermissionGrant>> GetListAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<PermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                names.Contains(s.Name) &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }
}

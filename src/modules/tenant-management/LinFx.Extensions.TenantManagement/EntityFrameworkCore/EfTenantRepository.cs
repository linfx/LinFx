﻿using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.TenantManagement.EntityFrameworkCore;

[Service]
public class EfTenantRepository : EfRepository<TenantManagementDbContext, Tenant, string>, ITenantRepository
{
    public EfTenantRepository(
        IServiceProvider serviceProvider,
        IDbContextProvider<TenantManagementDbContext> dbContextProvider)
        : base(serviceProvider, dbContextProvider)
    { }

    public virtual async Task<Tenant> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            //.IncludeDetails(includeDetails)
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync(t => t.Name == name, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Tenant>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            //.IncludeDetails(includeDetails)
            .WhereIf(!filter.IsNullOrWhiteSpace(), u => u.Name.Contains(filter))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(Tenant.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        => await (await GetDbSetAsync()).WhereIf(!filter.IsNullOrWhiteSpace(), u => u.Name.Contains(filter)).CountAsync(cancellationToken: cancellationToken);
}

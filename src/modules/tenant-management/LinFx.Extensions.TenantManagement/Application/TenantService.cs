using LinFx.Application.Dtos;
using LinFx.Application.Services;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 租户服务
/// </summary>
[Authorize(TenantManagementPermissions.Tenants.Default)]
public class TenantService : ApplicationService, ITenantService
{
    protected TenantManagementDbContext Db { get; }
    protected ITenantManager TenantManager { get; }

    public TenantService(
        TenantManagementDbContext tenantRepository,
        ITenantManager tenantManager)
    {
        Db = tenantRepository;
        TenantManager = tenantManager;
    }

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async ValueTask<TenantDto> GetAsync(string id)
    {
        var item = await Db.Tenants.FindAsync(id);
        return item.MapTo<TenantDto>();
    }

    /// <summary>
    /// 获取租户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async ValueTask<PagedResult<TenantDto>> GetListAsync(TenantRequest input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Tenant.Name);
        }

        var count = await Db.Tenants.CountAsync();
        var items = await Db.Tenants.ToListAsync();

        return new PagedResult<TenantDto>(count, items.MapTo<List<TenantDto>>());
    }

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.Create)]
    public virtual async ValueTask<TenantDto> CreateAsync(TenantEditInput input)
    {
        var tenant = await TenantManager.CreateAsync(input.Name);

        Db.Tenants.Add(tenant);
        await Db.SaveChangesAsync();

        return tenant.MapTo<TenantDto>();
    }

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.Update)]
    public virtual async ValueTask<TenantDto> UpdateAsync(string id, TenantEditInput input)
    {
        var item = await Db.Tenants.FindAsync(id);
        await TenantManager.ChangeNameAsync(item, input.Name);
        await Db.SaveChangesAsync();

        return item.MapTo<TenantDto>();
    }

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.Delete)]
    public virtual async ValueTask DeleteAsync(string id)
    {
        var tenant = await Db.Tenants.FindAsync(id);
        if (tenant == null)
            return;

        await Db.SaveChangesAsync();
    }
}

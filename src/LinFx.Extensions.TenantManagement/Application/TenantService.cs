using LinFx.Application.Dtos;
using LinFx.Application.Services;
using LinFx.Domain.Entities;
using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 租户服务
/// </summary>
[Authorize(TenantManagementPermissions.Tenants.Default)]
public class TenantService(TenantManagementDbContext tenantRepository) : ApplicationService
{
    protected TenantManagementDbContext Db { get; } = tenantRepository;

    /// <summary>
    /// 获取租户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual ValueTask<PagedResult<TenantDto>> GetListAsync(TenantRequest input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Tenant.Name);
        }
        return Db.Tenants.ToPageResultAsync<Tenant, TenantDto>(input);
    }

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async ValueTask<TenantDto> GetAsync(string id)
    {
        var item = await Db.Tenants.FindAsync(id);
        if (item == null)
            throw new EntityNotFoundException(typeof(Tenant));

        return item.MapTo<TenantDto>();
    }

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.Create)]
    public virtual async ValueTask<TenantDto> CreateAsync(TenantEditInput input)
    {
        if (await Db.Tenants.AnyAsync(p => p.Name == input.Name))
            throw new ArgumentException(nameof(input.Name));

        var tenant = new Tenant
        {
            Name = input.Name
        };

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
        if (item == null)
            throw new EntityNotFoundException(typeof(Tenant), id);

        item.Name = input.Name;
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

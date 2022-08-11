using LinFx.Application.Dtos;
using LinFx.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 租户服务
/// </summary>
[Authorize(TenantManagementPermissions.Tenants.Default)]
public class TenantService : ApplicationService, ITenantService
{
    protected ITenantRepository TenantRepository { get; }
    protected ITenantManager TenantManager { get; }

    public TenantService(
        ITenantRepository tenantRepository,
        ITenantManager tenantManager)
    {
        TenantRepository = tenantRepository;
        TenantManager = tenantManager;
    }

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async ValueTask<TenantDto> GetAsync(string id)
    {
        var tenant = await TenantRepository.GetAsync(id);
        return tenant.MapTo<TenantDto>();
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

        var count = await TenantRepository.GetCountAsync(input.Filter);
        var items = await TenantRepository.GetPagedListAsync(input.Page, input.PageSize, input.Sorting);

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
        tenant = await TenantRepository.InsertAsync(tenant);

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
        var tenant = await TenantRepository.GetAsync(id);
        await TenantManager.ChangeNameAsync(tenant, input.Name);
        await TenantRepository.UpdateAsync(tenant);

        return tenant.MapTo<TenantDto>();
    }

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.Delete)]
    public virtual async ValueTask DeleteAsync(string id)
    {
        var tenant = await TenantRepository.FindAsync(id);
        if (tenant == null)
            return;

        await TenantRepository.DeleteAsync(tenant);
    }
}

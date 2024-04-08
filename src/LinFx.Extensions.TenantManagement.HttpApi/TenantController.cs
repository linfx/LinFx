using LinFx.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LinFx.Extensions.TenantManagement.HttpApi;

/// <summary>
/// 租户管理
/// </summary>
[ApiController]
[Route("api/multi-tenancy/[controller]")]
public class TenantController(ITenantService tenantService) : ControllerBase
{
    protected ITenantService TenantService { get; } = tenantService;

    /// <summary>
    /// 租户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/api/multi-tenancy/tenants")]
    public virtual ValueTask<PagedResult<TenantDto>> GetListAsync([FromQuery] TenantRequest input) => TenantService.GetListAsync(input);

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="id">租户Id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public virtual ValueTask<TenantDto> GetAsync(string id) => TenantService.GetAsync(id);

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public virtual ValueTask<TenantDto> CreateAsync(TenantEditInput input) => TenantService.CreateAsync(input);

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="id">租户Id</param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public virtual ValueTask<TenantDto> UpdateAsync(string id, TenantEditInput input) => TenantService.UpdateAsync(id, input);

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="id">租户Id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public virtual ValueTask DeleteAsync(string id) => TenantService.DeleteAsync(id);
}
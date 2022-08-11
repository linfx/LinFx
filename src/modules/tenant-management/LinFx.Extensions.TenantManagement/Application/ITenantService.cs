using LinFx.Application.Dtos;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 租户服务
/// </summary>
public interface ITenantService
{
    /// <summary>
    /// 租户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    ValueTask<PagedResult<TenantDto>> GetListAsync(TenantRequest input);

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    ValueTask<TenantDto> GetAsync(string id);

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    ValueTask<TenantDto> CreateAsync(TenantEditInput input);

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    ValueTask<TenantDto> UpdateAsync(string id, TenantEditInput input);

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    ValueTask DeleteAsync(string id);
}

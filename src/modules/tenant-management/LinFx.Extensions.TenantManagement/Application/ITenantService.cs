using LinFx.Application.Dtos;
using System.Threading.Tasks;

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
    Task<PagedResult<TenantDto>> GetListAsync(TenantRequest input);

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TenantDto> GetAsync(string id);

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<TenantDto> CreateAsync(TenantEditInput input);

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<TenantDto> UpdateAsync(string id, TenantEditInput input);

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(string id);
}

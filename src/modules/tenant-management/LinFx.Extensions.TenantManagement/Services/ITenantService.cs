using LinFx.Application.Models;
using LinFx.Module.TenantManagement.ViewModels;
using System.Threading.Tasks;

namespace LinFx.Module.TenantManagement.Services
{
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
        Task<PagedResult<TenantResult>> GetListAsync(TenantInput input);

        /// <summary>
        /// 获取租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TenantResult> GetAsync(string id);

        /// <summary>
        /// 创建租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TenantResult> CreateAsync(TenantCreateInput input);

        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TenantResult> UpdateAsync(string id, TenantUpdateInput input);

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(string id);
    }
}

using LinFx.Application;
using LinFx.Application.Models;
using LinFx.Data;
using LinFx.Data.Linq;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.TenantManagement
{
    /// <summary>
    /// 租户服务
    /// </summary>
    [Service]
    [Authorize(TenantManagementPermissions.Tenants.Default)]
    public class TenantService : ApplicationService, ITenantService
    {
        protected IRepository<Tenant> TenantRepository { get; }

        protected TenantManager TenantManager { get; }

        /// <summary>
        /// 获取租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TenantDto> GetAsync(string id)
        {
            var tenant = await TenantRepository.FirstOrDefaultAsync(p => p.Id == id);
            return tenant?.MapTo<TenantDto>();
        }

        /// <summary>
        /// 获取租户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResult<TenantDto>> GetListAsync(TenantInput input)
        {
            return await TenantRepository.Query().ToPageResultAsync<Tenant, TenantDto>(input);
        }

        /// <summary>
        /// 创建租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(TenantManagementPermissions.Tenants.Create)]
        public virtual async Task<TenantDto> CreateAsync(TenantEditInput input)
        {
            var tenant = await TenantManager.CreateAsync(input.Name);

            TenantRepository.Add(tenant);
            await TenantRepository.SaveChangesAsync();

            return tenant.MapTo<TenantDto>();
        }

        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(TenantManagementPermissions.Tenants.Update)]
        public virtual async Task<TenantDto> UpdateAsync(string id, TenantEditInput input)
        {
            var tenant = await TenantRepository.FirstOrDefaultAsync(p => p.Id == id);
            if (tenant == null)
                throw new Exception("tenant 不存在");

            input.MapTo(tenant);
            await TenantRepository.SaveChangesAsync();
            return tenant.MapTo<TenantDto>();
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(TenantManagementPermissions.Tenants.Delete)]
        public virtual async Task DeleteAsync(string id)
        {
            var tenant = await TenantRepository.FirstOrDefaultAsync(p => p.Id == id);
            if (tenant == null)
                return;

            TenantRepository.Remove(tenant);
            await TenantRepository.SaveChangesAsync();
        }
    }
}
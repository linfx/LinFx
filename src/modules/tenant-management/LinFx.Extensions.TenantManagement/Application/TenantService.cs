using LinFx.Application;
using LinFx.Application.Models;
using LinFx.Data;
using LinFx.Data.Linq;
using Microsoft.AspNetCore.Authorization;
using System;
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

        public async Task<TenantDto> GetAsync(string id)
        {
            var tenant = await TenantRepository.FirstOrDefaultAsync(p => p.Id == id);
            return tenant?.MapTo<TenantDto>();
        }

        public Task<PagedResult<TenantDto>> GetListAsync(TenantInput input)
        {
            throw new System.NotImplementedException();
        }

        [Authorize(TenantManagementPermissions.Tenants.Create)]
        public Task<TenantDto> CreateAsync(TenantEditInput input)
        {
            throw new System.NotImplementedException();
        }

        [Authorize(TenantManagementPermissions.Tenants.Update)]
        public async Task<TenantDto> UpdateAsync(string id, TenantEditInput input)
        {
            var tenant = await TenantRepository.FirstOrDefaultAsync(p => p.Id == id);
            if (tenant == null)
                throw new Exception("tenant 不存在");

            input.MapTo(tenant);
            await TenantRepository.SaveChangesAsync();
            return tenant.MapTo<TenantDto>();
        }

        [Authorize(TenantManagementPermissions.Tenants.Delete)]
        public async Task DeleteAsync(string id)
        {
            var tenant = await TenantRepository.FirstOrDefaultAsync(p => p.Id == id);
            if (tenant == null)
                return;

            TenantRepository.Remove(tenant);
            await TenantRepository.SaveChangesAsync();
        }
    }
}
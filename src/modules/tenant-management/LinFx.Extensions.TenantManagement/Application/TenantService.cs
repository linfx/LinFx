using LinFx.Application.Dtos;
using LinFx.Application.Services;
using LinFx.Extensions.Uow;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.TenantManagement
{
    /// <summary>
    /// 租户服务
    /// </summary>
    [Authorize(TenantManagementPermissions.Tenants.Default)]
    public class TenantService : ApplicationService, ITenantService
    {
        protected ITenantRepository TenantRepository { get; }
        protected ITenantManager TenantManager { get; }

        public TenantService(
            IServiceProvider serviceProvider,
            ITenantRepository tenantRepository,
            ITenantManager tenantManager) : base(serviceProvider)
        {
            TenantRepository = tenantRepository;
            TenantManager = tenantManager;
        }

        /// <summary>
        /// 获取租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TenantDto> GetAsync(string id)
        {
            using var uow = UnitOfWorkManager.Begin();

            var tenant = await TenantRepository.GetAsync(id);

            await uow.CompleteAsync();
            return tenant.MapTo<TenantDto>();
        }

        /// <summary>
        /// 获取租户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResult<TenantDto>> GetListAsync(TenantRequest input)
        {
            using var uow = UnitOfWorkManager.Begin();

            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Tenant.Name);
            }

            var count = await TenantRepository.GetCountAsync(input.Filter);
            var items = await TenantRepository.GetPagedListAsync(input.Page, input.PageSize, input.Sorting);

            await uow.CompleteAsync();
            return new PagedResult<TenantDto>(count, items.MapTo<List<TenantDto>>());
        }

        /// <summary>
        /// 创建租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(TenantManagementPermissions.Tenants.Create)]
        public virtual async Task<TenantDto> CreateAsync(TenantEditInput input)
        {
            using var uow = UnitOfWorkManager.Begin();

            var tenant = await TenantManager.CreateAsync(input.Name);
            await TenantRepository.InsertAsync(tenant);

            await uow.CompleteAsync();
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
            using var uow = UnitOfWorkManager.Begin();

            var tenant = await TenantRepository.GetAsync(id);
            await TenantManager.ChangeNameAsync(tenant, input.Name);
            await TenantRepository.UpdateAsync(tenant);

            await uow.CompleteAsync();
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
            using var uow = UnitOfWorkManager.Begin();

            var tenant = await TenantRepository.FindAsync(id);
            if (tenant == null)
                return;

            await TenantRepository.DeleteAsync(tenant);

            await uow.CompleteAsync();
        }
    }
}
using LinFx.Application;
using LinFx.Extensions.DependencyInjection;
using LinFx.Module.TenantManagement.Data;
using LinFx.Module.TenantManagement.Models;
using LinFx.Module.TenantManagement.ViewModels;

namespace LinFx.Module.TenantManagement.Services
{
    /// <summary>
    /// 租户服务
    /// </summary>
    [Service]
    public class TenantService : CrudService<Tenant, TenantResult, TenantResult, string, TenantInput, TenantCreateInput, TenantUpdateInput>, ITenantService
    {
        public TenantService(ServiceContext context, TenantManagementDbContext db)
            : base(context, db)
        {
        }

        protected TenantManagementDbContext Db => (TenantManagementDbContext)_db;
    }
}
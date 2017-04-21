using LinFx.Data;
using LinFx.Domain.Repositories;
using LinFx.Domain.Services;

namespace LinFx.SaaS.MultiTenancy
{
    public class TenantManager : DomainService<Tenant>
    {
        public TenantManager(IRepository<Tenant> repository) : base(repository) { }
    }
}
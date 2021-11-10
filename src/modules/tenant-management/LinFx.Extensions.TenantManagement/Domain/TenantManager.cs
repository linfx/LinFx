using LinFx.Utils;
using System.Threading.Tasks;

namespace LinFx.Extensions.TenantManagement
{
    [Service]
    public class TenantManager : ITenantManager
    {
        protected ITenantRepository TenantRepository { get; }

        public TenantManager(ITenantRepository tenantRepository)
        {
            TenantRepository = tenantRepository;
        }

        /// <summary>
        /// 创建租户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Tenant> CreateAsync(string name)
        {
            Check.NotNull(name, nameof(name));

            await ValidateNameAsync(name);
            return new Tenant(IDUtils.NewId().ToString(), name);
        }

        /// <summary>
        /// 修改名称
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task ChangeNameAsync(Tenant tenant, string name)
        {
            Check.NotNull(tenant, nameof(tenant));
            Check.NotNull(name, nameof(name));

            await ValidateNameAsync(name, tenant.Id);
            tenant.SetName(name);
        }

        protected virtual async Task ValidateNameAsync(string name, string expectedId = null)
        {
            var tenant = await TenantRepository.FindByNameAsync(name);
            if (tenant != null && tenant.Id != expectedId)
            {
                throw new UserFriendlyException("Duplicate tenancy name: " + name); //TODO: A domain exception would be better..?
            }
        }
    }
}

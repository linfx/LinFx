using System.Threading.Tasks;

namespace LinFx.Extensions.MultiTenancy
{
    public interface ITenantStore
    {
        Task<TenantInfo> FindAsync(string tenantIdOrName);
    }
}
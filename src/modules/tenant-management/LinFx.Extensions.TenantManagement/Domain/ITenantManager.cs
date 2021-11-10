using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LinFx.Extensions.TenantManagement
{
    public interface ITenantManager
    {
        [NotNull]
        Task<Tenant> CreateAsync([NotNull] string name);

        Task ChangeNameAsync([NotNull] Tenant tenant, [NotNull] string name);
    }
}

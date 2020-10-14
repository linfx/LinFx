using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement
{
    /// <summary>
    /// 权限授权仓储
    /// </summary>
    public interface IPermissionGrantRepository
    {
        Task InsertAsync(PermissionGrant permissionGrant);

        Task DeleteAsync(PermissionGrant permissionGrant);

        Task<PermissionGrant> FindAsync(string name, string providerName, string providerKey);

        Task<List<PermissionGrant>> GetListAsync(string providerName, string providerKey);
    }
}
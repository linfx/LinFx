using LinFx.Extensions.PermissionManagement.Application.Models;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement.Application
{
    public interface IPermissionService
    {
        Task<PermissionListResult> GetAsync(string providerName, string providerKey);

        Task UpdateAsync(string providerName, string providerKey, UpdatePermissionDto input);
    }
}
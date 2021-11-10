using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement
{
    public interface IPermissionService
    {
        Task<PermissionListResultDto> GetAsync(string providerName, string providerKey);

        Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input);
    }
}
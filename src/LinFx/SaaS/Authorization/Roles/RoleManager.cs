using System;
using System.Threading.Tasks;

namespace LinFx.SaaS.Authorization.Roles
{
    public class RoleManager
    {
        public async Task GrantPermissionAsync(Role role, Permission permission)
        {
            if (await IsGrantedAsync(role.Id, permission))
            {
                return;
            }
        }

        public virtual async Task<bool> IsGrantedAsync(string roleId, Permission permission)
        {
            ////Get cached role permissions
            //var cacheItem = await GetRolePermissionCacheItemAsync(roleId);

            ////Check the permission
            //return cacheItem.GrantedPermissions.Contains(permission.Name);

            throw new NotImplementedException();
        }
    }
}

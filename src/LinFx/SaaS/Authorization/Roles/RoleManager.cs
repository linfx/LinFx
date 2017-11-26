using LinFx.Authorization;
using LinFx.Caching;
using System;
using System.Threading.Tasks;

namespace LinFx.SaaS.Authorization.Roles
{
    public class RoleManager
    {
        private readonly ICacheManager _cacheManager;

        private IRolePermissionStore<Role> RolePermissionStore;

        public async Task GrantPermissionAsync(Role role, Permission permission)
        {
            if (await IsGrantedAsync(role.Id, permission))
            {
                return;
            }
        }

        public virtual async Task<bool> IsGrantedAsync(int roleId, Permission permission)
        {
            //Get cached role permissions
            var cacheItem = await GetRolePermissionCacheItemAsync(roleId);

            //Check the permission
            return cacheItem.GrantedPermissions.Contains(permission.Name);
        }

        private async Task<RolePermissionCacheItem> GetRolePermissionCacheItemAsync(int roleId)
        {
            //var cacheKey = roleId + "@" + (GetCurrentTenantId() ?? string.Empty);
            //return await _cacheManager.GetRolePermissionCache().GetAsync(cacheKey, async () =>
            //{
            //    var newCacheItem = new RolePermissionCacheItem(roleId);

            //    foreach (var permissionInfo in await RolePermissionStore.GetPermissionsAsync(roleId))
            //    {
            //        if (permissionInfo.IsGranted)
            //        {
            //            newCacheItem.GrantedPermissions.Add(permissionInfo.Name);
            //        }
            //    }

            //    return newCacheItem;
            //});

            throw new NotImplementedException();
        }

        private string GetCurrentTenantId()
        {
            //if (_unitOfWorkManager.Current != null)
            //{
            //    return _unitOfWorkManager.Current.GetTenantId();
            //}
            //return AbpSession.TenantId;

            return null;
        }
    }
}

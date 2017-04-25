using System.Collections.Generic;

namespace LinFx.SaaS.Authorization.Roles
{
    public class RolePermissionSetting : PermissionSetting
    {
        /// <summary>
        /// Role id.
        /// </summary>
        public string RoleId { get; set; }
    }

    /// <summary>
    /// Used to cache permissions of a role.
    /// </summary>
    public class RolePermissionCacheItem
    {
        public const string CacheStoreName = "RolePermissions";

        public long RoleId { get; set; }

        public HashSet<string> GrantedPermissions { get; set; } 

        public RolePermissionCacheItem()
        {
            GrantedPermissions = new HashSet<string>();
        }

        public RolePermissionCacheItem(int roleId)
            : this()
        {
            RoleId = roleId;
        }
    }
}
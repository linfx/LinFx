using LinFx.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.SaaS.Authorization.Roles
{
    public class RoleStore<TRole> : IRolePermissionStore<TRole>
    {
        private readonly IRepository<RolePermissionSetting> _rolePermissionSettingRepository;

        public ILogger Logger { get; set; }

        public RoleStore(IRepository<RolePermissionSetting> rolePermissionSettingRepository)
        {
            _rolePermissionSettingRepository = rolePermissionSettingRepository;
        }

        public Task AddPermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            throw new NotImplementedException();
        }

        public Task<IList<PermissionGrantInfo>> GetPermissionsAsync(TRole role)
        {
            throw new NotImplementedException();
        }

        public Task<IList<PermissionGrantInfo>> GetPermissionsAsync(int roleId)
        {

            throw new NotImplementedException();
        }

        public Task<bool> HasPermissionAsync(int roleId, PermissionGrantInfo permissionGrant)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllPermissionSettingsAsync(TRole role)
        {
            throw new NotImplementedException();
        }

        public Task RemovePermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            throw new NotImplementedException();
        }
    }
}

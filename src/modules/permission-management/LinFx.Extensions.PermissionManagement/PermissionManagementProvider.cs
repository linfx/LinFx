using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.MultiTenancy;
using LinFx.Utils;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement
{
    public abstract class PermissionManagementProvider : IPermissionManagementProvider
    {
        /// <summary>
        /// 名称
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 权限授权仓储
        /// </summary>
        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        /// <summary>
        /// 当前租户
        /// </summary>
        protected ICurrentTenant CurrentTenant { get; }

        protected PermissionManagementProvider(
            IPermissionGrantRepository permissionGrantRepository,
            ICurrentTenant currentTenant)
        {
            PermissionGrantRepository = permissionGrantRepository;
            CurrentTenant = currentTenant;
        }

        public virtual async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
        {
            if (providerName != Name)
                return PermissionValueProviderGrantInfo.NonGranted;

            return new PermissionValueProviderGrantInfo(
                await PermissionGrantRepository.FindAsync(name, providerName, providerKey) != null,
                providerKey
            );
        }

        public virtual Task SetAsync(string name, string providerKey, bool isGranted)
        {
            return isGranted
                ? GrantAsync(name, providerKey)
                : RevokeAsync(name, providerKey);
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="name"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        protected virtual async Task GrantAsync(string name, string providerKey)
        {
            var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, providerKey);
            if (permissionGrant != null)
                return;

            await PermissionGrantRepository.InsertAsync(new PermissionGrant(IDUtils.NewId(), name, Name, providerKey, CurrentTenant.Id));
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="name"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        protected virtual async Task RevokeAsync(string name, string providerKey)
        {
            var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, providerKey);
            if (permissionGrant == null)
            {
                return;
            }
            await PermissionGrantRepository.DeleteAsync(permissionGrant);
        }
    }
}

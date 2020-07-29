using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    /// <summary>
    /// 权限值提供者
    /// </summary>
    public abstract class PermissionValueProvider : IPermissionValueProvider
    {
        /// <summary>
        /// 提供者的名称。
        /// </summary>
        public abstract string Name { get; }

        protected IPermissionStore PermissionStore { get; }

        protected PermissionValueProvider(IPermissionStore permissionStore)
        {
            PermissionStore = permissionStore;
        }

        public abstract Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionValueCheckContext context);
    }
}

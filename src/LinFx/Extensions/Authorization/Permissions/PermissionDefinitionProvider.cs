using LinFx.Extensions.Authorization.Permissions;

namespace LinFx.Extensions.Authorization.Permissions
{
    /// <summary>
    /// 权限定义提供者
    /// </summary>
    public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        /// <summary>
        /// 定义
        /// </summary>
        /// <param name="context"></param>
        public abstract void Define(IPermissionDefinitionContext context);
    }
}
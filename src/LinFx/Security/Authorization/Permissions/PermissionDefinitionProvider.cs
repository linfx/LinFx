namespace LinFx.Security.Authorization.Permissions
{
    /// <summary>
    /// 权限定义提供者
    /// </summary>
    public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        public abstract void Define(IPermissionDefinitionContext context);
    }
}
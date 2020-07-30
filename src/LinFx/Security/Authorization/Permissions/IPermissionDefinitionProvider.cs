namespace LinFx.Security.Authorization.Permissions
{
    /// <summary>
    /// 权限定义提供者
    /// </summary>
    public interface IPermissionDefinitionProvider
    {
        void Define(IPermissionDefinitionContext context);
    }
}
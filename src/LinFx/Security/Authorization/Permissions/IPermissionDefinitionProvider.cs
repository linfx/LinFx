namespace LinFx.Security.Authorization.Permissions
{
    public interface IPermissionDefinitionProvider
    {
        void Define(IPermissionDefinitionContext context);
    }
}
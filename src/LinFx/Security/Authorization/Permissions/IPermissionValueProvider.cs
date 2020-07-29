using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    /// <summary>
    /// 权限值提供者
    /// </summary>
    public interface IPermissionValueProvider
    {
        string Name { get; }

        Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionValueCheckContext context);
    }
}
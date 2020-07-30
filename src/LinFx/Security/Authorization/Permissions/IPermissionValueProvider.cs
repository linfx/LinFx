using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    /// <summary>
    /// 权限值提供者
    /// </summary>
    public interface IPermissionValueProvider
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 权限校验
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionValueCheckContext context);
    }
}
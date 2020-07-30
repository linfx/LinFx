using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    public interface IPermissionStore
    {
        /// <summary>
        /// 是否授权
        /// </summary>
        /// <param name="name"></param>
        /// <param name="providerName"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        Task<bool> IsGrantedAsync(string name, string providerName, string providerKey);
    }
}
using LinFx.Extensions.PermissionManagement.Application;
using LinFx.Extensions.PermissionManagement.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement.HttpApi
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [ApiController]
    [Area("permissionManagement")]
    [Route("api/permission-management/permissions")]
    public class PermissionController : ControllerBase
    {
        protected IPermissionService PermissionAppService;

        public PermissionController(IPermissionService permissionAppService)
        {
            PermissionAppService = permissionAppService;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual Task<PermissionListResult> GetAsync(string providerName, string providerKey)
        {
            return PermissionAppService.GetAsync(providerName, providerKey);
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerKey"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual Task UpdateAsync(string providerName, string providerKey, UpdatePermissionDto input)
        {
            return PermissionAppService.UpdateAsync(providerName, providerKey, input);
        }
    }
}

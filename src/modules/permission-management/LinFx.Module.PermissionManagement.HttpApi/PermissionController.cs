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
    [Route("api/permission")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionAppService)
        {
            _permissionService = permissionAppService;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<PermissionListResult> GetAsync(string providerName, string providerKey)
        {
            return _permissionService.GetAsync(providerName, providerKey);
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerKey"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task UpdateAsync(string providerName, string providerKey, UpdatePermissionDto input)
        {
            return _permissionService.UpdateAsync(providerName, providerKey, input);
        }
    }
}

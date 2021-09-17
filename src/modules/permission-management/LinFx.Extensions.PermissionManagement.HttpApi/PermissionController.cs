﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement.HttpApi
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [ApiController]
    [Route("api/permission-management/permission")]
    public class PermissionController : ControllerBase
    {
        protected IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual Task<PermissionListResultDto> GetAsync(string providerName, string providerKey)
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
        public virtual Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            return _permissionService.UpdateAsync(providerName, providerKey, input);
        }
    }
}

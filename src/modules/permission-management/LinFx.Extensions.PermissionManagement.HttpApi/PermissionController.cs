using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.PermissionManagement.HttpApi;

/// <summary>
/// 权限管理
/// </summary>
//[Authorize]
[ApiController]
[Route("api/permission-management/permission")]
public class PermissionController : ControllerBase
{
    protected PermissionService _permissionService;

    public PermissionController(PermissionService permissionService)
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
    [Route("/api/permission-management/permissions")]
    public virtual Task<PermissionListResultDto> GetAsync([Required] string providerName, [Required] string providerKey) => _permissionService.GetAsync(providerName, providerKey);

    /// <summary>
    /// 更新权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public virtual Task UpdateAsync([Required] string providerName, [Required] string providerKey, UpdatePermissionsDto input) => _permissionService.UpdateAsync(providerName, providerKey, input);
}

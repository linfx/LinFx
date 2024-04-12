using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.PermissionManagement.HttpApi;

/// <summary>
/// 权限管理
/// </summary>
[ApiController]
[Route("api/permission-management/[controller]")]
public class PermissionController(PermissionService permissionService) : ControllerBase
{
    protected PermissionService PermissionService { get; } = permissionService;

    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/permission-management/permissions")]
    public virtual ValueTask<PermissionListResultDto> GetAsync([Required] string providerName, [Required] string providerKey) => PermissionService.GetAsync(providerName, providerKey);

    /// <summary>
    /// 更新权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public virtual Task UpdateAsync([Required] string providerName, [Required] string providerKey, UpdatePermissionsDto input) => PermissionService.UpdateAsync(providerName, providerKey, input);
}

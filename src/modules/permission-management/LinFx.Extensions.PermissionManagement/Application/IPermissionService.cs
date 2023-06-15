namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 权限服务
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task<PermissionListResultDto> GetAsync(string providerName, string providerKey);

    /// <summary>
    /// 更新权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input);
}

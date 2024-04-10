namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 权限信息
/// </summary>
public class PermissionGrantInfoDto
{
    /// <summary>
    /// 名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 父类名称
    /// </summary>
    public string? ParentName { get; set; }

    /// <summary>
    /// 是否授权
    /// </summary>
    public bool IsGranted { get; set; }

    public List<string>? AllowedProviders { get; set; }

    public List<ProviderInfoDto> GrantedProviders { get; set; } = [];
}

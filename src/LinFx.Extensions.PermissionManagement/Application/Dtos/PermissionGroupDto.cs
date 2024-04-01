namespace LinFx.Extensions.PermissionManagement;

public class PermissionGroupDto
{
    /// <summary>
    /// 权限组名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 权限组显示名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    public List<PermissionGrantInfoDto> Permissions { get; set; } = [];
}

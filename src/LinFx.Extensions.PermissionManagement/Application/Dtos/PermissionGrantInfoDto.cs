namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 权限信息
/// </summary>
public class PermissionGrantInfoDto
{
    public required string Name { get; set; }

    public string? DisplayName { get; set; }

    public string? ParentName { get; set; }

    public bool IsGranted { get; set; }

    public List<string>? AllowedProviders { get; set; }

    public List<ProviderInfoDto>? GrantedProviders { get; set; }
}

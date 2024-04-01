namespace LinFx.Extensions.PermissionManagement;

public class PermissionListResultDto
{
    public required string EntityDisplayName { get; set; }

    public List<PermissionGroupDto> Groups { get; set; } = [];
}

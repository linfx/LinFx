namespace LinFx.Extensions.PermissionManagement;

public class PermissionListResultDto
{
    public string EntityDisplayName { get; set; }

    public List<PermissionGroupDto> Groups { get; set; }
}

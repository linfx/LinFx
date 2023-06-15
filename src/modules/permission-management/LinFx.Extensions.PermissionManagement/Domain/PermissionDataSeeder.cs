using LinFx.Utils;

namespace LinFx.Extensions.PermissionManagement;

public class PermissionDataSeeder
{
    protected IPermissionGrantRepository PermissionGrantRepository { get; }

    public PermissionDataSeeder(IPermissionGrantRepository permissionGrantRepository)
    {
        PermissionGrantRepository = permissionGrantRepository;
    }

    public async Task SeedAsync(string providerName, string providerKey, IEnumerable<string> grantedPermissions, string tenantId = default)
    {
        foreach (var permissionName in grantedPermissions)
        {
            if (await PermissionGrantRepository.FindAsync(permissionName, providerName, providerKey) != null)
                continue;

            await PermissionGrantRepository.InsertAsync(new PermissionGrant(IDUtils.NewId(), permissionName, providerName, providerKey, tenantId));
        }
    }
}
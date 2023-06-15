using LinFx.Domain.Repositories;

namespace LinFx.Extensions.PermissionManagement;

public interface IPermissionGrantRepository : IBasicRepository<PermissionGrant, long>
{
    Task<PermissionGrant> FindAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<PermissionGrant>> GetListAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<PermissionGrant>> GetListAsync(
        string[] names,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );
}

using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.PermissionManagement.EntityFrameworkCore
{
    public interface IPermissionManagementDbContext : IEfDbContext
    {
        DbSet<PermissionGrant> PermissionGrants { get; }
    }
}
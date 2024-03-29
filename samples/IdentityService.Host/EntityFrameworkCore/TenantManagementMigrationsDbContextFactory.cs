using LinFx.Extensions.TenantManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityService.EntityFrameworkCore;

public class TenantManagementMigrationsDbContextFactory : IDesignTimeDbContextFactory<TenantManagementDbContext>
{
    public TenantManagementDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<TenantManagementDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"), b => b.MigrationsAssembly(GetType().Assembly.FullName));

        return new TenantManagementDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}

using System.IO;
using LinFx.Extensions.PermissionManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TenantManagementService.EntityFrameworkCore;

public class PermissionManagementDbContextMigrationsDbContextFactory : IDesignTimeDbContextFactory<PermissionManagementDbContext>
{
    public PermissionManagementDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<PermissionManagementDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("TenantManagementService.Host"));

        return new PermissionManagementDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}

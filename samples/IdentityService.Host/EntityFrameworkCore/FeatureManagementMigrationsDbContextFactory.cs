using LinFx.Extensions.FeatureManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityService.EntityFrameworkCore;

public class FeatureManagementMigrationsDbContextFactory : IDesignTimeDbContextFactory<FeatureManagementDbContext>
{
    public FeatureManagementDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<FeatureManagementDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"), b => b.MigrationsAssembly(GetType().Assembly.FullName));

        return new FeatureManagementDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}

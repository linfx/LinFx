﻿using LinFx.Extensions.PermissionManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityService.EntityFrameworkCore;

public class PermissionManagementDbContextMigrationsDbContextFactory : IDesignTimeDbContextFactory<PermissionManagementDbContext>
{
    public PermissionManagementDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<PermissionManagementDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"), b => b.MigrationsAssembly(GetType().Assembly.FullName));

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

using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

public interface IDbContextConfigurer
{
    void Configure(DbContextConfigurationContext context);
}

public interface IDbContextConfigurer<TDbContext>
    where TDbContext : DbContext
{
    void Configure(DbContextConfigurationContext<TDbContext> context);
}

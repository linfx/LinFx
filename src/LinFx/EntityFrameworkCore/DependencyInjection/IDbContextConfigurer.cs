namespace LinFx.EntityFrameworkCore.DependencyInjection
{
    public interface IDbContextConfigurer
    {
        void Configure(DbContextConfigurationContext context);
    }

    public interface IDbContextConfigurer<TDbContext>
        where TDbContext : EfCodeDbContext
    {
        void Configure(DbContextConfigurationContext<TDbContext> context);
    }
}
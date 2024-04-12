using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.FeatureManagement;

public class FeatureManagementDbContext(DbContextOptions<FeatureManagementDbContext> options) : EfDbContext(options)
{
    public DbSet<FeatureValue> FeatureValues { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureFeatureManagement();
    }
}

using Microsoft.EntityFrameworkCore;
using DbContext = LinFx.EntityFrameworkCore.DbContext;

namespace LinFx.Extensions.TenantManagement.EntityFrameworkCore
{
    public class TenantManagementDbContext : DbContext
    {
        public TenantManagementDbContext(DbContextOptions<TenantManagementDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Tenant>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).HasMaxLength(50);
            });
        }
    }
}

using LinFx.Module.TenantManagement.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = LinFx.Data.DbContext;

namespace LinFx.Module.TenantManagement.Data
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

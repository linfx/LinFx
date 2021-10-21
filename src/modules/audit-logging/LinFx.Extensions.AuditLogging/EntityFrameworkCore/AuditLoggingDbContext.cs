using LinFx.Data;
using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.AuditLogging.EntityFrameworkCore
{
    [ConnectionStringName(AuditLoggingDbProperties.ConnectionStringName)]
    public class AuditLoggingDbContext : EfCoreDbContext, IAuditLoggingDbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; }

        public AuditLoggingDbContext(DbContextOptions<AuditLoggingDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureAuditLogging();
        }
    }
}

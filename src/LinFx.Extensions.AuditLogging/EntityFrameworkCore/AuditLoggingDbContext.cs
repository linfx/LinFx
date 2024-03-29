using LinFx.Extensions.AuditLogging.Domain;
using LinFx.Extensions.Data;
using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.AuditLogging.EntityFrameworkCore;

[ConnectionStringName(AuditLoggingDbProperties.ConnectionStringName)]
public class AuditLoggingDbContext : EfDbContext<AuditLoggingDbContext>
{
    public AuditLoggingDbContext() { }

    public AuditLoggingDbContext(DbContextOptions<AuditLoggingDbContext> options) : base(options) { }

    /// <summary>
    /// 审计日志
    /// </summary>
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureAuditLogging();
    }
}

using LinFx.Extensions.AuditLogging.Domain;
using Microsoft.EntityFrameworkCore;
using DbContext = LinFx.EntityFrameworkCore.DbContext;

namespace LinFx.Extensions.AuditLogging.EntityFrameworkCore
{
    public class AuditLoggingDbContext : DbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}

using LinFx.Extensions.AuditLogging.Domain;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.AuditLogging.EntityFrameworkCore
{
    public class AuditLoggingDbContext : DbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}

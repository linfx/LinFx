﻿using LinFx.Data;
using LinFx.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.AuditLogging.EntityFrameworkCore
{
    [ConnectionStringName(AuditLoggingDbProperties.ConnectionStringName)]
    public interface IAuditLoggingDbContext : IEfCoreDbContext
    {
        DbSet<AuditLog> AuditLogs { get; }
    }
}
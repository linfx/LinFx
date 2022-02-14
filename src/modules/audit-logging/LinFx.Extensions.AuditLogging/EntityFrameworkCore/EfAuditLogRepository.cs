using LinFx.Domain.Entities;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.AuditLogging.EntityFrameworkCore;

[Service]
public class EfAuditLogRepository : EfRepository<IAuditLoggingDbContext, AuditLog, string>, IAuditLogRepository
{
    public EfAuditLogRepository(IServiceProvider serviceProvider, IDbContextProvider<IAuditLoggingDbContext> dbContextProvider)
        : base(serviceProvider, dbContextProvider)
    {
    }

    public virtual async Task<List<AuditLog>> GetListAsync(
        string sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string httpMethod = null,
        string url = null,
        string userId = null,
        string userName = null,
        string applicationName = null,
        string clientIpAddress = null,
        string correlationId = null,
        int? maxExecutionDuration = null,
        int? minExecutionDuration = null,
        bool? hasException = null,
        HttpStatusCode? httpStatusCode = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var query = await GetListQueryAsync(
            startTime,
            endTime,
            httpMethod,
            url,
            userId,
            userName,
            applicationName,
            clientIpAddress,
            correlationId,
            maxExecutionDuration,
            minExecutionDuration,
            hasException,
            httpStatusCode,
            includeDetails
        );

        var auditLogs = await query
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(AuditLog.ExecutionTime) + " DESC" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));

        return auditLogs;
    }

    public virtual async Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string httpMethod = null,
        string url = null,
        string userId = null,
        string userName = null,
        string applicationName = null,
        string clientIpAddress = null,
        string correlationId = null,
        int? maxExecutionDuration = null,
        int? minExecutionDuration = null,
        bool? hasException = null,
        HttpStatusCode? httpStatusCode = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetListQueryAsync(
            startTime,
            endTime,
            httpMethod,
            url,
            userId,
            userName,
            applicationName,
            clientIpAddress,
            correlationId,
            maxExecutionDuration,
            minExecutionDuration,
            hasException,
            httpStatusCode
        );

        var totalCount = await query.LongCountAsync(GetCancellationToken(cancellationToken));

        return totalCount;
    }

    protected virtual async Task<IQueryable<AuditLog>> GetListQueryAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string httpMethod = null,
        string url = null,
        string userId = null,
        string userName = null,
        string applicationName = null,
        string clientIpAddress = null,
        string correlationId = null,
        int? maxExecutionDuration = null,
        int? minExecutionDuration = null,
        bool? hasException = null,
        HttpStatusCode? httpStatusCode = null,
        bool includeDetails = false)
    {
        var nHttpStatusCode = (int?)httpStatusCode;
        return (await GetDbSetAsync()).AsNoTracking()
            //.IncludeDetails(includeDetails)
            .WhereIf(startTime.HasValue, auditLog => auditLog.ExecutionTime >= startTime)
            .WhereIf(endTime.HasValue, auditLog => auditLog.ExecutionTime <= endTime)
            .WhereIf(hasException.HasValue && hasException.Value, auditLog => auditLog.Exceptions != null && auditLog.Exceptions != "")
            .WhereIf(hasException.HasValue && !hasException.Value, auditLog => auditLog.Exceptions == null || auditLog.Exceptions == "")
            .WhereIf(httpMethod != null, auditLog => auditLog.HttpMethod == httpMethod)
            .WhereIf(url != null, auditLog => auditLog.Url != null && auditLog.Url.Contains(url))
            .WhereIf(userId != null, auditLog => auditLog.UserId == userId)
            .WhereIf(userName != null, auditLog => auditLog.UserName == userName)
            .WhereIf(applicationName != null, auditLog => auditLog.ApplicationName == applicationName)
            .WhereIf(clientIpAddress != null, auditLog => auditLog.ClientIpAddress != null && auditLog.ClientIpAddress == clientIpAddress)
            .WhereIf(correlationId != null, auditLog => auditLog.CorrelationId == correlationId)
            .WhereIf(httpStatusCode != null && httpStatusCode > 0, auditLog => auditLog.HttpStatusCode == nHttpStatusCode)
            .WhereIf(maxExecutionDuration != null && maxExecutionDuration.Value > 0, auditLog => auditLog.ExecutionDuration <= maxExecutionDuration)
            .WhereIf(minExecutionDuration != null && minExecutionDuration.Value > 0, auditLog => auditLog.ExecutionDuration >= minExecutionDuration);
    }

    public virtual async Task<Dictionary<DateTime, double>> GetAverageExecutionDurationPerDayAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var result = await (await GetDbSetAsync()).AsNoTracking()
            .Where(a => a.ExecutionTime < endDate.AddDays(1) && a.ExecutionTime > startDate)
            .OrderBy(t => t.ExecutionTime)
            .GroupBy(t => new { t.ExecutionTime.Date })
            .Select(g => new { Day = g.Min(t => t.ExecutionTime), avgExecutionTime = g.Average(t => t.ExecutionDuration) })
            .ToListAsync(GetCancellationToken(cancellationToken));

        return result.ToDictionary(element => element.Day, element => element.avgExecutionTime);
    }

    //public override async Task<IQueryable<AuditLog>> WithDetailsAsync()
    //{
    //    return (await GetQueryableAsync()).IncludeDetails();
    //}

    public virtual async Task<EntityChange> GetEntityChange(string entityChangeId, CancellationToken cancellationToken = default)
    {
        var entityChange = await (await GetDbContextAsync()).Set<EntityChange>()
                                .AsNoTracking()
                                //.IncludeDetails()
                                .Where(x => x.Id == entityChangeId)
                                .OrderBy(x => x.Id)
                                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

        if (entityChange == null)
        {
            throw new EntityNotFoundException(typeof(EntityChange));
        }

        return entityChange;
    }

    public virtual async Task<List<EntityChange>> GetEntityChangeListAsync(
        string sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        string auditLogId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        EntityChangeType? changeType = null,
        string entityId = null,
        string entityTypeFullName = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var query = await GetEntityChangeListQueryAsync(auditLogId, startTime, endTime, changeType, entityId, entityTypeFullName, includeDetails);

        return await query.OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(EntityChange.ChangeTime) + " DESC" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetEntityChangeCountAsync(
        string auditLogId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        EntityChangeType? changeType = null,
        string entityId = null,
        string entityTypeFullName = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetEntityChangeListQueryAsync(auditLogId, startTime, endTime, changeType, entityId, entityTypeFullName);

        var totalCount = await query.LongCountAsync(GetCancellationToken(cancellationToken));

        return totalCount;
    }

    public virtual async Task<EntityChangeWithUsername> GetEntityChangeWithUsernameAsync(
        string entityChangeId,
        CancellationToken cancellationToken = default)
    {
        var auditLog = await (await GetDbSetAsync())
            .AsNoTracking()
            //.IncludeDetails()
            .Where(x => x.EntityChanges.Any(y => y.Id == entityChangeId)).FirstAsync(GetCancellationToken(cancellationToken));

        return new EntityChangeWithUsername()
        {
            EntityChange = auditLog.EntityChanges.First(x => x.Id == entityChangeId),
            UserName = auditLog.UserName
        };
    }

    public virtual async Task<List<EntityChangeWithUsername>> GetEntityChangesWithUsernameAsync(
        string entityId,
        string entityTypeFullName,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = dbContext.Set<EntityChange>()
                            .AsNoTracking()
                            //.IncludeDetails()
                            .Where(x => x.EntityId == entityId && x.EntityTypeFullName == entityTypeFullName);

        return await (from e in query
                      join auditLog in dbContext.AuditLogs on e.AuditLogId equals auditLog.Id
                      select new EntityChangeWithUsername { EntityChange = e, UserName = auditLog.UserName })
                    .OrderByDescending(x => x.EntityChange.ChangeTime).ToListAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual async Task<IQueryable<EntityChange>> GetEntityChangeListQueryAsync(
        string auditLogId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        EntityChangeType? changeType = null,
        string entityId = null,
        string entityTypeFullName = null,
        bool includeDetails = false)
    {
        return (await GetDbContextAsync())
            .Set<EntityChange>()
            .AsNoTracking()
            //.IncludeDetails(includeDetails)
            .WhereIf(!string.IsNullOrWhiteSpace(auditLogId), e => e.AuditLogId == auditLogId)
            .WhereIf(startTime.HasValue, e => e.ChangeTime >= startTime)
            .WhereIf(endTime.HasValue, e => e.ChangeTime <= endTime)
            .WhereIf(changeType.HasValue, e => e.ChangeType == changeType)
            .WhereIf(!string.IsNullOrWhiteSpace(entityId), e => e.EntityId == entityId)
            .WhereIf(!string.IsNullOrWhiteSpace(entityTypeFullName), e => e.EntityTypeFullName.Contains(entityTypeFullName));
    }
}

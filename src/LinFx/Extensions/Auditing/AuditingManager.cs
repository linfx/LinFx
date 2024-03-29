﻿using LinFx.Domain.Entities.Auditing;
using LinFx.Extensions.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计日志管理器
/// </summary>
public class AuditingManager(
    IAmbientScopeProvider<IAuditLogScope> ambientScopeProvider,
    IAuditingFactory auditingFactory,
    IAuditingStore auditingStore,
    IServiceProvider serviceProvider,
    IOptions<AuditingOptions> options) : IAuditingManager
{
    private const string AmbientContextKey = "Auditing.IAuditLogScope";

    protected IServiceProvider ServiceProvider { get; } = serviceProvider;
    protected AuditingOptions Options { get; } = options.Value;
    protected ILogger<AuditingManager> Logger { get; set; } = NullLogger<AuditingManager>.Instance;
    private readonly IAmbientScopeProvider<IAuditLogScope> _ambientScopeProvider = ambientScopeProvider;
    private readonly IAuditingFactory _auditingFactory = auditingFactory;
    private readonly IAuditingStore _auditingStore = auditingStore;

    /// <summary>
    /// 当前审计日志范围
    /// </summary>
    public IAuditLogScope Current => _ambientScopeProvider.GetValue(AmbientContextKey);

    /// <summary>
    /// 开始审计
    /// </summary>
    /// <returns></returns>
    public IAuditLogSaveHandle BeginScope()
    {
        var ambientScope = _ambientScopeProvider.BeginScope(AmbientContextKey, new AuditLogScope(_auditingFactory.CreateAuditLogInfo()));

        Debug.Assert(Current != null, "Current != null");

        return new DisposableSaveHandle(this, ambientScope, Current.Log, Stopwatch.StartNew());
    }

    /// <summary>
    /// 执行审计贡献者
    /// </summary>
    /// <param name="auditLogInfo">审计信息</param>
    protected virtual void ExecutePostContributors(AuditLogInfo auditLogInfo)
    {
        using var scope = ServiceProvider.CreateScope();
        var context = new AuditLogContributionContext(scope.ServiceProvider, auditLogInfo);

        foreach (var contributor in Options.Contributors)
        {
            try
            {
                contributor.PostContribute(context);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, LogLevel.Warning);
            }
        }
    }

    protected virtual void BeforeSave(DisposableSaveHandle saveHandle)
    {
        saveHandle.StopWatch.Stop();
        saveHandle.AuditLog.ExecutionDuration = Convert.ToInt32(saveHandle.StopWatch.Elapsed.TotalMilliseconds);
        ExecutePostContributors(saveHandle.AuditLog);
        MergeEntityChanges(saveHandle.AuditLog);
    }

    /// <summary>
    /// 合并实体变化
    /// </summary>
    /// <param name="auditLog"></param>
    protected virtual void MergeEntityChanges(AuditLogInfo auditLog)
    {
        var changeGroups = auditLog.EntityChanges
            .Where(e => e.ChangeType == EntityChangeType.Updated)
            .GroupBy(e => new { e.EntityTypeFullName, e.EntityId })
            .ToList();

        foreach (var changeGroup in changeGroups)
        {
            if (changeGroup.Count() <= 1)
                continue;

            var firstEntityChange = changeGroup.First();

            foreach (var entityChangeInfo in changeGroup)
            {
                if (entityChangeInfo == firstEntityChange)
                    continue;

                firstEntityChange.Merge(entityChangeInfo);

                auditLog.EntityChanges.Remove(entityChangeInfo);
            }
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="saveHandle"></param>
    /// <returns></returns>
    protected virtual async Task SaveAsync(DisposableSaveHandle saveHandle)
    {
        BeforeSave(saveHandle);
        await _auditingStore.SaveAsync(saveHandle.AuditLog);
    }

    protected class DisposableSaveHandle : IAuditLogSaveHandle
    {
        public AuditLogInfo AuditLog { get; }
        public Stopwatch StopWatch { get; }

        private readonly AuditingManager _auditingManager;
        private readonly IDisposable _scope;

        public DisposableSaveHandle(
            AuditingManager auditingManager,
            IDisposable scope,
            AuditLogInfo auditLog,
            Stopwatch stopWatch)
        {
            _auditingManager = auditingManager;
            _scope = scope;
            AuditLog = auditLog;
            StopWatch = stopWatch;
        }

        public async Task SaveAsync()
        {
            await _auditingManager.SaveAsync(this);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}

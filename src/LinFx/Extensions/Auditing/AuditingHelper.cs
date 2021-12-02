using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Timing;
using LinFx.Security.Clients;
using LinFx.Security.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace LinFx.Extensions.Auditing;

[Service]
public class AuditingHelper : IAuditingHelper
{
    protected ILogger<AuditingHelper> Logger { get; }
    protected IAuditingStore AuditingStore { get; }

    /// <summary>
    /// 当前用户
    /// </summary>
    protected ICurrentUser CurrentUser { get; }

    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }

    /// <summary>
    /// 当前客户端
    /// </summary>
    protected ICurrentClient CurrentClient { get; }

    protected IClock Clock { get; }
    protected AuditingOptions Options;
    protected IServiceProvider ServiceProvider;
    //protected ICorrelationIdProvider CorrelationIdProvider { get; }

    public AuditingHelper(
        IOptions<AuditingOptions> options,
        ICurrentUser currentUser,
        ICurrentTenant currentTenant,
        //ICurrentClient currentClient,
        IClock clock,
        //IAuditingStore auditingStore,
        ILogger<AuditingHelper> logger,
        IServiceProvider serviceProvider
        //ICorrelationIdProvider correlationIdProvider
        )
    {
        Options = options.Value;
        CurrentUser = currentUser;
        CurrentTenant = currentTenant;
        //CurrentClient = currentClient;
        Clock = clock;
        //AuditingStore = auditingStore;

        Logger = logger;
        ServiceProvider = serviceProvider;
        //CorrelationIdProvider = correlationIdProvider;
    }

    /// <summary>
    /// 是否保存
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public virtual bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false)
    {
        if (methodInfo == null)
            return false;

        if (!methodInfo.IsPublic)
            return false;

        if (methodInfo.IsDefined(typeof(AuditedAttribute), true))
            return true;

        if (methodInfo.IsDefined(typeof(DisableAuditingAttribute), true))
            return false;

        var classType = methodInfo.DeclaringType;
        if (classType != null)
        {
            var shouldAudit = AuditingInterceptorRegistrar.ShouldAuditTypeByDefaultOrNull(classType);
            if (shouldAudit != null)
                return shouldAudit.Value;
        }

        return defaultValue;
    }

    public virtual bool IsEntityHistoryEnabled(Type entityType, bool defaultValue = false)
    {
        if (!entityType.IsPublic)
            return false;

        if (Options.IgnoredTypes.Any(t => t.IsAssignableFrom(entityType)))
            return false;

        if (entityType.IsDefined(typeof(AuditedAttribute), true))
            return true;

        foreach (var propertyInfo in entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (propertyInfo.IsDefined(typeof(AuditedAttribute)))
                return true;
        }

        if (entityType.IsDefined(typeof(DisableAuditingAttribute), true))
            return false;

        if (Options.EntityHistorySelectors.Any(selector => selector.Predicate(entityType)))
            return true;

        return defaultValue;
    }

    /// <summary>
    /// 创建审计信息
    /// </summary>
    /// <returns></returns>
    public virtual AuditLogInfo CreateAuditLogInfo()
    {
        var auditInfo = new AuditLogInfo
        {
            ApplicationName = Options.ApplicationName,
            TenantId = CurrentTenant.Id,
            TenantName = CurrentTenant.Name,
            UserId = CurrentUser.Id,
            UserName = CurrentUser.UserName,
            ClientId = CurrentClient.Id,
            //CorrelationId = CorrelationIdProvider.Get(),
            //ImpersonatorUserId = AbpSession.ImpersonatorUserId, //TODO: Impersonation system is not available yet!
            //ImpersonatorTenantId = AbpSession.ImpersonatorTenantId,
            ExecutionTime = Clock.Now
        };

        ExecutePreContributors(auditInfo);

        return auditInfo;
    }

    public virtual AuditLogActionInfo CreateAuditLogAction(
        AuditLogInfo auditLog,
        Type type,
        MethodInfo method,
        object[] arguments)
    {
        return CreateAuditLogAction(auditLog, type, method, CreateArgumentsDictionary(method, arguments));
    }

    public virtual AuditLogActionInfo CreateAuditLogAction(
        AuditLogInfo auditLog,
        Type type,
        MethodInfo method,
        IDictionary<string, object> arguments)
    {
        var actionInfo = new AuditLogActionInfo
        {
            ServiceName = type != null
                ? type.FullName
                : "",
            MethodName = method.Name,
            Parameters = SerializeConvertArguments(arguments),
            ExecutionTime = Clock.Now
        };

        //TODO Execute contributors

        return actionInfo;
    }

    protected virtual void ExecutePreContributors(AuditLogInfo auditLogInfo)
    {
        using var scope = ServiceProvider.CreateScope();
        var context = new AuditLogContributionContext(scope.ServiceProvider, auditLogInfo);

        foreach (var contributor in Options.Contributors)
        {
            try
            {
                contributor.PreContribute(context);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, LogLevel.Warning);
            }
        }
    }

    protected virtual string SerializeConvertArguments(IDictionary<string, object> arguments)
    {
        try
        {
            if (arguments.IsNullOrEmpty())
            {
                return "{}";
            }

            var dictionary = new Dictionary<string, object>();

            foreach (var argument in arguments)
            {
                if (argument.Value != null && Options.IgnoredTypes.Any(t => t.IsInstanceOfType(argument.Value)))
                {
                    dictionary[argument.Key] = null;
                }
                else
                {
                    dictionary[argument.Key] = argument.Value;
                }
            }

            return JsonSerializer.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            Logger.LogException(ex, LogLevel.Warning);
            return "{}";
        }
    }

    protected virtual Dictionary<string, object> CreateArgumentsDictionary(MethodInfo method, object[] arguments)
    {
        var parameters = method.GetParameters();
        var dictionary = new Dictionary<string, object>();

        for (var i = 0; i < parameters.Length; i++)
        {
            dictionary[parameters[i].Name] = arguments[i];
        }

        return dictionary;
    }
}

using LinFx.Extensions.Auditing;
using LinFx.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace LinFx.Extensions.AspNetCore.Mvc.Auditing;

/// <summary>
/// 审计日志过滤器
/// </summary>
public class AuditActionFilter : IAsyncActionFilter, ITransientDependency
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 判断是否写日志
        if (!ShouldSaveAudit(context, out var auditLog, out var auditLogAction))
        {
            await next();
            return;
        }

        //TODO: 为当前类型打上标识

        // 开始性能计数
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // 尝试调用接口方法
            var result = await next();

            // 产生异常之后，将其异常信息存放在审计信息之中
            if (result.Exception != null && !result.ExceptionHandled)
            {
                auditLog?.Exceptions.Add(result.Exception);
            }
        }
        catch (Exception ex)
        {
            // 产生异常之后，将其异常信息存放在审计信息之中
            auditLog?.Exceptions.Add(ex);
            throw;
        }
        finally
        {
            // 停止计数，并且存储审计信息
            stopwatch.Stop();
            auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
            auditLog?.Actions.Add(auditLogAction);
        }
    }

    /// <summary>
    /// 判断是否写日志
    /// </summary>
    /// <param name="context"></param>
    /// <param name="auditLog"></param>
    /// <param name="auditLogAction"></param>
    /// <returns></returns>
    private bool ShouldSaveAudit(ActionExecutingContext context, out AuditLogInfo? auditLog, out AuditLogActionInfo? auditLogAction)
    {
        auditLog = null;
        auditLogAction = null;

        var options = context.GetRequiredService<IOptions<AuditingOptions>>().Value;
        if (!options.IsEnabled)
            return false;

        if (!context.ActionDescriptor.IsControllerAction())
            return false;

        var auditLogScope = context.GetService<IAuditingManager>()?.Current;
        if (auditLogScope == null)
            return false;

        var auditingFactory = context.GetRequiredService<IAuditingFactory>();
        if (!auditingFactory.ShouldSaveAudit(context.ActionDescriptor.GetMethodInfo(), true))
            return false;

        auditLog = auditLogScope.Log;
        auditLogAction = auditingFactory.CreateAuditLogAction(
            auditLog,
            context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.AsType(),
            context.ActionDescriptor.AsControllerActionDescriptor().MethodInfo,
            context.ActionArguments
        );

        return true;
    }
}

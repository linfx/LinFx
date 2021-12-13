using LinFx.Extensions.Auditing;
using LinFx.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LinFx.Extensions.AspNetCore.Mvc.Auditing;

public class AuditActionFilter : IAsyncActionFilter, ITransientDependency
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!ShouldSaveAudit(context, out var auditLog, out var auditLogAction))
        {
            await next();
            return;
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await next();

            if (result.Exception != null && !result.ExceptionHandled)
            {
                auditLog.Exceptions.Add(result.Exception);
            }
        }
        catch (Exception ex)
        {
            auditLog.Exceptions.Add(ex);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
            auditLog.Actions.Add(auditLogAction);
        }
    }

    private bool ShouldSaveAudit(ActionExecutingContext context, out AuditLogInfo auditLog, out AuditLogActionInfo auditLogAction)
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

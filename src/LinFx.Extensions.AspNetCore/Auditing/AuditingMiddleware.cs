using LinFx.Extensions.Auditing;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Uow;
using LinFx.Security.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace LinFx.Extensions.AspNetCore.Auditing;

/// <summary>
/// 审计日志中间件
/// </summary>
public class AuditingMiddleware : IMiddleware, ITransientDependency
{
    private readonly IAuditingManager _auditingManager;

    protected AuditingOptions AuditingOptions { get; }

    protected AspNetCoreAuditingOptions AspNetCoreAuditingOptions { get; }

    protected ICurrentUser CurrentUser { get; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public AuditingMiddleware(
        IAuditingManager auditingManager,
        ICurrentUser currentUser,
        IOptions<AuditingOptions> auditingOptions,
        IOptions<AspNetCoreAuditingOptions> aspNetCoreAuditingOptions,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _auditingManager = auditingManager;
        CurrentUser = currentUser;
        UnitOfWorkManager = unitOfWorkManager;
        AuditingOptions = auditingOptions.Value;
        AspNetCoreAuditingOptions = aspNetCoreAuditingOptions.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!AuditingOptions.IsEnabled || IsIgnoredUrl(context))
        {
            await next(context);
            return;
        }

        var hasError = false;
        using var saveHandle = _auditingManager.BeginScope();
        Debug.Assert(_auditingManager.Current != null);

        try
        {
            await next(context);

            if (_auditingManager.Current.Log.Exceptions.Count != 0)
                hasError = true;
        }
        catch (Exception ex)
        {
            hasError = true;

            if (!_auditingManager.Current.Log.Exceptions.Contains(ex))
                _auditingManager.Current.Log.Exceptions.Add(ex);

            throw;
        }
        finally
        {
            if (ShouldWriteAuditLog(context, hasError))
            {
                if (UnitOfWorkManager.Current != null)
                    await UnitOfWorkManager.Current.SaveChangesAsync();

                await saveHandle.SaveAsync();
            }
        }
    }

    private bool IsIgnoredUrl(HttpContext context) => context.Request.Path.Value != null && AspNetCoreAuditingOptions.IgnoredUrls.Any(x => context.Request.Path.Value.StartsWith(x));

    private bool ShouldWriteAuditLog(HttpContext httpContext, bool hasError)
    {
        if (AuditingOptions.AlwaysLogOnException && hasError)
            return true;

        if (!AuditingOptions.IsEnabledForAnonymousUsers && !CurrentUser.IsAuthenticated)
            return false;

        if (!AuditingOptions.IsEnabledForGetRequests && string.Equals(httpContext.Request.Method, HttpMethods.Get, StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }
}

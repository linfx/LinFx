using LinFx.Extensions.AspNetCore.Http;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.ExceptionHandling;
using LinFx.Extensions.Http;
using LinFx.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace LinFx.Extensions.AspNetCore.ExceptionHandling;

/// <summary>
/// 异常中间件
/// </summary>
public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware, ITransientDependency
{
    private readonly ILogger _logger = logger;
    private readonly Func<object, Task> _clearCacheHeadersDelegate = ClearCacheHeaders;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // We can't do anything if the response has already started, just abort.
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("An exception occurred, but response has already started!");
                throw;
            }

            if (context.Items["_ActionInfo"] is ActionInfoInHttpContext actionInfo)
            {
                // 异常包装
                if (actionInfo.IsObjectResult) //TODO: Align with ExceptionFilter.ShouldHandleException!
                {
                    await HandleAndWrapException(context, ex);
                    return;
                }
            }

            throw;
        }
    }

    /// <summary>
    /// 异常包装
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private async Task HandleAndWrapException(HttpContext context, Exception exception)
    {
        _logger.LogException(exception);

        //await context
        //    .RequestServices
        //    .GetRequiredService<IExceptionNotifier>()
        //    .NotifyAsync(new ExceptionNotificationContext(exception));

        // 授权异常
        if (exception is AuthorizationException)
        {
            await context.RequestServices.GetRequiredService<IAuthorizationExceptionHandler>().HandleAsync(exception.As<AuthorizationException>(), context);
        }
        else
        {
            var errorInfoConverter = context.RequestServices.GetRequiredService<IExceptionToErrorInfoConverter>();
            var statusCodeFinder = context.RequestServices.GetRequiredService<IHttpExceptionStatusCodeFinder>();
            var exceptionHandlingOptions = context.RequestServices.GetRequiredService<IOptions<ExceptionHandlingOptions>>().Value;

            context.Response.Clear();
            context.Response.StatusCode = (int)statusCodeFinder.GetStatusCode(context, exception);
            context.Response.OnStarting(_clearCacheHeadersDelegate, context.Response);

            await context.Response.WriteAsJsonAsync(new RemoteServiceErrorResponse(errorInfoConverter.Convert(exception, options =>
            {
                options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
                options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
            })));
        }
    }

    static Task ClearCacheHeaders(object state)
    {
        var response = (HttpResponse)state;
        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);
        return Task.CompletedTask;
    }
}

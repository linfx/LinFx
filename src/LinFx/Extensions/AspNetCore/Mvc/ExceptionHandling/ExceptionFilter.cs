using LinFx.Extensions.AspNetCore.ExceptionHandling;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Http;
using LinFx.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace LinFx.Extensions.AspNetCore.Mvc.ExceptionHandling;

public class ExceptionFilter : IAsyncExceptionFilter, ITransientDependency
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (!ShouldHandleException(context))
            return;

        // 处理与包装异常
        await HandleAndWrapException(context);
    }

    protected virtual bool ShouldHandleException(ExceptionContext context)
    {
        //TODO: Create DontWrap attribute to control wrapping..?

        //if (context.ActionDescriptor.IsControllerAction() &&
        //    context.ActionDescriptor.HasObjectResult())
        //{
        //    return true;
        //}

        //if (context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
        //{
        //    return true;
        //}

        //if (context.HttpContext.Request.IsAjax())
        //{
        //    return true;
        //}

        return false;
    }

    /// <summary>
    /// 处理与包装异常
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual async Task HandleAndWrapException(ExceptionContext context)
    {
        //TODO: Trigger an AbpExceptionHandled event or something like that.

        var exceptionHandlingOptions = context.GetRequiredService<IOptions<ExceptionHandlingOptions>>().Value;
        var exceptionToErrorInfoConverter = context.GetRequiredService<IExceptionToErrorInfoConverter>();
       // var remoteServiceErrorInfo = exceptionToErrorInfoConverter.Convert(context.Exception, options =>
       //{
       //    options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
       //    options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
       //});

        var logLevel = context.Exception.GetLogLevel();

        var remoteServiceErrorInfoBuilder = new StringBuilder();
        remoteServiceErrorInfoBuilder.AppendLine($"---------- {nameof(RemoteServiceErrorInfo)} ----------");
        //remoteServiceErrorInfoBuilder.AppendLine(context.GetRequiredService<IJsonSerializer>().Serialize(remoteServiceErrorInfo, indented: true));

        var logger = context.GetService<ILogger<ExceptionFilter>>(NullLogger<ExceptionFilter>.Instance);

        //logger.LogWithLevel(logLevel, remoteServiceErrorInfoBuilder.ToString());

        logger.LogException(context.Exception, logLevel);

        //await context.GetRequiredService<IExceptionNotifier>().NotifyAsync(new ExceptionNotificationContext(context.Exception));

        if (context.Exception is AuthorizationException)
        {
            await context.HttpContext.RequestServices.GetRequiredService<IAuthorizationExceptionHandler>()
                .HandleAsync(context.Exception.As<AuthorizationException>(), context.HttpContext);
        }
        else
        {
            //context.HttpContext.Response.Headers.Add(HttpConsts.AbpErrorFormat, "true");
            context.HttpContext.Response.StatusCode = (int)context
                .GetRequiredService<IHttpExceptionStatusCodeFinder>()
                .GetStatusCode(context.HttpContext, context.Exception);

            //context.Result = new ObjectResult(new RemoteServiceErrorResponse(remoteServiceErrorInfo));
        }

        context.Exception = null; //Handled!
    }
}

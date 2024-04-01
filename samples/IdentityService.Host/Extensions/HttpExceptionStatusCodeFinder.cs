using LinFx;
using LinFx.Domain.Entities;
using LinFx.Extensions.AspNetCore.ExceptionHandling;
using LinFx.Extensions.Data;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.ExceptionHandling;
using LinFx.Security.Authorization;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DoorlockServerApplication.Extensions;

/// <summary>
/// Http status 代码转换
/// </summary>
[Service(ReplaceServices = true)]
public class HttpExceptionStatusCodeFinder(IOptions<ExceptionHttpStatusCodeOptions> options) : DefaultHttpExceptionStatusCodeFinder(options)
{
    public override HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
    {
        if (exception is IHasHttpStatusCode exceptionWithHttpStatusCode && exceptionWithHttpStatusCode.HttpStatusCode > 0)
            return (HttpStatusCode)exceptionWithHttpStatusCode.HttpStatusCode;

        if (exception is IHasErrorCode exceptionWithErrorCode && !exceptionWithErrorCode.Code.IsNullOrWhiteSpace())
            if (Options.ErrorCodeToHttpStatusCodeMappings.TryGetValue(exceptionWithErrorCode.Code, out var status))
                return status;

        if (exception is AuthorizationException)
            return httpContext.User.Identity.IsAuthenticated ? HttpStatusCode.Forbidden : HttpStatusCode.Unauthorized;

        return exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            EntityNotFoundException => HttpStatusCode.NotFound,
            DbConcurrencyException => HttpStatusCode.Conflict,
            NotImplementedException => HttpStatusCode.NotImplemented,
            //MqttCommunicationTimedOutException => HttpStatusCode.BadRequest,
            IBusinessException => HttpStatusCode.Forbidden,
            _ => HttpStatusCode.InternalServerError
        };
    }
}

using LinFx.Domain.Entities;
using LinFx.Extensions.Data;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.ExceptionHandling;
using LinFx.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace LinFx.Extensions.AspNetCore.ExceptionHandling;

/// <summary>
/// Http status 代码转换
/// </summary>
/// <param name="options"></param>
public class DefaultHttpExceptionStatusCodeFinder(IOptions<ExceptionHttpStatusCodeOptions> options) : IHttpExceptionStatusCodeFinder, ITransientDependency
{
    protected ExceptionHttpStatusCodeOptions Options { get; } = options.Value;

    public virtual HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
    {
        if (exception is IHasHttpStatusCode exceptionWithHttpStatusCode && exceptionWithHttpStatusCode.HttpStatusCode > 0)
            return (HttpStatusCode)exceptionWithHttpStatusCode.HttpStatusCode;

        if (exception is IHasErrorCode exceptionWithErrorCode && !exceptionWithErrorCode.Code.IsNullOrWhiteSpace())
            if (Options.ErrorCodeToHttpStatusCodeMappings.TryGetValue(exceptionWithErrorCode.Code, out var status))
                return status;

        if (exception is AuthorizationException)
            return httpContext.User.Identity.IsAuthenticated ? HttpStatusCode.Forbidden : HttpStatusCode.Unauthorized;

        //TODO: Handle SecurityException..?
        return exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            EntityNotFoundException => HttpStatusCode.NotFound,
            DbConcurrencyException => HttpStatusCode.Conflict,
            NotImplementedException => HttpStatusCode.NotImplemented,
            IBusinessException => HttpStatusCode.Forbidden,
            _ => HttpStatusCode.InternalServerError
        };
    }
}

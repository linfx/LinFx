using LinFx.Domain.Entities;
using LinFx.Extensions.Data;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.ExceptionHandling;
using LinFx.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace LinFx.Extensions.AspNetCore.ExceptionHandling;

public class DefaultHttpExceptionStatusCodeFinder : IHttpExceptionStatusCodeFinder, ITransientDependency
{
    protected ExceptionHttpStatusCodeOptions Options { get; }

    public DefaultHttpExceptionStatusCodeFinder(
        IOptions<ExceptionHttpStatusCodeOptions> options)
    {
        Options = options.Value;
    }

    public virtual HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
    {
        if (exception is IHasHttpStatusCode exceptionWithHttpStatusCode &&
            exceptionWithHttpStatusCode.HttpStatusCode > 0)
        {
            return (HttpStatusCode)exceptionWithHttpStatusCode.HttpStatusCode;
        }

        if (exception is IHasErrorCode exceptionWithErrorCode &&
            !exceptionWithErrorCode.Code.IsNullOrWhiteSpace())
        {
            if (Options.ErrorCodeToHttpStatusCodeMappings.TryGetValue(exceptionWithErrorCode.Code, out var status))
            {
                return status;
            }
        }

        if (exception is AuthorizationException)
        {
            return httpContext.User.Identity.IsAuthenticated
                ? HttpStatusCode.Forbidden
                : HttpStatusCode.Unauthorized;
        }

        //TODO: Handle SecurityException..?

        if (exception is ValidationException)
        {
            return HttpStatusCode.BadRequest;
        }

        if (exception is EntityNotFoundException)
        {
            return HttpStatusCode.NotFound;
        }

        if (exception is DbConcurrencyException)
        {
            return HttpStatusCode.Conflict;
        }

        if (exception is NotImplementedException)
        {
            return HttpStatusCode.NotImplemented;
        }

        //if (exception is IBusinessException)
        //{
        //    return HttpStatusCode.Forbidden;
        //}

        return HttpStatusCode.InternalServerError;
    }
}

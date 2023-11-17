using Microsoft.AspNetCore.Http;
using System.Net;

namespace LinFx.Extensions.AspNetCore.ExceptionHandling;

public interface IHttpExceptionStatusCodeFinder
{
    HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception);
}

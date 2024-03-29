using Microsoft.AspNetCore.Http;
using System.Net;

namespace LinFx.Extensions.AspNetCore.ExceptionHandling;

public interface IHttpExceptionStatusCodeFinder
{
    /// <summary>
    /// 获取状态码
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception);
}

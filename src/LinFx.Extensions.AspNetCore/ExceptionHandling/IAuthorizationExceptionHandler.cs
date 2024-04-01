using LinFx.Security.Authorization;
using Microsoft.AspNetCore.Http;

namespace LinFx.Extensions.AspNetCore.ExceptionHandling;

/// <summary>
/// 授权异常处理
/// </summary>
public interface IAuthorizationExceptionHandler
{
    /// <summary>
    /// 处理
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    Task HandleAsync(AuthorizationException exception, HttpContext httpContext);
}

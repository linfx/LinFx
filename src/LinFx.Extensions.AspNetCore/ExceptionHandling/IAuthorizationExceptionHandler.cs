using LinFx.Security.Authorization;
using Microsoft.AspNetCore.Http;

namespace LinFx.Extensions.AspNetCore.ExceptionHandling;

/// <summary>
/// 授权异常处理
/// </summary>
public interface IAuthorizationExceptionHandler
{
    Task HandleAsync(AuthorizationException exception, HttpContext httpContext);
}

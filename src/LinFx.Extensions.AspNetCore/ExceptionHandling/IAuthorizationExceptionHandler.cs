using LinFx.Security.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LinFx.Extensions.AspNetCore.ExceptionHandling;

public interface IAuthorizationExceptionHandler
{
    Task HandleAsync(AuthorizationException exception, HttpContext httpContext);
}

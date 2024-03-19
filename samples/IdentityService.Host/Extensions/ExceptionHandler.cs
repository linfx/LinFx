using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message;
        logger.LogError("Error Message: {exceptionMessage}, Time of occurrence {time}", exceptionMessage, DateTime.UtcNow);
        // Return false to continue with the default behavior
        // - or - return true to signal that this exception is handled

        var problemDetails = new ProblemDetails
        {
            //Status = customException.Code,
            //Title = customException.Message,
        };
        //if (environment.IsDevelopment())
        //{
        //    //problemDetails.Detail = $"Exception occurred: {customException.Message} {customException.StackTrace} {customException.Source}";
        //}
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
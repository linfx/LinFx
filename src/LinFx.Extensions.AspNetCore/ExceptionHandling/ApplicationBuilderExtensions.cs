using LinFx.Extensions.AspNetCore.ExceptionHandling;

namespace Microsoft.AspNetCore.Builder;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        //if (app.Properties.ContainsKey(ExceptionHandlingMiddlewareMarker))
        //{
        //    return app;
        //}

        //app.Properties[ExceptionHandlingMiddlewareMarker] = true;
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
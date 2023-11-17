using Microsoft.AspNetCore.Authentication;

namespace Microsoft.AspNetCore.Builder;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseJwtTokenMiddleware(this IApplicationBuilder app, string schema)
    {
        return app.Use(async (ctx, next) =>
        {
            if (ctx.User.Identity?.IsAuthenticated != true)
            {
                var result = await ctx.AuthenticateAsync(schema);
                if (result.Succeeded && result.Principal != null)
                {
                    ctx.User = result.Principal;
                }
            }
            await next();
        });
    }
}
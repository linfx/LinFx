using LinFx.Extensions.AspNetCore.Uow;

namespace Microsoft.AspNetCore.Builder;

public static partial class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds a <see cref="UnitOfWorkMiddleware"/> to the specified Microsoft.AspNetCore.Builder.IApplicationBuilder.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
    {
        return app
            .UseMiddleware<UnitOfWorkMiddleware>();
    }
}

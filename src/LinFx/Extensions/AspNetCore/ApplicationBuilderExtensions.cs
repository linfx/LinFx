using JetBrains.Annotations;
using LinFx;
using LinFx.Application;
using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder;

public static partial class ApplicationBuilderExtensions
{
    public static void InitializeApplication([NotNull] this IApplicationBuilder app)
    {
        Check.NotNull(app, nameof(app));

        app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
        var application = app.ApplicationServices.GetRequiredService<IApplicationWithExternalServiceProvider>();
        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        applicationLifetime.ApplicationStopping.Register(() =>
        {
            //application.Shutdown();
        });

        applicationLifetime.ApplicationStopped.Register(() =>
        {
            //application.Dispose();
        });

        application.Initialize(app.ApplicationServices);
    }
}

using JetBrains.Annotations;
using LinFx;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Threading;
using LinFx.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder;

public static partial class ApplicationBuilderExtensions
{
    public async static Task InitializeApplicationAsync([NotNull] this IApplicationBuilder app)
    {
        Check.NotNull(app, nameof(app));

        app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
        var application = app.ApplicationServices.GetRequiredService<IApplicationWithExternalServiceProvider>();
        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        applicationLifetime.ApplicationStopping.Register(() =>
        {
            AsyncHelper.RunSync(() => application.ShutdownAsync());
        });

        applicationLifetime.ApplicationStopped.Register(() =>
        {
            application.Dispose();
        });

        await application.InitializeAsync(app.ApplicationServices);
    }
}

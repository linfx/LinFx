using JetBrains.Annotations;
using LinFx.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LinFx.Extensions.AspNetCore;

public static class ApplicationInitializationContextExtensions
{
    public static IApplicationBuilder GetApplicationBuilder(this ApplicationInitializationContext context) => context.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;

    public static IHostEnvironment GetEnvironment(this ApplicationInitializationContext context) => context.ServiceProvider.GetRequiredService<IHostEnvironment>();

    [CanBeNull]
    public static IHostEnvironment GetEnvironmentOrNull(this ApplicationInitializationContext context) => context.ServiceProvider.GetService<IHostEnvironment>();

    public static IConfiguration GetConfiguration(this ApplicationInitializationContext context) => context.ServiceProvider.GetRequiredService<IConfiguration>();

    public static ILoggerFactory GetLoggerFactory(this ApplicationInitializationContext context) => context.ServiceProvider.GetRequiredService<ILoggerFactory>();
}

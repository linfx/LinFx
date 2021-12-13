using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper functions for configuring LinFx services.
/// </summary>
public class LinFxBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="LinFxBuilder"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to attach to.</param>
    public LinFxBuilder(IServiceCollection services) => Services = services;

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> services are attached to.
    /// </summary>
    /// <value>
    /// The <see cref="IServiceCollection"/> services are attached to.
    /// </value>
    public IServiceCollection Services { get; private set; }
}

public static class LinFxBuilderExtensions
{
    public static LinFxBuilder Configure<TOptions>(this LinFxBuilder builder, Action<TOptions> configureOptions) 
        where TOptions : class
    {
        builder.Services.Configure(configureOptions);
        return builder;
    }
}

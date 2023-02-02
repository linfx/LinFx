using Autofac;
using Autofac.Extensions.DependencyInjection;
using LinFx.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Autofac;

/// <summary>
/// A factory for creating a <see cref="T:Autofac.ContainerBuilder" /> and an <see cref="T:System.IServiceProvider" />.
/// </summary>
public class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    private readonly ContainerBuilder _builder;

    public AutofacServiceProviderFactory(ContainerBuilder builder) => _builder = builder;

    /// <summary>
    /// Creates a container builder from an <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
    /// </summary>
    /// <param name="services">The collection of services</param>
    /// <returns>A container builder that can be used to create an <see cref="T:System.IServiceProvider" />.</returns>
    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        _builder.Populate(services);
        return _builder;
    }

    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        Check.NotNull(containerBuilder, nameof(containerBuilder));

        return new AutofacServiceProvider(containerBuilder.Build());
    }
}

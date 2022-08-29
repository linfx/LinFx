using Autofac;
using LinFx.Application;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Autofac;

public static class AutofacApplicationCreationOptionsExtensions
{
    public static void UseAutofac(this ApplicationCreationOptions options) => options.Services.AddAutofacServiceProviderFactory();

    public static AutofacServiceProviderFactory AddAutofacServiceProviderFactory(this IServiceCollection services) => services.AddAutofacServiceProviderFactory(new ContainerBuilder());

    public static AutofacServiceProviderFactory AddAutofacServiceProviderFactory(this IServiceCollection services, ContainerBuilder containerBuilder)
    {
        var factory = new AutofacServiceProviderFactory(containerBuilder);

        services.AddObjectAccessor(containerBuilder);
        services.AddSingleton((IServiceProviderFactory<ContainerBuilder>)factory);

        return factory;
    }
}

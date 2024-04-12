using Autofac;
using LinFx;
using LinFx.Extensions.Autofac;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

public static class AutofacServiceCollectionExtensions
{
    public static ContainerBuilder GetContainerBuilder([NotNull] this IServiceCollection services)
    {
        Check.NotNull(services, nameof(services));

        var builder = services.GetObjectOrNull<ContainerBuilder>()
            ?? throw new Exception($"Could not find ContainerBuilder. Be sure that you have called {nameof(AutofacApplicationCreationOptionsExtensions.UseAutofac)} method before!");

        return builder;
    }

    public static IServiceProvider BuildAutofacServiceProvider([NotNull] this IServiceCollection services, Action<ContainerBuilder>? builderAction = default) => services.BuildServiceProviderFromFactory(builderAction);
}

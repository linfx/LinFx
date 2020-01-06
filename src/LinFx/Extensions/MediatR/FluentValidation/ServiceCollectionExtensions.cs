using FluentValidation;
using LinFx.Extensions.Mediator.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LinFx.Extensions.MediatR.FluentValidation
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFluentValidation(this IServiceCollection services, params Assembly[] assemblies)
        {
            AddFluentValidation(services, ServiceLifetime.Transient, assemblies);
        }

        public static void AddFluentValidation(this IServiceCollection services, ServiceLifetime lifetime, params Assembly[] assemblies)
        {
            services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>), lifetime));
            services.AddValidatorsFromAssemblies(assemblies, lifetime);
        }
    }
}

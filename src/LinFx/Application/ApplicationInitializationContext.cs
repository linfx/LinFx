using JetBrains.Annotations;
using LinFx.Extensions.DependencyInjection;
using LinFx.Utils;

namespace LinFx.Application;

/// <summary>
/// 应用初始化上下文
/// </summary>
public class ApplicationInitializationContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; set; }

    public ApplicationInitializationContext([NotNull] IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        ServiceProvider = serviceProvider;
    }
}

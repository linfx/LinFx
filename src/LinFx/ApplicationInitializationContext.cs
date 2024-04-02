using System.Diagnostics.CodeAnalysis;
using LinFx.Extensions.DependencyInjection;

namespace LinFx;

/// <summary>
/// 应用初始化上下文
/// </summary>
public class ApplicationInitializationContext([NotNull] IServiceProvider serviceProvider) : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; set; } = serviceProvider;
}

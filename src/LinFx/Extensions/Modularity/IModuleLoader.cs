using System.Diagnostics.CodeAnalysis;
using LinFx.Extensions.Modularity.PlugIns;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Modularity;

/// <summary>
/// 模块加载器
/// </summary>
public interface IModuleLoader
{
    /// <summary>
    /// 加载模块
    /// </summary>
    IModuleDescriptor[] LoadModules([NotNull] IServiceCollection services, [NotNull] Type startupModuleType, [NotNull] PlugInSourceList plugInSources);
}

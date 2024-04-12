using System.Diagnostics.CodeAnalysis;
using LinFx;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 模块管理器
/// </summary>
public interface IModuleManager
{
    /// <summary>
    /// 初始化模块
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task InitializeModulesAsync([NotNull] ApplicationInitializationContext context);

    /// <summary>
    /// 关闭模块
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task ShutdownModulesAsync([NotNull] ApplicationShutdownContext context);
}

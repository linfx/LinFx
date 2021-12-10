using JetBrains.Annotations;
using LinFx.Application;

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
    void InitializeModules([NotNull] ApplicationInitializationContext context);

    /// <summary>
    /// 关闭模块
    /// </summary>
    /// <param name="context"></param>
    void ShutdownModules([NotNull] ApplicationShutdownContext context);
}

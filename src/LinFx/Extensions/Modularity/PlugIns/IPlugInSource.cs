using JetBrains.Annotations;
using System;

namespace LinFx.Extensions.Modularity.PlugIns;

/// <summary>
/// 插件
/// </summary>
public interface IPlugInSource
{
    [NotNull]
    Type[] GetModules();
}
using LinFx.Collections;

namespace LinFx.Extensions.Modularity;

/// <summary>
/// 模块生命周期
/// </summary>
public class ModuleLifecycleOptions
{
    public ITypeList<IModuleLifecycleContributor> Contributors { get; } = new TypeList<IModuleLifecycleContributor>();
}

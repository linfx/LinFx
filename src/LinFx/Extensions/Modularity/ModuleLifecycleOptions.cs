using LinFx.Collections;

namespace LinFx.Extensions.Modularity;

public class ModuleLifecycleOptions
{
    public ITypeList<IModuleLifecycleContributor> Contributors { get; }

    public ModuleLifecycleOptions()
    {
        Contributors = new TypeList<IModuleLifecycleContributor>();
    }
}

using System.Collections.Generic;

namespace LinFx.Extensions.Modules
{
    public interface IModuleConfigurationManager
    {
        IEnumerable<ModuleInfo> GetModules();
    }
}
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.Modularity
{
    [Obsolete]
    public interface IModuleConfigurationManager
    {
        [Obsolete]
        IEnumerable<ModuleInfo> GetModules();
    }
}
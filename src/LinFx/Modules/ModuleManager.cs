using System;
using System.Collections.Generic;

namespace LinFx.Modules
{
    public interface IAbpModuleManager
    {
        ModuleInfo StartupModule { get; }

        IReadOnlyList<ModuleInfo> Modules { get; }

        void Initialize(Type startupModule);

        void StartModules();

        void ShutdownModules();
    }

    public class ModuleManager
    {
    }
}

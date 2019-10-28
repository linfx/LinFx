using System;
using System.Reflection;

namespace LinFx.Modules
{
    /// <summary>
    /// 模块
    /// </summary>
    public class ModuleInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsBundledWithHost { get; set; }

        public Version Version { get; set; }

        public Type Type { get; }

        public Assembly Assembly { get; }

        public IModule Instance { get; }

        public ModuleInfo(Type type, IModule instance)
        {
            Type = type;
            Instance = instance;
        }
    }
}

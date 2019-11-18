using System;
using System.Reflection;

namespace LinFx.Modules
{
    /// <summary>
    /// 模块
    /// </summary>
    public class ModuleInfo
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        public bool IsBundledWithHost { get; set; }

        public Version Version { get; set; }

        public Type Type { get; }

        public Assembly Assembly { get; set; }

        public IModuleInitializer Instance { get; }

        public ModuleInfo(Type type, IModuleInitializer instance)
        {
            Type = type;
            Instance = instance;
        }
    }
}

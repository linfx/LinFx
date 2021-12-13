using System;
using System.Reflection;

namespace LinFx.Extensions.Modularity
{
    /// <summary>
    /// 模块信息
    /// </summary>
    [Obsolete]
    public class ModuleInfo
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        public bool IsBundledWithHost { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 程序集
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// 实例
        /// </summary>
        public IModuleInitializer Instance { get; }

        public ModuleInfo(Type type, IModuleInitializer instance)
        {
            Type = type;
            Instance = instance;
        }
    }
}

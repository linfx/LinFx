using System;
using System.Reflection;

namespace LinFx.Modules
{
    /// <summary>
    /// 模块
    /// </summary>
    public class Module
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Version Version { get; set; }

        public Assembly Assembly { get; set; }
    }
}

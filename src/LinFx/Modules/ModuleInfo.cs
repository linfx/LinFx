using System;
using System.Reflection;

namespace LinFx.Modules
{
    public class ModuleInfo
    {
        /// <summary>
        /// The assembly which contains the module definition.
        /// </summary>
        public Assembly Assembly { get; }
        /// <summary>
        /// Type of the module.
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// Instance of the module.
        /// </summary>
        public Module Instance { get; }



        public override string ToString()
        {
            return Type.AssemblyQualifiedName ?? Type.FullName;
        }
    }
}

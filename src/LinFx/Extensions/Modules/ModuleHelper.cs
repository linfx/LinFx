using System;
using System.Collections.Generic;

namespace LinFx.Modules
{
    internal class ModuleHelper
    {
        public static List<Type> FindAllModuleTypes(Type startupModuleType)
        {
            var moduleTypes = new List<Type>();
            //AddModuleAndDependenciesResursively(moduleTypes, startupModuleType);
            return moduleTypes;
        }
    }
}

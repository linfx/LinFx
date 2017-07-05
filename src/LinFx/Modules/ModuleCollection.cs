using System.Collections.Generic;
using System.Linq;

namespace LinFx.Modules
{
    public class ModuleCollection : List<ModuleInfo>
    {
        /// <summary>
        /// Gets a reference to a module instance.
        /// </summary>
        /// <typeparam name="TModule">Module type</typeparam>
        /// <returns>Reference to the module instance</returns>
        public TModule GetModule<TModule>() where TModule : Module
        {
            var module = this.FirstOrDefault(m => m.Type == typeof(TModule));
            if (module == null)
                throw new LinFxException("Can not find module for " + typeof(TModule).FullName);

            return (TModule)module.Instance;
        }
    }
}
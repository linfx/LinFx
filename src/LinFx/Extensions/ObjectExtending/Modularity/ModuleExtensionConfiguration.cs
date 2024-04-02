using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.ObjectExtending.Modularity
{
    public class ModuleExtensionConfiguration
    {
        [NotNull]
        public EntityExtensionConfigurationDictionary Entities { get; }

        [NotNull]
        public Dictionary<string, object> Configuration { get; }

        public ModuleExtensionConfiguration()
        {
            Entities = new EntityExtensionConfigurationDictionary();
            Configuration = new Dictionary<string, object>();
        }
    }
}
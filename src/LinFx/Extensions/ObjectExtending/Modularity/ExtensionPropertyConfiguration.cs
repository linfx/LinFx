using System.Diagnostics.CodeAnalysis;
using LinFx.Reflection;
using LinFx.Utils;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.ObjectExtending.Modularity
{
    public class ExtensionPropertyConfiguration : IBasicObjectExtensionPropertyInfo
    {
        [NotNull]
        public EntityExtensionConfiguration EntityExtensionConfiguration { get; }

        [NotNull]
        public string Name { get; }

        [NotNull]
        public Type Type { get; }

        [NotNull]
        public List<Attribute> Attributes { get; }

        [NotNull]
        public List<Action<ObjectExtensionPropertyValidationContext>> Validators { get; }

        [AllowNull]
        public LocalizedString DisplayName { get; set; }

        [NotNull]
        public Dictionary<string, object> Configuration { get; }

        /// <summary>
        /// Single point to enable/disable this property for the clients (UI and API).
        /// If this is false, the configuration made in the <see cref="UI"/> and the <see cref="Api"/>
        /// properties are not used.
        /// Default: true.
        /// </summary>
        public bool IsAvailableToClients { get; set; } = true;

        [NotNull]
        public ExtensionPropertyEntityConfiguration Entity { get; }

        [NotNull]
        public ExtensionPropertyUiConfiguration UI { get; }

        [NotNull]
        public ExtensionPropertyApiConfiguration Api { get; }

        /// <summary>
        /// Uses as the default value if <see cref="DefaultValueFactory"/> was not set.
        /// </summary>
        [AllowNull]
        public object DefaultValue { get; set; }

        /// <summary>
        /// Used with the first priority to create the default value for the property.
        /// Uses to the <see cref="DefaultValue"/> if this was not set.
        /// </summary>
        [AllowNull]
        public Func<object> DefaultValueFactory { get; set; }

        public ExtensionPropertyConfiguration(
            [NotNull] EntityExtensionConfiguration entityExtensionConfiguration,
            [NotNull] Type type,
            [NotNull] string name)
        {
            EntityExtensionConfiguration = Check.NotNull(entityExtensionConfiguration, nameof(entityExtensionConfiguration));
            Type = Check.NotNull(type, nameof(type));
            Name = Check.NotNull(name, nameof(name));

            Configuration = new Dictionary<string, object>();
            Attributes = new List<Attribute>();
            Validators = new List<Action<ObjectExtensionPropertyValidationContext>>();

            Entity = new ExtensionPropertyEntityConfiguration();
            UI = new ExtensionPropertyUiConfiguration();
            Api = new ExtensionPropertyApiConfiguration();

            Attributes.AddRange(ExtensionPropertyHelper.GetDefaultAttributes(Type));
            DefaultValue = TypeHelper.GetDefaultValue(Type);
        }

        public object GetDefaultValue()
        {
            return ExtensionPropertyHelper.GetDefaultValue(Type, DefaultValueFactory, DefaultValue);
        }
    }
}
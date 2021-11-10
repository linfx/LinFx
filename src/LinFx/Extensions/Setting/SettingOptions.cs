using LinFx.Collections;

namespace LinFx.Extensions.Setting
{
    public class SettingOptions
    {
        public ITypeList<ISettingDefinitionProvider> DefinitionProviders { get; }

        public ITypeList<ISettingValueProvider> ValueProviders { get; }

        public SettingOptions()
        {
            DefinitionProviders = new TypeList<ISettingDefinitionProvider>();
            ValueProviders = new TypeList<ISettingValueProvider>();
        }
    }
}

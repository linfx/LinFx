using System.Collections.Generic;

namespace LinFx.Extensions.Setting
{
    public interface ISettingDefinitionContext
    {
        SettingDefinition GetOrNull(string name);

        IReadOnlyList<SettingDefinition> GetAll();

        void Add(params SettingDefinition[] definitions);
    }
}

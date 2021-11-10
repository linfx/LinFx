using System.Collections.Generic;
using JetBrains.Annotations;

namespace LinFx.Extensions.Setting
{
    public interface ISettingDefinitionManager
    {
        [NotNull]
        SettingDefinition Get([NotNull] string name);

        IReadOnlyList<SettingDefinition> GetAll();

        SettingDefinition GetOrNull(string name);
    }
}
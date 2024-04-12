using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Setting
{
    public interface ISettingDefinitionManager
    {
        SettingDefinition Get([NotNull] string name);

        IReadOnlyList<SettingDefinition> GetAll();

        SettingDefinition GetOrNull(string name);
    }
}
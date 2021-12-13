using LinFx.Extensions.DependencyInjection;

namespace LinFx.Extensions.Setting
{
    [Service]
    public abstract class SettingDefinitionProvider : ISettingDefinitionProvider
    {
        public abstract void Define(ISettingDefinitionContext context);
    }
}
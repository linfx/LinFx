using LinFx.Extensions.Setting;
using Microsoft.Extensions.Localization;

namespace LinFx.Extensions.Timing
{
    public class TimingSettingProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(new SettingDefinition(TimingSettingNames.TimeZone, "UTC", L("DisplayName:Timing.Timezone"), L("Description:Timing.Timezone"), isVisibleToClients: true));
        }

        private static LocalizedString L(string name)
        {
            //return LocalizableString.Create<TimingResource>(name);
            throw new System.NotImplementedException();
        }
    }
}

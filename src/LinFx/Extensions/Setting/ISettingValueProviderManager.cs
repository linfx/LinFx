using System.Collections.Generic;

namespace LinFx.Extensions.Setting
{
    public interface ISettingValueProviderManager
    {
        List<ISettingValueProvider> Providers { get; }
    }
}
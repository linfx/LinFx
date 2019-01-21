using System.Collections.Generic;

namespace LinFx.Domain.Abstractions
{
    public interface IHasExtraProperties
    {
        Dictionary<string, object> ExtraProperties { get; }
    }
}

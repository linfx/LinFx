using LinFx;
using System.Collections.Generic;

namespace LinFx.Domain.Models
{
    public interface IHasExtraProperties
    {
        Dictionary<string, object> ExtraProperties { get; }
    }
}

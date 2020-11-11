using LinFx;
using LinFx.Domain;
using LinFx.Domain.Entities;
using System.Collections.Generic;

namespace LinFx.Domain.Entities
{
    public interface IHasExtraProperties
    {
        Dictionary<string, object> ExtraProperties { get; }
    }
}

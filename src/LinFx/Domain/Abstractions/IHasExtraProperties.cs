namespace LinFx.Extensions.Data

namespace LinFx.Domain.Abstractions
{
    public interface IHasExtraProperties
    {
        Dictionary<string, object> ExtraProperties { get; }
    }
}

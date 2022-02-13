using JetBrains.Annotations;

namespace LinFx.Domain.Entities.Events.Distributed;

public interface IEntityToEtoMapper
{
    [CanBeNull]
    object Map(object entityObj);
}

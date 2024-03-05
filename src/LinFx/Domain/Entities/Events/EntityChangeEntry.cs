using LinFx.Extensions.Auditing;

namespace LinFx.Domain.Entities.Events;

[Serializable]
public class EntityChangeEntry(object entity, EntityChangeType changeType)
{
    public object Entity { get; set; } = entity;

    public EntityChangeType ChangeType { get; set; } = changeType;
}

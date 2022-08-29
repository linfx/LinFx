using LinFx.Extensions.Auditing;

namespace LinFx.Domain.Entities.Events;

[Serializable]
public class EntityChangeEntry
{
    public object Entity { get; set; }

    public EntityChangeType ChangeType { get; set; }

    public EntityChangeEntry(object entity, EntityChangeType changeType)
    {
        Entity = entity;
        ChangeType = changeType;
    }
}

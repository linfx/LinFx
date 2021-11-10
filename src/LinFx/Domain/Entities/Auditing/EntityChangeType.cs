namespace LinFx.Domain.Entities.Auditing
{
    public enum EntityChangeType : byte
    {
        Created = 0,
        Updated = 1,
        Deleted = 2
    }
}

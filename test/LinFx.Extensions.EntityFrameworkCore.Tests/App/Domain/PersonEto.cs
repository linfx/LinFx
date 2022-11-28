using LinFx.Domain.Entities.Events.Distributed;

public class PersonEto : EntityEto
{
    public virtual Guid? TenantId { get; set; }

    public virtual Guid? CityId { get; set; }

    public virtual string Name { get; set; }

    public virtual int Age { get; set; }
}

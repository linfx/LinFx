using LinFx.Domain.Entities.Auditing;
using LinFx.Extensions.Timing;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

public class Person : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual string TenantId { get; set; }

    public virtual Guid? CityId { get; set; }

    public virtual string Name { get; private set; }

    public virtual int Age { get; set; }

    public virtual DateTime? Birthday { get; set; }

    [DisableDateTimeNormalization]
    public virtual DateTime? LastActive { get; set; }

    [NotMapped]
    public virtual DateTime? NotMappedDateTime { get; set; }

    public virtual Collection<Phone> Phones { get; set; }

    public virtual DateTime LastActiveTime { get; set; }

    public virtual void ChangeName(string name)
    {
        var oldName = Name;
        Name = name;

        AddLocalEvent(
            new PersonNameChangedEvent
            {
                Person = this,
                OldName = oldName
            }
        );

        AddDistributedEvent(
            new PersonNameChangedEto
            {
                Id = Id,
                OldName = oldName,
                NewName = Name,
                TenantId = TenantId
            }
        );
    }
}

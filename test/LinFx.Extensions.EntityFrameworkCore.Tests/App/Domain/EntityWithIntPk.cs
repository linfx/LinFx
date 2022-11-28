using LinFx.Domain.Entities;

public class EntityWithIntPk : AggregateRoot<int>
{
    public string Name { get; set; }

    public EntityWithIntPk()
    {

    }

    public EntityWithIntPk(string name)
    {
        Name = name;
    }
}

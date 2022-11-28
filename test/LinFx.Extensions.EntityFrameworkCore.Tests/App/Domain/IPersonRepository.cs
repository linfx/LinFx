using LinFx.Domain.Repositories;

public interface IPersonRepository : IBasicRepository<Person, Guid>
{
    Task<PersonView> GetViewAsync(string name);
}

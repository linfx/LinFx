using LinFx.Domain.Repositories;

public interface ICityRepository : IBasicRepository<City, Guid>
{
    Task<City> FindByNameAsync(string name);

    Task<List<Person>> GetPeopleInTheCityAsync(string cityName);
}

using LinFx.Domain.Repositories;
using LinFx.Extensions.EntityFrameworkCore.Repositories;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

/// <summary>
/// ²Ö´¢×¢²áÆ÷
/// </summary>
public class EfRepositoryRegistrar : RepositoryRegistrarBase<DbContextRegistrationOptions>
{
    public EfRepositoryRegistrar(DbContextRegistrationOptions options)
        : base(options)
    { }

    protected override IEnumerable<Type> GetEntityTypes(Type dbContextType) => DbContextHelper.GetEntityTypes(dbContextType);

    protected override Type GetRepositoryType(Type dbContextType, Type entityType) => typeof(EfRepository<,>).MakeGenericType(dbContextType, entityType);

    protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType) => typeof(EfRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
}

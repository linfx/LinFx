using LinFx.Domain.Repositories;
using LinFx.Extensions.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

/// <summary>
/// ²Ö´¢×¢²áÆ÷
/// </summary>
public class EfRepositoryRegistrar : RepositoryRegistrarBase<DbContextRegistrationOptions>
{
    public EfRepositoryRegistrar(DbContextRegistrationOptions options)
        : base(options)
    {
    }

    protected override IEnumerable<Type> GetEntityTypes(Type dbContextType)
    {
        return DbContextHelper.GetEntityTypes(dbContextType);
    }

    protected override Type GetRepositoryType(Type dbContextType, Type entityType)
    {
        return typeof(EfRepository<,>).MakeGenericType(dbContextType, entityType);
    }

    protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
    {
        return typeof(EfRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
    }
}

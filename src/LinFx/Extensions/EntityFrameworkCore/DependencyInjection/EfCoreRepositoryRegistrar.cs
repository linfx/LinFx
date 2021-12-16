using LinFx.Domain.Repositories;
using LinFx.Extensions.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

/// <summary>
/// �ִ�ע����
/// </summary>
public class EfCoreRepositoryRegistrar : RepositoryRegistrarBase<DbContextRegistrationOptions>
{
    public EfCoreRepositoryRegistrar(DbContextRegistrationOptions options)
        : base(options)
    {
    }

    protected override IEnumerable<Type> GetEntityTypes(Type dbContextType)
    {
        return DbContextHelper.GetEntityTypes(dbContextType);
    }

    protected override Type GetRepositoryType(Type dbContextType, Type entityType)
    {
        return typeof(EfCoreRepository<,>).MakeGenericType(dbContextType, entityType);
    }

    protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
    {
        return typeof(EfCoreRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
    }
}

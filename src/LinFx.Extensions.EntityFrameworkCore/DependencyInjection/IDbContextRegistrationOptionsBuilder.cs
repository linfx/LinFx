using System.Diagnostics.CodeAnalysis;
using LinFx.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

public interface IDbContextRegistrationOptionsBuilder : ICommonDbContextRegistrationOptionsBuilder
{
    void Entity<TEntity>([NotNull] Action<EntityOptions<TEntity>> optionsAction) where TEntity : IEntity;
}

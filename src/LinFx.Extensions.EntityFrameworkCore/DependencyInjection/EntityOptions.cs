﻿using JetBrains.Annotations;
using LinFx.Domain.Entities;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

public class EntityOptions<TEntity>
    where TEntity : IEntity
{
    public static EntityOptions<TEntity> Empty { get; } = new EntityOptions<TEntity>();

    public Func<IQueryable<TEntity>, IQueryable<TEntity>> DefaultWithDetailsFunc { get; set; }
}

public class EntityOptions
{
    private readonly IDictionary<Type, object> _options;

    public EntityOptions()
    {
        _options = new Dictionary<Type, object>();
    }

    public EntityOptions<TEntity> GetOrNull<TEntity>()
        where TEntity : IEntity
    {
        return _options.GetOrDefault(typeof(TEntity)) as EntityOptions<TEntity>;
    }

    public void Entity<TEntity>([NotNull] Action<EntityOptions<TEntity>> optionsAction)
        where TEntity : IEntity
    {
        Check.NotNull(optionsAction, nameof(optionsAction));

        optionsAction(
            _options.GetOrAdd(
                typeof(TEntity),
                () => new EntityOptions<TEntity>()
            ) as EntityOptions<TEntity>
        );
    }
}

﻿using LinFx.Domain.Entities;
using LinFx.Domain.Repositories;
using LinFx.Extensions.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace LinFx.Extensions.EntityFrameworkCore.Repositories;

public static class RepositoryExtensions
{
    public static IQueryable<TEntity> Where<TEntity>(this IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity
    {
        return source.Queryable.Where(predicate);
    }

    public static IQueryable<TResult> Select<TEntity, TResult>(this IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TResult>> selector) where TEntity : class, IEntity
    {
        return source.Queryable.Select(selector);
    }

    public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(this IReadOnlyRepository<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        where TEntity : class, IEntity
    {
        return source.Queryable.Include(navigationPropertyPath);
    }
}
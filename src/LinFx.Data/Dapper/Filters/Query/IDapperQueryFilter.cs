using LinFx.Data.Dapper.Extensions;
using System;
using System.Linq.Expressions;

namespace LinFx.Data.Dapper.Filters.Query
{
    public interface IDapperQueryFilter
    {
        string FilterName { get; }

        bool IsEnabled { get; }

        IFieldPredicate ExecuteFilter<TEntity>() where TEntity : class;

        Expression<Func<TEntity, bool>> ExecuteFilter<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    }
}

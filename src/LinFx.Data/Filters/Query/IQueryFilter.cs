using LinFx.Data.Extensions;
using System;
using System.Linq.Expressions;

namespace LinFx.Data.Filters.Query
{
	public interface IQueryFilter
    {
        string FilterName { get; }
        bool IsEnabled { get; }
        IFieldPredicate ExecuteFilter<TEntity>() where TEntity : class;
        Expression<Func<TEntity, bool>> ExecuteFilter<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    }
}

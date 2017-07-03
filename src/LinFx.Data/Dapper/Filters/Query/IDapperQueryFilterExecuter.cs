using LinFx.Data.Dapper.Extensions;
using System;
using System.Linq.Expressions;

namespace LinFx.Data.Dapper.Filters.Query
{
    public interface IDapperQueryFilterExecuter
    {
        IPredicate ExecuteFilter<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        PredicateGroup ExecuteFilter<TEntity>() where TEntity : class;
    }
}

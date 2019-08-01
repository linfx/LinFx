using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinFx.Extensions.Dapper;
using LinFx.Extensions.Data.Expressions;

namespace LinFx.Extensions.Data.Filters.Query
{
    public interface IQueryFilterExecuter
	{
		IPredicate ExecuteFilter<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
		PredicateGroup ExecuteFilter<TEntity>() where TEntity : class;
	}

	public class QueryFilterExecuter : IQueryFilterExecuter
    {
        private readonly IEnumerable<IQueryFilter> _queryFilters = new List<IQueryFilter>();

        public IPredicate ExecuteFilter<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            ICollection<IQueryFilter> filters = _queryFilters.ToList();

            foreach (IQueryFilter filter in filters)
            {
                predicate = filter.ExecuteFilter(predicate);
            }

			//if(predicate == null)
			//	return null;

            IPredicate pg = predicate?.ToPredicateGroup();
            return pg;
        }

        public PredicateGroup ExecuteFilter<TEntity>() where TEntity : class
        {
            ICollection<IQueryFilter> filters = _queryFilters.ToList();
            var groups = new PredicateGroup
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };

            foreach (IQueryFilter filter in filters)
            {
                IFieldPredicate predicate = filter.ExecuteFilter<TEntity>();
                if (predicate != null)
                {
                    groups.Predicates.Add(predicate);
                }
            }
            return groups;
        }
    }
}

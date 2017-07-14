using LinFx.Data.Extensions;
using System;
using System.Linq.Expressions;

namespace LinFx.Data.Expressions
{
	internal static class DataAccessExpressionExtensions
    {
        public static IPredicate ToPredicateGroup<TEntity>(this Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            Check.NotNull(expression, nameof(expression));

            var dev = new DataAccessExpressionVisitor<TEntity>();
            IPredicate pg = dev.Process(expression);

            return pg;
        }
    }
}

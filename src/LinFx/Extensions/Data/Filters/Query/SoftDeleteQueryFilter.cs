using LinFx.Extensions.Auditing;
using LinFx.Extensions.Dapper;
using LinFx.Extensions.Data.Utils;
using System;
using System.Linq.Expressions;

namespace LinFx.Extensions.Data.Filters.Query
{
    public class SoftDeleteQueryFilter : IQueryFilter
    {
        //public string FilterName => DataFilters.SoftDelete;
        public string FilterName => throw new NotImplementedException();

        public bool IsEnabled => false;

        public bool IsDeleted => false;

        private bool IsFilterable<TEntity>() where TEntity : class
        {
            return typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)) && IsEnabled;
        }

        public IFieldPredicate ExecuteFilter<TEntity>() where TEntity : class
        {
            IFieldPredicate predicate = null;
            if (IsFilterable<TEntity>())
                predicate = Predicates.Field<TEntity>(f => (f as ISoftDelete).IsDeleted, Operator.Eq, IsDeleted);

            return predicate;
        }

        public Expression<Func<TEntity, bool>> ExecuteFilter<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (IsFilterable<TEntity>())
            {
                var propType = typeof(TEntity).GetProperty(nameof(ISoftDelete.IsDeleted));
                if (predicate == null)
                    predicate = ExpressionUtils.MakePredicate<TEntity>(nameof(ISoftDelete.IsDeleted), IsDeleted, propType.PropertyType);
                else
                {
                    ParameterExpression paramExpr = predicate.Parameters[0];
                    MemberExpression memberExpr = Expression.Property(paramExpr, nameof(ISoftDelete.IsDeleted));
                    BinaryExpression body = Expression.AndAlso(predicate.Body, Expression.Equal(memberExpr, Expression.Constant(IsDeleted, propType.PropertyType)));
                    predicate = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
                }
            }
            return predicate;
        }
    }
}

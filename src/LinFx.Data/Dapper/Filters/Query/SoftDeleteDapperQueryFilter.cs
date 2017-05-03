using LinFx.Data.Dapper.Extensions;
using LinFx.Data.Dapper.Utils;
using LinFx.Domain.Entities;
using LinFx.Domain.Uow;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinFx.Data.Dapper.Filters.Query
{
    public interface IDapperQueryFilter
    {
        string FilterName { get; }

        bool IsEnabled { get; }

        IFieldPredicate ExecuteFilter<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>;

        Expression<Func<TEntity, bool>> ExecuteFilter<TEntity, TPrimaryKey>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity<TPrimaryKey>;
    }

    public class SoftDeleteDapperQueryFilter : IDapperQueryFilter
    {
        public string FilterName => DataFilters.SoftDelete;

        public bool IsEnabled => false;

        public bool IsDeleted => false;

        public IFieldPredicate ExecuteFilter<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        {
            throw new NotImplementedException();
        }

        public Expression<Func<TEntity, bool>> ExecuteFilter<TEntity, TPrimaryKey>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity<TPrimaryKey>
        {
            if (IsFilterable<TEntity, TPrimaryKey>())
            {
                PropertyInfo propType = typeof(TEntity).GetProperty(nameof(ISoftDelete.IsDeleted));
                if (predicate == null)
                {
                    predicate = ExpressionUtils.MakePredicate<TEntity>(nameof(ISoftDelete.IsDeleted), IsDeleted, propType.PropertyType);
                }
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

        private bool IsFilterable<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        {
            return typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)) && IsEnabled;
        }
    }
}

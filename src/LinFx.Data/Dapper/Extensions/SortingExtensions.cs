using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinFx.Data.Dapper.Extensions
{
    internal static class SortingExtensions
    {
        public static List<ISort> ToSortable<T>(this Expression<Func<T, object>>[] sortingExpression, bool ascending = true)
        {
            //Check.NotNullOrEmpty(sortingExpression, nameof(sortingExpression));

            var sortList = new List<ISort>();

            sortingExpression.ToList().ForEach(sortExpression =>
            {
                var sortProperty = ReflectionUtils.GetProperty(sortExpression);
                sortList.Add(new Sort { Ascending = ascending, PropertyName = sortProperty.Name });
            });

            return sortList;
        }
    }
}

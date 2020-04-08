using LinFx.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinFx.Extensions.Dapper
{
    internal static class SortingExtensions
    {
        public static Sorting[] ToSorting<T>(this Expression<Func<T, object>>[] sortingExpression, bool ascending = true)
        {
			var sortList = new List<Sorting>();
			sortingExpression.ToList().ForEach(sortExpression =>
			{
				var sortProperty = Reflection.GetProperty(sortExpression);
				sortList.Add(new Sorting { Ascending = ascending, PropertyName = sortProperty.Name });
			});
			return sortList.ToArray();
		}
    }
}

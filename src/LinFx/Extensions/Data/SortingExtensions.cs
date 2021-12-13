using System.Collections.Generic;

namespace LinFx.Extensions.Data;

public static class SortingExtensions
{
    public static Sorting[] ToSorting(this string sortby)
    {
        var sorts = new List<Sorting>();
        if (!string.IsNullOrWhiteSpace(sortby))
        {
            var collection = sortby.Split(',');
            foreach (string c in collection)
            {
                var order = c.Substring(0, 1);
                var name = c.Remove(0, 1);
                var sort = new Sorting()
                {
                    Ascending = order == "+",
                    PropertyName = name
                };
                sorts.Add(sort);
            }
        }
        return sorts.ToArray();
    }
}

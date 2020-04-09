using System.Collections.Generic;

namespace LinFx.Data.Abstractions
{
    public class Sorting
    {
        public bool Ascending { get; set; }
        public string PropertyName { get; set; }
    }

    public static class SortingExt
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
}

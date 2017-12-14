using LinFx.Data;
using System.Collections.Generic;

namespace LinFx.Utils
{
    public class UriUtils
    {
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToFilter(string filter)
        {
            var dic = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                if (filter.EndsWith(";"))
                    filter = filter.Remove(filter.Length - 1);

                var collection = filter.Split(';');
                foreach (string c in collection)
                {
                    var val = c.Split(':');
                    if (!dic.ContainsKey(val[0]) && !string.IsNullOrEmpty(val[1]))
                        dic.Add(val[0], val[1]);
                }
            }
            return dic;
        }

		public static Sorting[] ToSorting(string sortby)
		{
			var sorts = new List<Sorting>();
			if(!string.IsNullOrWhiteSpace(sortby))
			{
				var collection = sortby.Split(',');
				foreach(string c in collection)
				{
					var order = c.Substring(0, 1);
					var name = c.Remove(0, 1);
					var sort = new Sorting()
					{
						Ascending = (order == "-"),
						PropertyName = name
					};
					sorts.Add(sort);
				}
			}
			return sorts.ToArray();
		}
    }
}

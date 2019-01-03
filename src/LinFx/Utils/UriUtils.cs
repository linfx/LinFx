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
    }
}

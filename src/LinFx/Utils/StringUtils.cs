using System.Text.RegularExpressions;

namespace LinFx.Utils
{
    public class StringUtils
    {
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="str">需要过滤的字符串</param>
        /// <returns>过滤好的字符串</returns>
        public static string Filter(string str)
        {
            if (str == "")
                return str;
            else
            {
                str = str.Replace("'", "");
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                str = str.Replace("%", "");
                str = str.Replace("'delete", "");
                str = str.Replace("''", "");
                str = str.Replace("\"\"", "");
                str = str.Replace(",", "");
                str = str.Replace(".", "");
                str = str.Replace(">=", "");
                str = str.Replace("=<", "");
                str = str.Replace("-", "");
                str = str.Replace("_", "");
                str = str.Replace(";", "");
                str = str.Replace("||", "");
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                str = str.Replace("&", "");
                str = str.Replace("#", "");
                str = str.Replace("/", "");
                str = str.Replace("-", "");
                str = str.Replace("|", "");
                str = str.Replace("?", "");
                str = str.Replace(">?", "");
                str = str.Replace("?<", "");
                str = str.Replace(" ", "");

                str = Regex.Replace(str, @"\p{Cs}", "");
                return str;
            }

        }
    }
}

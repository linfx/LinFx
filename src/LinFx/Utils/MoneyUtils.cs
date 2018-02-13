namespace LinFx.Utils
{
    public static class MoneyUtils
    {
        public static string ToFen(decimal yuan)
        {
            int result = (int)(yuan * 100);
            return result.ToString();
        }

        /// <summary>
        /// 保留两位小数
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static string ToN2String(decimal amt)
        {
            return string.Format("{0:N2}", amt);
        }


        /// <summary>
        /// 保留两位小数
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string amt)
        {
            decimal.TryParse(amt, out decimal tmpAmt);
            return tmpAmt;
        }
    }

    public static class MoneyEx
    {
        public static string ToMoneyString(this decimal d)
        {
            return string.Format("{0:N2}", d);
        }

        public static decimal ToMoneyDecimal(this string amt)
        {
            decimal.TryParse(amt, out decimal tmpAmt);
            return tmpAmt;
        }
    }
}
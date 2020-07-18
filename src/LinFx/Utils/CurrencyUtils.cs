using System;
using System.Collections.Generic;
using System.Globalization;

namespace LinFx.Utils
{
    public static class CurrencyUtils
    {
        private static readonly List<string> _zeroDecimalCurrencies = new List<string> { "BIF", "DJF", "JPY", "KRW", "PYG", "VND", "XAF", "XPF", "CLP", "GNF", "KMF", "MGA", "RWF", "VUV", "XOF" };

        public static bool IsZeroDecimalCurrencies()
        {
            var regionInfo = new RegionInfo(CultureInfo.CurrentCulture.LCID);
            return _zeroDecimalCurrencies.Contains(regionInfo.ISOCurrencySymbol);
        }
    }

    public static class MoneyUtils
    {
        /// <summary>
        /// 元转分
        /// </summary>
        /// <param name="yuan"></param>
        /// <returns></returns>
        public static string ToFen(decimal yuan)
        {
            int result = (int)(yuan * 100);
            return result.ToString();
        }

        /// <summary>
        /// 分转元
        /// </summary>
        /// <param name="fen"></param>
        /// <returns></returns>
        public static double ToYuan(int fen)
        {
            int result = fen / 100;
            return result;
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
        /// 截取两位小数
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static decimal Truncate(decimal amt)
        {
            if (amt < 0)
                throw new Exception("金额不能为负数!");

            if (amt < 0.01m)
                return 0;

            amt = Math.Truncate(amt * 100) / 100;
            return amt;
        }
    }
}

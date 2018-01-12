namespace LinFx.Utils
{
    public static class MoneyUtils
    {
        public static string ToFen(decimal yuan)
        {
            int result = (int)(yuan * 100);
            return result.ToString();
        }
    }
}

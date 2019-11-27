namespace LinFx.Data.Abstractions
{
    public class Paging
    {
        public uint Page { get; set; } = 1;

        public uint Limit { get; set; } = 20;

        public Paging(uint page, uint limit)
        {
            Page = page;
            Limit = limit;
        }
    }
}

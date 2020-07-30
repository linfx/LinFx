namespace LinFx.Data
{
    public class Paging
    {
        public int Page { get; set; } = 1;

        public int Limit { get; set; } = 20;

        public Paging(int page, int limit)
        {
            Page = page;
            Limit = limit;
        }
    }
}

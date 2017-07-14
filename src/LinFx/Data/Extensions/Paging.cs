namespace LinFx.Data.Extensions
{
	public class Paging
	{
		public int Page { get; set; }
		public int Limit { get; set; } = 20;

		public Paging() { }

		public Paging(int page, int limit)
		{
			Page = page;
			Limit = limit;
		}
	}
}

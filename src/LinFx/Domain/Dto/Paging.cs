namespace LinFx.Domain.Dto
{
	public class Paging
	{
		public int Page { get; set; }
		public int Limit { get; set; }

		public Paging(int page, int limit)
		{
			Page = page;
			Limit = limit;
		}
	}
}

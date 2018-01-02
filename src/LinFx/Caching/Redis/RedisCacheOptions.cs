namespace LinFx.Caching.Redis
{
	public class RedisCacheOptions
    {
		private const string ConnectionStringKey = "LinFx.Redis.Cache";
		private const string DatabaseIdSettingKey = "LinFx.Redis.Cache.DatabaseId";

		public string ConnectionString { get; set; } = "localhost";

		public int DatabaseId { get; set; } = -1;

		public RedisCacheOptions()
		{
		}
	}
}

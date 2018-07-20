namespace LinFx.Extensions.Caching.Redis
{
	public class RedisCacheOptions
    {
		private const string ConnectionStringKey = "LinFx.Redis.Cache";
		private const string DatabaseIdSettingKey = "LinFx.Redis.Cache.DatabaseId";

		public string ConnectionString { get; set; } = "localhost";

        public string Password { get; set; }

        public string ChannelPrefix { get; set; }

        public int DatabaseId { get; set; } = -1;

		public RedisCacheOptions()
		{
		}
	}
}

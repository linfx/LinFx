using System.Threading.Tasks;

namespace LinFx.Caching.Redis
{
	/// <summary>
	///     Interface to be implemented by all custom (de)serialization methods used when persisting and retrieving
	///     objects from the Redis cache.
	/// </summary>
	public interface IRedisCacheSerializer
    {
		/// <summary>
		///     Creates an instance of the object from its serialized string representation.
		/// </summary>
		/// <param name="objbyte">String representation of the object from the Redis server.</param>
		/// <returns>Returns a newly constructed object.</returns>
		/// <seealso cref="Serialize" />
		Task<T> DeserializeAsync<T>(string value);

		/// <summary>
		///     Produce a string representation of the supplied object.
		/// </summary>
		/// <param name="value">Instance to serialize.</param>
		/// <param name="type">Type of the object.</param>
		/// <returns>Returns a string representing the object instance that can be placed into the Redis cache.</returns>
		/// <seealso cref="DeserializeAsync" />
		Task<string> SerializeAsync<T>(T value);
	}

	/// <summary>
	///     Default implementation uses JSON as the underlying persistence mechanism.
	/// </summary>
	public class DefaultRedisCacheSerializer : IRedisCacheSerializer
	{
		public virtual Task<T> DeserializeAsync<T>(string value)
		{
			return Task.Factory.StartNew(() => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value));
		}

		public Task<string> SerializeAsync<T>(T value)
		{
			return Task.Factory.StartNew(() => Newtonsoft.Json.JsonConvert.SerializeObject(value));
		}
	}
}

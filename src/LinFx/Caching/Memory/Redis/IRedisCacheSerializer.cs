using LinFx.Utils;
using StackExchange.Redis;
using System;

namespace LinFx.Caching.Memory.Redis
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
		object Deserialize(RedisValue objbyte);

		/// <summary>
		///     Produce a string representation of the supplied object.
		/// </summary>
		/// <param name="value">Instance to serialize.</param>
		/// <param name="type">Type of the object.</param>
		/// <returns>Returns a string representing the object instance that can be placed into the Redis cache.</returns>
		/// <seealso cref="Deserialize" />
		string Serialize(object value, Type type);
	}

	/// <summary>
	///     Default implementation uses JSON as the underlying persistence mechanism.
	/// </summary>
	public class DefaultRedisCacheSerializer : IRedisCacheSerializer
	{
		/// <summary>
		///     Creates an instance of the object from its serialized string representation.
		/// </summary>
		/// <param name="objbyte">String representation of the object from the Redis server.</param>
		/// <returns>Returns a newly constructed object.</returns>
		/// <seealso cref="IRedisCacheSerializer.Serialize" />
		public virtual object Deserialize(RedisValue objbyte)
		{
			return JsonUtils.DeserializeWithType(objbyte);
		}

		/// <summary>
		///     Produce a string representation of the supplied object.
		/// </summary>
		/// <param name="value">Instance to serialize.</param>
		/// <param name="type">Type of the object.</param>
		/// <returns>Returns a string representing the object instance that can be placed into the Redis cache.</returns>
		/// <seealso cref="IRedisCacheSerializer.Deserialize" />
		public virtual string Serialize(object value, Type type)
		{
			return JsonUtils.SerializeWithType(value, type);
		}
	}
}

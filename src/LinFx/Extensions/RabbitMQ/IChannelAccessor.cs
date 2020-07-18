using System;
using RabbitMQ.Client;

namespace LinFx.Extensions.RabbitMq
{
    /// <summary>
    /// channel accessor
    /// </summary>
    public interface IChannelAccessor : IDisposable
    {
        /// <summary>
        /// Name of the channel.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Reference to the channel.
        /// Never dispose the <see cref="Channel"/> object.
        /// Instead, dispose the <see cref="IChannelAccessor"/> after usage.
        /// </summary>
        IModel Channel { get; }
    }
}
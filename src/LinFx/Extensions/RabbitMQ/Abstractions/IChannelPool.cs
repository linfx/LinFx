using System;

namespace LinFx.Extensions.RabbitMQ
{
    public interface IChannelPool : IDisposable
    {
        IChannelAccessor Acquire(string channelName = null, string connectionName = null);
    }
}
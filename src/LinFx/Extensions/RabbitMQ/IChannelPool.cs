using System;

namespace LinFx.Extensions.RabbitMq
{
    public interface IChannelPool : IDisposable
    {
        /// <summary>
        /// 获得
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        IChannelAccessor Acquire(string channelName = default, string connectionName = default);
    }
}
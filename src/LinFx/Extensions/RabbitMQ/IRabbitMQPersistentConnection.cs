using RabbitMQ.Client;
using System;

namespace LinFx.Extensions.RabbitMq
{
    /// <summary>
    /// 连接
    /// </summary>
    public interface IRabbitMqPersistentConnection : IDisposable
    {
        /// <summary>
        /// 是否连接
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 尝试连接
        /// </summary>
        /// <returns></returns>
        bool TryConnect();

        /// <summary>
        /// 创建Model
        /// </summary>
        /// <returns></returns>
        IModel CreateModel();
    }
}

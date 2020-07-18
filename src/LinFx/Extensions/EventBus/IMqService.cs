using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.Abstractions
{
    public interface IMqService
    {
        /// <summary>
        /// 发送
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendAsync<T>(string queue, T message) where T : class;

        /// <summary>
        /// 接收
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Result ReceiveAsync<T>(string queue, Action<T> callback) where T : class;
    }
}

﻿using RabbitMQ.Client;

namespace LinFx.Extensions.RabbitMQ;

/// <summary>
/// 连接池
/// </summary>
public interface IConnectionPool : IDisposable
{
    /// <summary>
    /// 获取连接
    /// </summary>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    IConnection Get(string? connectionName = default);
}

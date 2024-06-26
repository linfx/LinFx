﻿namespace LinFx.Extensions.EventBus.Distributed;

/// <summary>
/// 分布式事件总线
/// </summary>
public interface IDistributedEventBus : IEventBus
{
    /// <summary>
    /// Registers to an event. 
    /// Same (given) instance of the handler is used for all event occurrences.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="handler">Object to handle the event</param>
    IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class;
}

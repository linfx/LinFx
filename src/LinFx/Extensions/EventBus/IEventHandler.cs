﻿using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus
{
    public interface IEventHandler
    {
    }

    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IEvent
    {
        Task HandleAsync(TEvent evt);
    }
}

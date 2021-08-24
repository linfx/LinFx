using System;

namespace LinFx.Extensions.EventBus
{
    public interface IEventHandlerFactory
    {
        IDisposable GetHandler();
    }
}
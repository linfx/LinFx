using LinFx.Extensions.EventBus.Handlers;
using System;

namespace LinFx.Test.Extensions.EventBus
{
    public class MySimpleHandler : IEventHandler<MySimpleEventData>
    {
        public void HandleEvent(MySimpleEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}

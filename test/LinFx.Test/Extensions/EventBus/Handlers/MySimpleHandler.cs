using LinFx.Extensions.EventBus.Handlers;

namespace LinFx.Test.Extensions.EventBus
{
    public class MySimpleHandler : IEventHandler<MySimpleEvent>
    {
        public void HandleEvent(MySimpleEvent eventData)
        {
            eventData.Value++;
        }
    }
}

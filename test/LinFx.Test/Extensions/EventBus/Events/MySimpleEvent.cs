using LinFx.Extensions.EventBus;

namespace LinFx.Test.Extensions.EventBus
{
    public class MySimpleEvent : Event
    {
        public int Value { get; set; }

        public MySimpleEvent(int value)
        {
            Value = value;
        }
    }
}

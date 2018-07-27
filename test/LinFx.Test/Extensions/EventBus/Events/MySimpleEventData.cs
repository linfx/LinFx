using LinFx.Extensions.EventBus;

namespace LinFx.Test.Extensions.EventBus
{
    public class MySimpleEventData : Event
    {
        public int Value { get; set; }

        public MySimpleEventData(int value)
        {
            Value = value;
        }
    }
}

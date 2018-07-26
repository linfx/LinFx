using LinFx.Extensions.EventBus;

namespace LinFx.Test.Extensions.EventBus
{
    public class MySimpleEventData : EventData
    {
        public int Value { get; set; }

        public MySimpleEventData(int value)
        {
            Value = value;
        }
    }
}

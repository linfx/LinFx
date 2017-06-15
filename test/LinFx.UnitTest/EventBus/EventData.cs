using LinFx.Events.Bus;

namespace LinFx.UnitTest.SaaS.Events.Bus
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

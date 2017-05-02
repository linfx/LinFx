using LinFx.Events.Bus;
using Xunit;

namespace LinFx.SaaS.UnitTest.Events.Bus
{
    public class EventHandlerTest
    {
        IEventBus EventBus = new EventBus();

        [Fact]
        public void Should_Call_Action_On_Event_With_Correct_Source()
        {
            var totalData = 0;

            EventBus.Register<MySimpleEventData>(eventData =>
            {
                totalData += eventData.Value;
                Assert.Equal(this, eventData.EventSource);
            });

            EventBus.Trigger(this, new MySimpleEventData(1));
            EventBus.Trigger(this, new MySimpleEventData(2));
            EventBus.Trigger(this, new MySimpleEventData(3));
            EventBus.Trigger(this, new MySimpleEventData(4));

            Assert.Equal(10, totalData);
        }
    }

    public class MySimpleEventData : EventData
    {
        public int Value { get; set; }

        public MySimpleEventData(int value)
        {
            Value = value;
        }
    }
}

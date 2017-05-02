using LinFx.Events.Bus;
using Xunit;

namespace LinFx.SaaS.UnitTest.Events.Bus
{
    public class EventHandlerTest
    {
        IEventBus eventBus = EventBus.Default;

        [Fact]
        public void Should_Call_Action_On_Event_With_Correct_Source()
        {
            var totalData = 0;

            eventBus.Register<MySimpleEventData>(eventData =>
            {
                totalData += eventData.Value;
                Assert.Equal(this, eventData.EventSource);
            });

            eventBus.Trigger(this, new MySimpleEventData(1));
            eventBus.Trigger(this, new MySimpleEventData(2));
            eventBus.Trigger(this, new MySimpleEventData(3));
            eventBus.Trigger(this, new MySimpleEventData(4));

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

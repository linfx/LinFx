using LinFx.Extensions.EventBus;
using System;
using Xunit;

namespace LinFx.Test.Extensions.EventBus
{
    public class ActionBasedEventHandlerTest
    {
        readonly IEventBus _eventBus = LinFx.Extensions.EventBus.EventBus.Default;

        [Fact]
        public void Should_Call_Action_On_Event_With_Correct_Source()
        {
            var totalData = 0;

            _eventBus.Register<MySimpleEventData>(eventData =>
            {
                totalData += eventData.Value;
                Assert.Equal(this, eventData.EventSource);
            });

            _eventBus.Trigger(this, new MySimpleEventData(1));
            _eventBus.Trigger(this, new MySimpleEventData(2));
            _eventBus.Trigger(this, new MySimpleEventData(3));
            _eventBus.Trigger(this, new MySimpleEventData(4));

            Assert.Equal(10, totalData);
        }

        [Fact]
        public void Should_Call_Handler_With_Non_Generic_Trigger()
        {
            var totalData = 0;

            _eventBus.Register<MySimpleEventData>(eventData =>
            {
                totalData += eventData.Value;
                Assert.Equal(this, eventData.EventSource);
            });

            _eventBus.Trigger(typeof(MySimpleEventData), this, new MySimpleEventData(1));
            _eventBus.Trigger(typeof(MySimpleEventData), this, new MySimpleEventData(2));
            _eventBus.Trigger(typeof(MySimpleEventData), this, new MySimpleEventData(3));
            _eventBus.Trigger(typeof(MySimpleEventData), this, new MySimpleEventData(4));

            Assert.Equal(10, totalData);
        }

        [Fact]
        public void Should_Not_Call_Action_After_Unregister_1()
        {
            var totalData = 0;

            var registerDisposer = _eventBus.Register<MySimpleEventData>(eventData =>
            {
                totalData += eventData.Value;
            });

            _eventBus.Trigger(this, new MySimpleEventData(1));
            _eventBus.Trigger(this, new MySimpleEventData(2));
            _eventBus.Trigger(this, new MySimpleEventData(3));

            registerDisposer.Dispose();

            _eventBus.Trigger(this, new MySimpleEventData(4));

            Assert.Equal(6, totalData);
        }

        [Fact]
        public void Should_Not_Call_Action_After_Unregister_2()
        {
            var totalData = 0;

            var action = new Action<MySimpleEventData>(eventData =>
            {
                totalData += eventData.Value;
            });

            _eventBus.Register(action);
            
            _eventBus.Trigger(this, new MySimpleEventData(1));
            _eventBus.Trigger(this, new MySimpleEventData(2));
            _eventBus.Trigger(this, new MySimpleEventData(3));
            
            _eventBus.Unregister(action);
            
            _eventBus.Trigger(this, new MySimpleEventData(4));

            Assert.Equal(6, totalData);
        }
    }
}

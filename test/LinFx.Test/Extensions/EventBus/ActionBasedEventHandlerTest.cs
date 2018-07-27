using LinFx.Extensions.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Xunit;

namespace LinFx.Test.Extensions.EventBus
{
    public class ActionBasedEventHandlerTest
    {
        ServiceCollection _services;
        readonly IEventBus _eventBus;

        public ActionBasedEventHandlerTest()
        {
            _services = new ServiceCollection();
            _services.AddLinFx()
                .AddEventBus(options => { });

            var container = _services.BuildServiceProvider();
            _eventBus = container.GetService<IEventBus>();
        }

        [Fact]
        public void Should_Call_Handler_On_Event_With_Correct_Source()
        {
            var totalData = 0;

            //var handler = new MySimpleHandler();

            //_services.tr<MySimpleEvent, MySimpleHandler>();

            _services.Add(new ServiceDescriptor(typeof(MySimpleEvent), typeof(MySimpleHandler), ServiceLifetime.Transient));


            _eventBus.Trigger(this, new MySimpleEvent(1));
            _eventBus.Trigger(this, new MySimpleEvent(2));
            _eventBus.Trigger(this, new MySimpleEvent(3));
            _eventBus.Trigger(this, new MySimpleEvent(4));

            Assert.Equal(10, totalData);
        }


        [Fact]
        public void Should_Call_Action_On_Event_With_Correct_Source()
        {
            var totalData = 0;

            _eventBus.Register<MySimpleEvent>(eventData =>
            {
                totalData += eventData.Value;
                Assert.Equal(this, eventData.EventSource);
            });

            _eventBus.Trigger(this, new MySimpleEvent(1));
            _eventBus.Trigger(this, new MySimpleEvent(2));
            _eventBus.Trigger(this, new MySimpleEvent(3));
            _eventBus.Trigger(this, new MySimpleEvent(4));

            Assert.Equal(10, totalData);
        }

        [Fact]
        public void Should_Call_Handler_With_Non_Generic_Trigger()
        {
            var totalData = 0;

            _eventBus.Register<MySimpleEvent>(eventData =>
            {
                totalData += eventData.Value;
                Assert.Equal(this, eventData.EventSource);
            });

            _eventBus.Trigger<MySimpleEvent>(this, new MySimpleEvent(1));

            

            Assert.Equal(10, totalData);
        }

        [Fact]
        public void Should_Not_Call_Action_After_Unregister_1()
        {
            var totalData = 0;

            _eventBus.Register<MySimpleEvent>(eventData =>
            {
                totalData += eventData.Value;
            });

            _eventBus.Trigger(this, new MySimpleEvent(1));
            _eventBus.Trigger(this, new MySimpleEvent(2));
            _eventBus.Trigger(this, new MySimpleEvent(3));
            _eventBus.Trigger(this, new MySimpleEvent(4));

            Assert.Equal(6, totalData);
        }

        [Fact]
        public void Should_Not_Call_Action_After_Unregister_2()
        {
            var totalData = 0;

            var action = new Action<MySimpleEvent>(eventData =>
            {
                totalData += eventData.Value;
            });

            _eventBus.Register(action);
            
            _eventBus.Trigger(this, new MySimpleEvent(1));
            _eventBus.Trigger(this, new MySimpleEvent(2));
            _eventBus.Trigger(this, new MySimpleEvent(3));
            
            _eventBus.Unregister(action);
            
            _eventBus.Trigger(this, new MySimpleEvent(4));

            Assert.Equal(6, totalData);
        }
    }
}

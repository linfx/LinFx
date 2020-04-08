//using Automatonymous;
//using Xunit;

//namespace LinFx.Test.MassTransit.Automatonymous
//{
//    public class StateMachineTests
//    {
//        TestStateMachine _machine;
//        Instance _instance;

//        [Fact]
//        public void It_should_get_the_name_right()
//        {
//            _machine = new TestStateMachine();
//            _instance = new Instance();

//            _machine.RaiseEvent(_instance, x => x.Started).Wait();

//            //Assert.Equal("Running", _instance.CurrentState);
//        }
//    }

//    class Instance
//    {
//        /// <summary>
//        /// The CurrentState is exposed as a string for the ORM
//        /// </summary>
//        public OrderState2 CurrentState { get; private set; }
//    }


//    class TestStateMachine : AutomatonymousStateMachine<Instance>
//    {
//        public TestStateMachine()
//        {
//            InstanceState(x => x.CurrentState);

//            Initially(
//                When(Started)
//                    .TransitionTo(Running));
//        }

//        public Event Started { get; private set; }
//        public State Running { get; private set; }
//    }

//    public enum OrderState2
//    {
//        New,
//        Pay,
//    }
//}

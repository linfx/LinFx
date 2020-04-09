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
//using Automatonymous;
//using System;

//namespace LinFx.Test.MassTransit.Automatonymous
//{
//    public class Tests
//    {

//    }

//    /// <summary>
//    /// 状态机
//    /// </summary>
//    public class OrderStateMachine : MassTransitStateMachine<OrderState>
//    {
//        public State Submitted { get; private set; }

//        public State Accepted { get; private set; }

//        public Event<SubmitOrder> SubmitOrder { get; private set; }

//        public Event<OrderAccepted> OrderAccepted { get; private set; }

//        //public OrderStateMachine()
//        //{
//        //    InstanceState(x => x.CurrentState, Submitted, Accepted);
//        //}

//        public OrderStateMachine()
//        {
//            Initially(
//                When(SubmitOrder)
//                    .TransitionTo(Submitted));
//        }
//    }

//    public class OrderState : SagaStateMachineInstance
//    {
//        public Guid CorrelationId { get; set; }
//        public int CurrentState { get; set; }
//    }

//    public class SubmitOrder
//    {
//        Guid OrderId { get; }
//    }

//    public class OrderAccepted
//    {
//        Guid OrderId { get; }
//    }
//}

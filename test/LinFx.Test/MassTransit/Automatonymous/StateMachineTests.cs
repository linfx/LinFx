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

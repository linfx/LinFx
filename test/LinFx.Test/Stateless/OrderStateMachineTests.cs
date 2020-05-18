using Stateless;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace LinFx.Test.Stateless
{
    public class OrderStateMachineTests
    {
        static readonly List<Order> simpleOrders = new List<Order>()
        {
            new Order(1,"衣服","80",150),
            new Order(2,"裤子","443",100),
            new Order(3,"鞋子","3306",200),
            new Order(4,"袜子","9527",50),
        };

        [Fact]
        public void Should_be_Completed()
        {
            var order = simpleOrders.First(p => p.OrderId == 1);
            order.Jump();

            Assert.Equal(Order.OrderState.OrderPendingPay, order.State);
        }
    }

    public class Order
    {
        private readonly StateMachine<OrderState, Trigger> _machine;

        public Order(long orderId, string orderName, string orderNo, double price)
        {
            OrderId = orderId;
            OrderName = orderName;
            OrderNo = orderNo;
            Price = price;
            CreateDate = DateTime.Now;
            State = OrderState.OrderCreated;

            // 初始化状态机
            _machine = new StateMachine<OrderState, Trigger>(() => State, s => SetState(s));

            // 状态机流程配置
            _machine.Configure(OrderState.OrderCreated)
                .Permit(Trigger.Jump, OrderState.OrderPendingPay)
                .Permit(Trigger.Cancel, OrderState.OrderInvalided);

            _machine.Configure(OrderState.OrderPendingPay)
                .Permit(Trigger.Payment, OrderState.OrderPendingSend)
                .Permit(Trigger.Cancel, OrderState.OrderInvalided);

            _machine.Configure(OrderState.OrderPendingSend)
                .Permit(Trigger.Send, OrderState.OrderPendingSign)
                .Permit(Trigger.Cancel, OrderState.OrderPendingRefund);

            _machine.Configure(OrderState.OrderPendingSign)
                .Permit(Trigger.Sign, OrderState.OrderCompleted);

            _machine.Configure(OrderState.OrderPendingRefund)
                .Permit(Trigger.Refund, OrderState.OrderInvalided);
        }

        public void Jump()
        {
            _machine.Fire(Trigger.Jump);
        }

        /// <summary>
        /// 订单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 订单名称
        /// </summary>
        public string OrderName { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 订单价格
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderState State { get; private set; }

        public void SetState(OrderState state)
        {
            State = state;
        }

        /// <summary>
        /// 针对订单的操作
        /// </summary>
        public enum Trigger
        {
            /// <summary>
            /// 跳转
            /// </summary>
            Jump,

            /// <summary>
            /// 取消
            /// </summary>
            Cancel,

            /// <summary>
            /// 支付
            /// </summary>
            Payment,

            /// <summary>
            /// 配送
            /// </summary>
            Send,

            /// <summary>
            /// 签订
            /// </summary>
            Sign,

            /// <summary>
            /// 退款
            /// </summary>
            Refund,
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public enum OrderState
        {
            /// <summary>
            /// 无效
            /// </summary>
            [Description("无效")]
            OrderInvalided = 0,

            /// <summary>
            /// 已创建
            /// </summary>
            [Description("已创建")]
            OrderCreated = 3,

            /// <summary>
            /// 待支付
            /// </summary>
            [Description("待支付")]
            OrderPendingPay = 6,

            /// <summary>
            /// 待配送
            /// </summary>
            [Description("待配送")]
            OrderPendingSend = 9,

            /// <summary>
            /// 待收货
            /// </summary>
            [Description("待收货")]
            OrderPendingSign = 12,

            /// <summary>
            /// 待退款
            /// </summary>
            [Description("待退款")]
            OrderPendingRefund = 15,

            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")]
            OrderCompleted = 18,
        }
    }
}

using LinFx.Domain.Entities;
using LinFx.Test.EventBus.Events;
using System.Collections.Generic;

namespace LinFx.Test.Domain.Models
{
    public class Order : AggregateRoot<int>
    {
        private readonly List<OrderItem> _orderItems = new List<OrderItem>();

        public Order(string userId, string userName)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName);

            AddDomainEvent(orderStartedDomainEvent);
        }

        public string No { get; set; }

        public void AddOrderItem()
        {
            var orderItem = new OrderItem();
            _orderItems.Add(orderItem);
        }
    }
}

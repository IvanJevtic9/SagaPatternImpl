using SagaImpl.Common.Apstraction.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SagaImpl.OrderService.Entities
{
    public class OrderEntity : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public virtual List<OrderItemEntity> OrderItems { get; private set; }

        public decimal TotalPrice { get; private set; } = 0;

        public OrderStatus Status { get; set; } = OrderStatus.PENDING;

        public void AddItems(List<OrderItemEntity> items)
        {
            foreach (var item in items)
            {
                TotalPrice += item.GetTotalPrice();
            }

            OrderItems.AddRange(items);
        }

        public void RemoveItem(int id)
        {
            var product = OrderItems.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                OrderItems.Remove(product);
                TotalPrice -= product.GetTotalPrice();
            }
        }
    }
}


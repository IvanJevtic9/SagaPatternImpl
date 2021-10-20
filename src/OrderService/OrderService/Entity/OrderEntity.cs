using OrderService.SharedKernel.Abstraction;
using OrderService.SharedKernel.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Entity
{
    public class OrderEntity : IEntity
    {
        public int Id { get; init; }

        public int UserId { get; init; }

        public DateTimeOffset CreatedDate { get; init; }

        public List<OrderItem> OrderItems { get; private set; }

        public decimal TotalPrice { get; private set; } = 0;
    
        public void AddItems(List<OrderItem> items)
        {
            foreach (var item in items)
            {
                TotalPrice += item.GetTotalPrice();
            }

            OrderItems.AddRange(items);
        }

        public void DeepCopy(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(int id)
        {
            var product = OrderItems.FirstOrDefault(p => p.Id == id);

            if(product != null)
            {
                OrderItems.Remove(product);
                TotalPrice -= product.GetTotalPrice();
            }
        }
    }
}


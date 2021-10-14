using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public List<ProductEntity> OrderItems { get; private set; }
        public decimal TotalPrice { get; private set; }
    
        public void AddItem(ProductEntity product)
        {
            OrderItems.Add(product);
            TotalPrice += product.Price;
        }

        public void RemoveItem(int id)
        {
            var product = OrderItems.FirstOrDefault(p => p.Id == id);

            if(product != null)
            {
                OrderItems.Remove(product);
                TotalPrice -= product.Price;
            }
        }
    }
}


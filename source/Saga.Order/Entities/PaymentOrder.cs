using Saga.Common;
using Saga.Common.Data.Abstraction;

namespace Saga.Order.Entities
{
    public class PaymentOrder : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Currency { get; set; }
        public decimal TotalPrice { get; private set; } = 0;
        public string OrderStatus { get; set; } = OrderStatusEnum.Pending.ToString();
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;
        public virtual ICollection<PaymentOrderItem> Items { get; set; }

        public void AddItems(ICollection<PaymentOrderItem> items)
        {
            foreach (var item in items)
            {
                TotalPrice += item.GetTotalPrice();

                Items.Add(item);
            }
        }

        public void RemoveItem(int id)
        {
            var product = Items.FirstOrDefault(p => p.Id == id);
            
            if(product != null)
            {
                Items.Remove(product);
                TotalPrice -= product.GetTotalPrice();
            }
        }
    }
}

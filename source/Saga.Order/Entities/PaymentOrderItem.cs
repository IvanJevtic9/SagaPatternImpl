using Saga.Common.Data.Abstraction;

namespace Saga.Order.Entities
{
    public class PaymentOrderItem : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int NumberOf { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public virtual PaymentOrder Order { get; set; }

        public decimal GetTotalPrice()
        {
            return NumberOf * Price;
        }
    }
}

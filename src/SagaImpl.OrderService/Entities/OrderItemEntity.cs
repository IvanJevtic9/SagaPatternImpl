using SagaImpl.Common.Apstraction.Interface;

namespace SagaImpl.OrderService.Entities
{
    public class OrderItemEntity : IEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int NumberOf { get; set; }

        public decimal Price { get; set; }

        public string DisplayMessage { get; set; }

        public virtual OrderEntity Order { get; set; }

        public decimal GetTotalPrice()
        {
            return NumberOf * Price;
        }
    }
}

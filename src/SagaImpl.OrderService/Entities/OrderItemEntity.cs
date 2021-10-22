using SagaImpl.Common.Apstraction.Interface;
using SagaImpl.Common.Model;

namespace SagaImpl.OrderService.Entities
{
    public class OrderItemEntity : OrderItem, IEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public virtual OrderEntity Order { get; set; }
    }
}

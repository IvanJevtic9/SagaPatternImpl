using SagaImpl.Common.Apstraction.Interface;

namespace SagaImpl.OrderService.Entities
{
    public class OrderStatus : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}

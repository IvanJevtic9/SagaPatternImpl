using Saga.Common.Data.Abstraction;

namespace Saga.Order.Entities
{
    public class Product : IEntity 
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

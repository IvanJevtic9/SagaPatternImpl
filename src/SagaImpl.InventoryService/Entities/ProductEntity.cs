using SagaImpl.Common.Apstraction.Interface;

namespace SagaImpl.InventoryService.Entities
{
    public class ProductEntity : IEntity
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"Product name: {Name}.\nQuantity: {Quantity}.\nPrice: {Price}.";
        }
    }
}

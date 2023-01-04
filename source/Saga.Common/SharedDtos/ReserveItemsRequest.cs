namespace Saga.Common.SharedDtos
{
    public class Item
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class ReserveItemsRequest
    {
        public string CorrelationId { get; set; }
        public List<Item> Items { get; set; } = new();
        public bool UndoOperation { get; set; } = false;
    }
}

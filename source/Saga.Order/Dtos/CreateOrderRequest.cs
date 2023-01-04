using Saga.Common;

namespace Saga.Order.Dtos
{
    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public CurrencyEnum Currency { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}

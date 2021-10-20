namespace OrderService.SharedKernel.Model
{
    public class OrderItem
    {
        public int Id { get; init; }

        public int Number { get; init; }

        public decimal Price { get; init; }

        public string DisplayMessage { get; init; }

        public decimal GetTotalPrice()
        {
            return Number * Price;
        }
    }
}

namespace SagaImpl.Common.Model
{
    public class OrderItem
    {
        public int NumberOf { get; set; }

        public decimal Price { get; set; }

        public string DisplayMessage { get; set; }

        public decimal GetTotalPrice()
        {
            return NumberOf * Price;
        }
    }
}

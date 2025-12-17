namespace PizzaOrders.Application.DTOs
{
    public class PaymentRequestDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}

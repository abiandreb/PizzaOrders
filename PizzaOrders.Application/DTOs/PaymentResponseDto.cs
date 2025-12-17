namespace PizzaOrders.Application.DTOs
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
    }
}

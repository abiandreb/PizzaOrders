using PizzaOrders.Domain.Common;
using PizzaOrders.Domain.Entities.Orders;

namespace PizzaOrders.Domain.Entities.Payment;

public class PaymentEntity : BaseEntity
{
    public int OrderId { get; set; }
    public OrderEntity Order { get; set; }

    public PaymentMethod Method { get; set; }        // e.g. Cash, Online
    public PaymentStatus Status { get; set; }        // e.g. Pending, Paid, Failed, Refunded

    public decimal Amount { get; set; }
    public string TransactionId { get; set; }        // From external gateway (mock for now)
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public string Gateway { get; set; }              // e.g. "MockPay", "Stripe", "PayU"
}

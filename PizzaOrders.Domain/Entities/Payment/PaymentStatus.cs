namespace PizzaOrders.Domain.Entities.Payment;

public enum PaymentStatus
{
    Pending = 0,
    Paid = 1,
    Failed = 2,
    Refunded = 3
}
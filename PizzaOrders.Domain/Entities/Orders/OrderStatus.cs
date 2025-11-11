namespace PizzaOrders.Domain.Entities.Orders;

public enum OrderStatus
{
    New,
    PaymentPending,
    Paid,
    Accepted,
    Preparing,
    Ready,
    Delivering,
    Delivered,
    Completed,
    Cancelled,
    Failed
}
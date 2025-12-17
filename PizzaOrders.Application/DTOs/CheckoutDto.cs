using PizzaOrders.Domain.Entities.Orders;

namespace PizzaOrders.Application.DTOs;

public class CheckoutResponse
{
    public int OrderId { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public List<CheckoutOrderItem> Items { get; set; } = new();
}

public class CheckoutOrderItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public ItemModifiers? Modifiers { get; set; }
}

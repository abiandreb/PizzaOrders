using PizzaOrders.Domain.Entities.Orders;

namespace PizzaOrders.Application.DTOs;

public class CartDto
{
    public List<CartItem> Items { get; set; } = new();
}

public class CartItem
{
    public int ProductId { get; set; }
    public ItemModifiers? Modifiers { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
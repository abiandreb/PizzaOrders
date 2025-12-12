using PizzaOrders.Domain.Entities.Orders;
using System;

namespace PizzaOrders.Application.DTOs;

public class CartDto
{
    public Guid SessionId { get; set; }
    public List<CartItem> Items { get; set; } = new();

    public CartDto(Guid sessionId)
    {
        SessionId = sessionId;
    }
}

public class CartItem
{
    public int ProductId { get; set; }
    public ItemModifiers Modifiers { get; set; } = new();
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
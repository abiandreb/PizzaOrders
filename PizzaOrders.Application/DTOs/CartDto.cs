using PizzaOrders.Domain.Entities.Orders;
using System;

namespace PizzaOrders.Application.DTOs;

public class CartDto
{
    public Guid SessionId { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }

    public CartDto(Guid sessionId)
    {
        SessionId = sessionId;
    }
}

public class CartItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal BasePrice { get; set; }
    public List<int> ToppingIds { get; set; } = new();
    public List<CartToppingDto> Toppings { get; set; } = new();
    public decimal TotalPrice { get; set; }
}

public class CartToppingDto
{
    public int ToppingId { get; set; }
    public string ToppingName { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
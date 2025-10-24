using System.Collections.Generic;

namespace PizzaOrders.Application.DTOs;

public class CreateOrderDto
{
    public int UserId { get; set; }
    public List<CreateOrderItemDto> Items { get; set; } = new();
}

public class CreateOrderItemDto
{
    public int PizzaId { get; set; }
    public int Quantity { get; set; }
}
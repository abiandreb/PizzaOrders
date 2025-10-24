using System;
using System.Collections.Generic;

namespace PizzaOrders.Application.DTOs;

public class OrderDto
{
    public int? Id { get; set; }
    public string? Status { get; set; }
    public decimal? TotalPrice { get; set; }
    public DateTime? CreatedAt { get; set; }
    public ICollection<OrderItemDto>? OrderItems { get; set; } = new List<OrderItemDto>();
}
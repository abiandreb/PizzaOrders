using System;
using System.Collections.Generic;

namespace PizzaOrders.Application.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public List<OrderItemModifierDto> Modifiers { get; set; } = new();
    }

    public class OrderItemModifierDto
    {
        public int ToppingId { get; set; }
        public string ToppingName { get; set; }
        public decimal Price { get; set; }
    }
}

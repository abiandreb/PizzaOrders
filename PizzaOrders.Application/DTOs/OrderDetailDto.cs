namespace PizzaOrders.Application.DTOs;

public class OrderDetailDto
{
    public int OrderId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public List<OrderDetailItemDto> Items { get; set; } = new();
}

public class OrderDetailItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string? Size { get; set; }
    public List<OrderItemModifierDto> Modifiers { get; set; } = new();
}

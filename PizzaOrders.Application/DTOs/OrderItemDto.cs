namespace PizzaOrders.Application.DTOs;

public class OrderItemDto
{
    public int? Id { get; set; }
    public int? OrderId { get; set; }
    public int? PizzaId { get; set; }
    public int? Quantity { get; set; }
    public decimal? ItemPrice { get; set; }
}
namespace PizzaOrders.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int PizzaId { get; set; }
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
    
    public Order Order { get; set; } = null!;
    public Pizza Pizza { get; set; } = null!;
}
namespace PizzaOrders.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
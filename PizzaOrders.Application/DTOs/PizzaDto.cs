namespace PizzaOrders.Application.DTOs;

public class PizzaDto
{
    public int? Id { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}
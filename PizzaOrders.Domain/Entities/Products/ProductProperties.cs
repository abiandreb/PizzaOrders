using Microsoft.EntityFrameworkCore;
using PizzaOrders.Domain.Entities.Toppings;

namespace PizzaOrders.Domain.Entities.Products;

[Owned]
public class ProductProperties
{
    public List<SizeOption> SizeOptions { get; set; } = new();
    public List<int> DefaultToppingIds { get; set; } = new();
    public List<int> AvailableExtraToppingIds { get; set; } = new();
}

[Owned]
public class SizeOption
{
    public string Size { get; set; }
    public decimal Price { get; set; }
}
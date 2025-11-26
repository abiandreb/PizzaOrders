using Microsoft.EntityFrameworkCore;

namespace PizzaOrders.Domain.Entities.Orders;

public class ItemModifiers
{
    public string Size { get; set; }
    public List<SelectedItemTopping> ExtraToppings { get; set; } = new();
}

public class SelectedItemTopping
{
    public int ToppingId { get; set; }
    public int Quantity { get; set; }
    public decimal? Price { get; set; }
}

using PizzaOrders.Domain.Common;
using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Domain.Entities.Toppings;

public class ToppingEntity : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
}
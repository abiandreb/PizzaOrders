using PizzaOrders.Domain.Common;
using PizzaOrders.Domain.Entities.Toppings;

namespace PizzaOrders.Domain.Entities.Products;

public class ProductToppingEntity : BaseEntity
{
    public int ProductId { get; set; }
    public ProductEntity Product { get; set; }
    
    public int ToppingId { get; set; }
    public ToppingEntity Topping { get; set; }
}
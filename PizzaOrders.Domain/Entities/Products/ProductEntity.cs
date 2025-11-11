using PizzaOrders.Domain.Common;

namespace PizzaOrders.Domain.Entities.Products;

public class ProductEntity : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool HasToppings { get; set; }
    public decimal BasePrice { get; set; }
    public string ImageUrl { get; set; }
    public ProductType ProductType { get; set; }

    public ICollection<ProductToppingEntity> ProductToppings { get; set; } = new List<ProductToppingEntity>();
}
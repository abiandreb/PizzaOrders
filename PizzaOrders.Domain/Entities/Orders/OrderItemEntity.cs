using PizzaOrders.Domain.Common;
using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Domain.Entities.Orders;

public class OrderItemEntity : BaseEntity
{
    public int OrderId { get; set; }
    public OrderEntity Order { get; set; }

    public int ProductId { get; set; }
    public ProductEntity Product { get; set; }

    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public ICollection<OrderItemToppingEntity> OrderItemToppings { get; set; } = new List<OrderItemToppingEntity>();
}
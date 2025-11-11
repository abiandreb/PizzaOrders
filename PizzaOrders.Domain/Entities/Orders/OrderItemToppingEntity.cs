using PizzaOrders.Domain.Common;

namespace PizzaOrders.Domain.Entities.Orders;

public class OrderItemToppingEntity : BaseEntity
{
    public int OrderItemId { get; set; }
    public OrderItemEntity OrderItem { get; set; }

    public int ToppingId { get; set; }      // reference
    public decimal Price { get; set; }     // snapshot
    public int Quantity { get; set; }      // e.g., double cheese = 2
}
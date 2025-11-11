using PizzaOrders.Domain.Common;
using PizzaOrders.Domain.Entities.AuthEntities;

namespace PizzaOrders.Domain.Entities.Orders;

public class OrderEntity : BaseEntity
{
    public int UserId { get; set; }
    public UserEntity UserEntity { get; set; }
}
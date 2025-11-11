using PizzaOrders.Domain.Common;
using PizzaOrders.Domain.Entities.AuthEntities;
using PizzaOrders.Domain.Entities.Payment;

namespace PizzaOrders.Domain.Entities.Orders;

public class OrderEntity : BaseEntity
{
    public int? UserId { get; set; }
    public UserEntity User { get; set; }

    public ICollection<OrderItemEntity> Items { get; set; } = new List<OrderItemEntity>();

    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }

    public int? PaymentId { get; set; }
    public PaymentEntity Payment { get; set; }
}
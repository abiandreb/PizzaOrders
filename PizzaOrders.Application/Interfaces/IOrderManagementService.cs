using PizzaOrders.Application.DTOs;
using PizzaOrders.Domain.Entities.Orders;

namespace PizzaOrders.Application.Interfaces;

public interface IOrderManagementService
{
    Task<IList<OrderAdminDto>> GetAllOrdersAsync(OrderStatus? statusFilter = null, CancellationToken cancellationToken = default);
    Task<OrderAdminDto?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default);
    Task<OrderAdminDto?> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus, CancellationToken cancellationToken = default);
}

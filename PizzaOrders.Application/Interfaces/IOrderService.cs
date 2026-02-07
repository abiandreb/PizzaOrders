using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces;

public interface IOrderService
{
    Task<IList<OrderDto>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default);
    Task<OrderDetailDto?> GetUserOrderByIdAsync(int orderId, int userId, CancellationToken cancellationToken = default);
}

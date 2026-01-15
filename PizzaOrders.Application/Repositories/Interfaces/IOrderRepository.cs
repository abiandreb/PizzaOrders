using PizzaOrders.Domain.Entities.Orders;

namespace PizzaOrders.Application.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(OrderEntity entity);
    }
}

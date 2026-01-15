using PizzaOrders.Domain.Entities.Toppings;

namespace PizzaOrders.Application.Repositories.Interfaces
{
    public interface IToppingRepository
    {
        Task<ToppingEntity> GetByIdAsync(int id);
        Task<IReadOnlyList<ToppingEntity>> GetAllAsync();
        Task<IReadOnlyList<ToppingEntity>> GetByIdsAsync(IReadOnlyList<int> ids);
        Task AddAsync(ToppingEntity entity);
        Task UpdateAsync(ToppingEntity entity);
        Task DeleteAsync(ToppingEntity entity);
    }
}

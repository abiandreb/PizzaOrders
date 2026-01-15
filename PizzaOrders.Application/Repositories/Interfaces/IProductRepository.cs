using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Application.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductEntity> GetByIdAsync(int id);
        Task<IReadOnlyList<ProductEntity>> GetAllAsync();
        Task<IReadOnlyList<ProductEntity>> GetByIdsAsync(IReadOnlyList<int> ids);
        Task AddAsync(ProductEntity entity);
        Task UpdateAsync(ProductEntity entity);
        Task DeleteAsync(ProductEntity entity);
    }
}

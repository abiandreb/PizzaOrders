using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces;

public interface IProductService
{
    Task<IList<ProductResponse>> GetAllProductsByType(int productType, CancellationToken cancellationToken = default);
    Task<ProductResponse?> GetProductById(int id, CancellationToken none);
}
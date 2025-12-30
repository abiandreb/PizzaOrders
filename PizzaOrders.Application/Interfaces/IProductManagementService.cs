using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces
{
    public interface IProductManagementService
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse> CreateProductAsync(CreateProductRequestDto request);
        Task<ProductResponse> UpdateProductAsync(UpdateProductRequestDto request);
        Task DeleteProductAsync(int productId);
    }
}

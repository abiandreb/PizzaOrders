using System.Threading.Tasks;
using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces
{
    public interface IProductManagementService
    {
        Task<ProductResponse> CreateProductAsync(CreateProductRequestDto request);
        Task<ProductResponse> UpdateProductAsync(UpdateProductRequestDto request);
        Task DeleteProductAsync(int productId);
    }
}

using System.Threading.Tasks;
using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces
{
    public interface IToppingManagementService
    {
        Task<ToppingResponseDto> CreateToppingAsync(CreateToppingRequestDto request);
        Task<ToppingResponseDto> UpdateToppingAsync(UpdateToppingRequestDto request);
        Task DeleteToppingAsync(int toppingId);
    }
}

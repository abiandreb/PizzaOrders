using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces
{
    public interface IToppingManagementService
    {
        Task<IEnumerable<ToppingResponseDto>> GetAllToppingsAsync();
        Task<ToppingResponseDto> CreateToppingAsync(CreateToppingRequestDto request);
        Task<ToppingResponseDto> UpdateToppingAsync(UpdateToppingRequestDto request);
        Task DeleteToppingAsync(int toppingId);
    }
}

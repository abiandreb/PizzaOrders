using PizzaOrders.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaOrders.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(Guid sessionId);
        Task AddToCartAsync(Guid sessionId, int productId, int quantity, List<int> toppingIds);
        Task RemoveFromCartAsync(Guid sessionId, int productId, List<int> toppingIds);
        Task ClearCartAsync(Guid sessionId);
        Task UpdateCartAsync(Guid sessionId, int productId, int quantity, List<int> toppingIds);
    }
}

using System;
using System.Threading.Tasks;
using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces
{
    public interface ICheckoutService
    {
        Task<OrderDto> ProcessCheckout(Guid sessionId, int? userId = null);
    }
}
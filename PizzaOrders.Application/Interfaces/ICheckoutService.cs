using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces;

public interface ICheckoutService
{
    Task<CheckoutResponse> ProcessCheckoutAsync(Guid sessionId, int? userId = null);
}

using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateOrder(CreateOrderDto dto);
    Task<List<OrderDto>> GetOrders();
}
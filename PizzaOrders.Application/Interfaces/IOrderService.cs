using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces;

public interface IOrderService
{
    Task<OrderObject> CreateOrder(CreateOrderDto dto);
    Task<List<OrderObject>> GetOrders();
}
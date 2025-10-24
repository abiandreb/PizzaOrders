using System.Linq;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Domain.Entities;

namespace PizzaOrders.Application.Mappers;

public static class OrderMappings
{
    public static OrderObject ToOrderDto(this Order model)
    {
        return new OrderObject
        {
            Id = model.Id,
            Status = model.Status,
            TotalPrice = model.TotalPrice,
            CreatedAt = model.CreatedAt,
            OrderItems = model.OrderItems.Select(x => x.ToOrderItemDto()).ToList()
        };
    }
}
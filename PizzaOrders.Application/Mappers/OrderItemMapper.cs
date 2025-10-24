using PizzaOrders.Application.DTOs;
using PizzaOrders.Domain.Entities;

namespace PizzaOrders.Application.Mappers;

public static class OrderItemMapper
{
    public static OrderItemDto ToOrderItemDto(this OrderItem model)
    {
        return new OrderItemDto
        {
            Id = model.Id,
            OrderId = model.OrderId,
            PizzaId = model.PizzaId,
            Quantity = model.Quantity,
            ItemPrice = model.ItemPrice
        };
    }

    public static OrderItem ToOrderItemModel(this OrderItemDto dto)
    {
        return new OrderItem
        {
            OrderId = dto.OrderId.Value,
            PizzaId = dto.PizzaId.Value,
            Quantity = dto.Quantity.Value,
            ItemPrice = dto.ItemPrice.Value
        };
    }
}
using PizzaOrders.Application.DTOs;
using PizzaOrders.Domain.Entities;

namespace PizzaOrders.Application.Mappers;

public static class PizzaMappings
{
    public static Pizza ToPizzaModel(this PizzaDto dto)
    {
        return new Pizza
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price
        };
    }

    public static PizzaDto ToPizzaDto(this Pizza model)
    {
        return new PizzaDto
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Price = model.Price
        };
    }

    public static PizzaPatchDto ToPizzaPatchDto(this Pizza model)
    {
        return new PizzaPatchDto
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price
        };
    }
}
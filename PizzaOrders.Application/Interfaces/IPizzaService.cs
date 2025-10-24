using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces;

public interface IPizzaService
{
    Task<IEnumerable<PizzaDto>> GetPizzasList();

    Task<PizzaDto> GetSinglePizza(int id);

    Task<PizzaDto> AddPizza(PizzaDto pizza);

    Task DeletePizza(int id);
}
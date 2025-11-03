using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Application.Mappers;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services;

public class PizzaService : IPizzaService
{
    private readonly AppDbContext _context;
    private readonly ILogger<PizzaService> _logger;

    public PizzaService(AppDbContext context, ILogger<PizzaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<PizzaDto>> GetPizzasList()
    {
        var pizzasEntities = await _context.Pizzas.ToListAsync();

        var pizzasDto = pizzasEntities.Select(x => new PizzaDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
        }).ToList();

        return !pizzasDto.Any() ? throw new InvalidOperationException("No pizzas found") : pizzasDto;
    }

    public async Task<PizzaDto> GetSinglePizza(int id)
    {
        _logger.LogInformation("LOG: Getting pizza for {id}", id);
        
        var pizza = await _context.Pizzas.FirstOrDefaultAsync(x => x.Id == id);

        if (pizza == null)
        {
            throw new InvalidOperationException("Pizza not found");
        }
        
        return new PizzaDto
        {
            Id = pizza.Id,
            Name = pizza.Name,
            Description = pizza.Description,
            Price = pizza.Price,
        };
    }

    public async Task<PizzaDto> AddPizza(PizzaDto pizza)
    {
        var pizzaEntity = pizza.ToPizzaEntity(); 
        await _context.Pizzas.AddAsync(pizzaEntity); 
        await _context.SaveChangesAsync();

        return pizzaEntity.ToPizzaDto();
    }
    
    public async Task DeletePizza(int id)
    {
        var pizza = await _context.Pizzas.FirstOrDefaultAsync(x => x.Id == id);
        
        if (pizza == null)
        {
            throw new InvalidOperationException("Pizza not found");
        }
        
        _context.Pizzas.Remove(pizza);
        await _context.SaveChangesAsync();
    }
}
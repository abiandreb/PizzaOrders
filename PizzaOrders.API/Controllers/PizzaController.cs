using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain;

namespace PizzaOrders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = UserRolesConstants.UserRole)]
public class PizzaController : ControllerBase
{
    private readonly IPizzaService _pizzaService;

    public PizzaController(IPizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PizzaDto>>> GetPizzas()
    {
        var pizzasDto = await _pizzaService.GetPizzasList();
        
        return Ok(pizzasDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PizzaDto>> GetPizza(int id)
    {
        var pizza = await _pizzaService.GetSinglePizza(id);

        if (pizza is null) return NotFound();
        
        return Ok(pizza);
    }

    [HttpPost]
    public async Task<ActionResult<PizzaDto>> AddPizza([FromBody] PizzaDto pizzaDto)
    {
        var newPizza = await _pizzaService.AddPizza(pizzaDto);
        
        return Ok(newPizza);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePizza(int id)
    {
        await _pizzaService.DeletePizza(id);
        
        return NoContent();
    }
}

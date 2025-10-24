using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Mappers;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzaController : ControllerBase
{
    private readonly AppDbContext context;
    private readonly ILogger<PizzaController> logger;

    public PizzaController(AppDbContext context, ILogger<PizzaController> logger)
    {
        this.context = context;
        this.logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PizzaDto>>> GetPizzas()
    {

        var pizzas = await context.Pizzas.ToListAsync();

        var pizzaDto = pizzas.Select(x => new PizzaDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
        });
        
        return Ok(pizzaDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PizzaDto>> GetPizza(int id)
    {

        logger.LogInformation("LOG: Getting pizza for {id}", id);
        var pizza = await context.Pizzas.FirstOrDefaultAsync(x => x.Id == id);

        if (pizza != null)
        {
            return Ok(new PizzaDto
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Description = pizza.Description,
                Price = pizza.Price,
            });
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PizzaDto>> AddPizza([FromBody] PizzaDto pizzaDto)
    {
        var model = pizzaDto.ToPizzaModel(); 
        await context.Pizzas.AddAsync(model); 
        await context.SaveChangesAsync(); 
        return CreatedAtAction(nameof(GetPizza), new { id =  model.Id }, model.ToPizzaDto());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePizza(int id)
    {
        var pizza = await context.Pizzas.FirstOrDefaultAsync(x => x.Id == id);

        if (pizza == null)
        {
            return NotFound();
        }
        
        context.Remove(pizza);
        await context.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchPizza(int id, [FromBody] JsonPatchDocument<PizzaPatchDto>? patchDto)
    {
        if (patchDto == null) return BadRequest();
        
        var pizza = await context.Pizzas.SingleOrDefaultAsync(x => x.Id == id);
        
        if (pizza == null)
        {
            return NotFound();  
        }
                
        var pizzaDto = pizza.ToPizzaPatchDto();
            
        patchDto.ApplyTo(pizzaDto, ModelState);
            
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (pizzaDto.Name != null) pizza.Name = pizzaDto.Name;
        if (pizzaDto.Description != null) pizza.Description = pizzaDto.Description;
        if (pizzaDto.Price != null) pizza.Price = (decimal)pizzaDto.Price;
            
        await context.SaveChangesAsync();
            
        return new OkObjectResult(pizza.ToPizzaPatchDto());
    }
}
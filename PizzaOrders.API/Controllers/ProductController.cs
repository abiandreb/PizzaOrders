using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.Interfaces;

namespace PizzaOrders.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(IProductService productService, ILogger<ProductController> logger)
    : ControllerBase
{
    private readonly ILogger<ProductController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAllPizzas(int productType)
    {
        var pizzas = await productService.GetAllProductsByType(productType, CancellationToken.None);

        if (!pizzas.Any())
        {
            return NotFound("No products found");
        }
        
        return Ok(pizzas);
    }
}
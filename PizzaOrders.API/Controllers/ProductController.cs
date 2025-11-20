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
    public async Task<IActionResult> GetAllProductsByType(int productType)
    {
        var products = await productService.GetAllProductsByType(productType, CancellationToken.None);

        if (!products.Any())
        {
            return NotFound("No products found");
        }
        
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await productService.GetProductById(id, CancellationToken.None);
        
        if (product is null)
        {
            return NotFound("Product not found");
        }
        
        return Ok(product);
    }
}
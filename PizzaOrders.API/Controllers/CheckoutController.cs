using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.Interfaces;

namespace PizzaOrders.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckoutController(ICheckoutService checkoutService) : ControllerBase
{
    [HttpPost("{sessionId:guid}")]
    public async Task<IActionResult> ProcessCheckout(Guid sessionId, [FromQuery] int? userId = null)
    {
        try
        {
            var result = await checkoutService.ProcessCheckoutAsync(sessionId, userId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
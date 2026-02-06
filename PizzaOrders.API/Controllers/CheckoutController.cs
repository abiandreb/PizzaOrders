using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using System.Security.Claims;

namespace PizzaOrders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;

    public CheckoutController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    /// <summary>
    /// Process checkout for authenticated users. UserId is taken from JWT claims.
    /// </summary>
    [HttpPost("{sessionId}")]
    [Authorize]
    public async Task<IActionResult> Checkout(Guid sessionId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user token");
            }

            var order = await _checkoutService.ProcessCheckout(sessionId, userId);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Process checkout for guest users (no account required).
    /// </summary>
    [HttpPost("{sessionId}/guest")]
    [AllowAnonymous]
    public async Task<IActionResult> GuestCheckout(Guid sessionId)
    {
        try
        {
            var order = await _checkoutService.ProcessCheckout(sessionId, userId: null);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
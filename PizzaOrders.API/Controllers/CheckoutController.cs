using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace PizzaOrders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("{sessionId}")]
        public async Task<IActionResult> Checkout(Guid sessionId, [FromBody] CheckoutRequestDto? request)
        {
            try
            {
                var order = await _checkoutService.ProcessCheckout(sessionId, request?.UserId);
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
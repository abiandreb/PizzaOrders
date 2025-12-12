using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaOrders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<CartDto>> CreateCart()
        {
            var sessionId = Guid.NewGuid();
            var cart = await _cartService.GetCartAsync(sessionId);
            return Ok(cart);
        }

        [HttpGet("{sessionId}")]
        public async Task<ActionResult<CartDto>> GetCart(Guid sessionId)
        {
            var cart = await _cartService.GetCartAsync(sessionId);
            return Ok(cart);
        }

        [HttpPost("{sessionId}/add")]
        public async Task<IActionResult> AddToCart(Guid sessionId, [FromBody] CartItemRequestDto request)
        {
            await _cartService.AddToCartAsync(sessionId, request.ProductId, request.Quantity, request.ToppingIds ?? new List<int>());
            return Ok();
        }

        [HttpPut("{sessionId}/update")]
        public async Task<IActionResult> UpdateCart(Guid sessionId, [FromBody] CartItemUpdateRequestDto request)
        {
            await _cartService.UpdateCartAsync(sessionId, request.ProductId, request.Quantity);
            return Ok();
        }

        [HttpDelete("{sessionId}/remove")]
        public async Task<IActionResult> RemoveFromCart(Guid sessionId, [FromQuery] int productId)
        {
            await _cartService.RemoveFromCartAsync(sessionId, productId);
            return Ok();
        }

        [HttpDelete("{sessionId}")]
        public async Task<IActionResult> ClearCart(Guid sessionId)
        {
            await _cartService.ClearCartAsync(sessionId);
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain;
using PizzaOrders.Domain.Entities;

namespace PizzaOrders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = UserRolesConstants.UserRole)]
public class OrderController : ControllerBase
{
    private readonly IOrderService orderService;
    
    public OrderController(IOrderService orderService)
    {
        this.orderService = orderService;
    }
    
    [HttpPost]
    public async Task<ActionResult<OrderObject>> CreateOrder([FromBody] CreateOrderDto? dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var result = await orderService.CreateOrder(dto);
        
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderObject>>> GetOrders()
    {
        var result = await orderService.GetOrders();
        
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<OrderObject>>> GetOrdersByUserId(string userId)
    {
        // TODO: Implement service method
        await Task.CompletedTask;
        return Ok(new List<OrderObject>());
    }

    [HttpPut("{orderId}")]
    public async Task<ActionResult> UpdateOrderStatus(string orderId, [FromBody] string status)
    {
        // TODO: Implement service method
        await Task.CompletedTask;
        return NoContent();
    }
}
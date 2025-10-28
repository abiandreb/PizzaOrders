using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;

namespace PizzaOrders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
}
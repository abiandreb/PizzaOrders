using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain;
using PizzaOrders.Domain.Entities.Orders;

namespace PizzaOrders.API.Controllers;

[ApiController]
[Route("api/management/orders")]
[Authorize(Roles = UserRolesConstants.AdminRole)]
public class OrderManagementController(IOrderManagementService orderManagementService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllOrders([FromQuery] string? status = null, CancellationToken cancellationToken = default)
    {
        OrderStatus? statusFilter = null;
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
        {
            statusFilter = parsedStatus;
        }

        var result = await orderManagementService.GetAllOrdersAsync(statusFilter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id, CancellationToken cancellationToken = default)
    {
        var result = await orderManagementService.GetOrderByIdAsync(id, cancellationToken);
        if (result is null)
        {
            return NotFound(new { message = $"Order with ID {id} not found" });
        }
        return Ok(result);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var newStatus))
        {
            return BadRequest(new { message = $"Invalid status: {request.Status}. Valid statuses are: {string.Join(", ", Enum.GetNames<OrderStatus>())}" });
        }

        var result = await orderManagementService.UpdateOrderStatusAsync(id, newStatus, cancellationToken);
        if (result is null)
        {
            return NotFound(new { message = $"Order with ID {id} not found" });
        }
        return Ok(result);
    }

    [HttpGet("statuses")]
    public IActionResult GetAvailableStatuses()
    {
        var statuses = Enum.GetNames<OrderStatus>();
        return Ok(statuses);
    }
}

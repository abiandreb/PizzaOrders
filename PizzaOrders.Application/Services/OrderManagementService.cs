using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services;

public class OrderManagementService(
    AppDbContext dbContext,
    ILogger<OrderManagementService> logger,
    IOrderNotificationService orderNotificationService) : IOrderManagementService
{
    public async Task<IList<OrderAdminDto>> GetAllOrdersAsync(OrderStatus? statusFilter = null, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .AsQueryable();

        if (statusFilter.HasValue)
        {
            query = query.Where(o => o.Status == statusFilter.Value);
        }

        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderAdminDto
            {
                Id = o.Id,
                UserId = o.UserId,
                UserEmail = o.User != null ? o.User.Email : null,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt,
                Items = o.Items.Select(i => new OrderItemAdminDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product != null ? i.Product.Name : "Unknown Product",
                    Quantity = i.Quantity,
                    UnitPrice = i.ItemPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        // Populate NextStatuses after materialization (can't call in EF projection)
        foreach (var order in orders)
        {
            order.NextStatuses = GetNextStatuses(order.Status);
        }

        logger.LogInformation("Retrieved {Count} orders", orders.Count);
        return orders;
    }

    public async Task<OrderAdminDto?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} not found", orderId);
            return null;
        }

        return new OrderAdminDto
        {
            Id = order.Id,
            UserId = order.UserId,
            UserEmail = order.User?.Email,
            TotalPrice = order.TotalPrice,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            NextStatuses = GetNextStatuses(order.Status.ToString()),
            Items = order.Items.Select(i => new OrderItemAdminDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? "Unknown Product",
                Quantity = i.Quantity,
                UnitPrice = i.ItemPrice,
                TotalPrice = i.TotalPrice
            }).ToList()
        };
    }

    public async Task<OrderAdminDto?> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} not found for status update", orderId);
            return null;
        }

        var previousStatus = order.Status;
        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Order {OrderId} status updated from {PreviousStatus} to {NewStatus}",
            orderId, previousStatus, newStatus);

        // Notify connected clients about the status change
        await orderNotificationService.NotifyOrderStatusUpdatedAsync(
            orderId, newStatus.ToString(), order.UpdatedAt);

        return new OrderAdminDto
        {
            Id = order.Id,
            UserId = order.UserId,
            UserEmail = order.User?.Email,
            TotalPrice = order.TotalPrice,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            NextStatuses = GetNextStatuses(order.Status.ToString()),
            Items = order.Items.Select(i => new OrderItemAdminDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? "Unknown Product",
                Quantity = i.Quantity,
                UnitPrice = i.ItemPrice,
                TotalPrice = i.TotalPrice
            }).ToList()
        };
    }

    public static List<string> GetNextStatuses(string currentStatus) => currentStatus switch
    {
        "New" => ["Cancelled"],
        "PaymentPending" => ["Cancelled"],
        "Paid" => ["Accepted", "Cancelled"],
        "Accepted" => ["Preparing", "Cancelled"],
        "Preparing" => ["Ready", "Cancelled"],
        "Ready" => ["Delivering"],
        "Delivering" => ["Delivered"],
        "Delivered" => ["Completed"],
        _ => [] // Completed, Cancelled, Failed — terminal states
    };
}

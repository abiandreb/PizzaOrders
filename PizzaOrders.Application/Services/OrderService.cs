using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services;

public class OrderService(AppDbContext dbContext, ILogger<OrderService> logger) : IOrderService
{
    public async Task<IList<OrderDto>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default)
    {
        var orders = await dbContext.Orders
            .AsNoTracking()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Select(o => new OrderDto
            {
                OrderId = o.Id,
                TotalPrice = o.TotalPrice,
                OrderDate = o.CreatedAt,
                Status = o.Status.ToString(),
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product != null ? i.Product.Name : "Unknown Product",
                    Quantity = i.Quantity,
                    UnitPrice = i.ItemPrice,
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        logger.LogInformation("Retrieved {Count} orders for user {UserId}", orders.Count, userId);
        return orders;
    }

    public async Task<OrderDetailDto?> GetUserOrderByIdAsync(int orderId, int userId, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} not found for user {UserId}", orderId, userId);
            return null;
        }

        // Collect all topping IDs from item modifiers to batch-load names
        var allToppingIds = order.Items
            .Where(i => i.ItemModifiers?.ExtraToppings != null)
            .SelectMany(i => i.ItemModifiers!.ExtraToppings.Select(t => t.ToppingId))
            .Distinct()
            .ToList();

        var toppingNames = allToppingIds.Count > 0
            ? await dbContext.Toppings
                .AsNoTracking()
                .Where(t => allToppingIds.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id, t => t.Name, cancellationToken)
            : new Dictionary<int, string>();

        return new OrderDetailDto
        {
            OrderId = order.Id,
            TotalPrice = order.TotalPrice,
            OrderDate = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Status = order.Status.ToString(),
            ItemCount = order.Items.Sum(i => i.Quantity),
            Items = order.Items.Select(i => new OrderDetailItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? "Unknown Product",
                Quantity = i.Quantity,
                UnitPrice = i.ItemPrice,
                TotalPrice = i.TotalPrice,
                Size = i.ItemModifiers?.Size,
                Modifiers = i.ItemModifiers?.ExtraToppings?.Select(t => new OrderItemModifierDto
                {
                    ToppingId = t.ToppingId,
                    ToppingName = toppingNames.GetValueOrDefault(t.ToppingId, "Unknown Topping"),
                    Price = t.Price ?? 0
                }).ToList() ?? new List<OrderItemModifierDto>()
            }).ToList()
        };
    }
}

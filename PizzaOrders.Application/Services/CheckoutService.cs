using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services;

public class CheckoutService(
    AppDbContext dbContext,
    ICartService cartService,
    ILogger<CheckoutService> logger) : ICheckoutService
{
    public async Task<CheckoutResponse> ProcessCheckoutAsync(Guid sessionId, int? userId = null)
    {
        // 1. Load cart from Redis
        var cart = await cartService.GetCartAsync(sessionId);

        if (cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty");
        }

        // 2. Validate products exist and reload current prices from SQL
        var productIds = cart.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await dbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        if (products.Count != productIds.Count)
        {
            var missingIds = productIds.Except(products.Keys);
            throw new InvalidOperationException($"Products not found: {string.Join(", ", missingIds)}");
        }

        // 3. Validate toppings and reload current topping prices from SQL
        var allToppingIds = cart.Items
            .SelectMany(i => i.Modifiers?.ExtraToppings?.Select(t => t.ToppingId) ?? Enumerable.Empty<int>())
            .Distinct()
            .ToList();

        var toppings = await dbContext.Toppings
            .Where(t => allToppingIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id);

        if (allToppingIds.Count > 0 && toppings.Count != allToppingIds.Count)
        {
            var missingIds = allToppingIds.Except(toppings.Keys);
            throw new InvalidOperationException($"Toppings not found: {string.Join(", ", missingIds)}");
        }

        // 4. Create Order entity
        var order = new OrderEntity
        {
            UserId = userId,
            Status = OrderStatus.New,
            Items = new List<OrderItemEntity>()
        };

        decimal orderTotal = 0;

        // 5. Create OrderItems with recalculated prices (snapshot from SQL)
        foreach (var cartItem in cart.Items)
        {
            var product = products[cartItem.ProductId];

            // Recalculate item price from SQL
            decimal itemPrice = product.BasePrice;

            // Add topping prices
            if (cartItem.Modifiers?.ExtraToppings != null)
            {
                foreach (var topping in cartItem.Modifiers.ExtraToppings)
                {
                    if (toppings.TryGetValue(topping.ToppingId, out var toppingEntity))
                    {
                        itemPrice += toppingEntity.Price * (topping.Quantity > 0 ? topping.Quantity : 1);
                        // Update topping price snapshot
                        topping.Price = toppingEntity.Price;
                    }
                }
            }

            var orderItem = new OrderItemEntity
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                ItemPrice = itemPrice,
                TotalPrice = itemPrice * cartItem.Quantity,
                ItemModifiers = cartItem.Modifiers
            };

            order.Items.Add(orderItem);
            orderTotal += orderItem.TotalPrice;
        }

        order.TotalPrice = orderTotal;

        // 6. Save order to SQL
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

        // 7. Clear cart from Redis
        await cartService.ClearCartAsync(sessionId);

        // 8. Return response
        return new CheckoutResponse
        {
            OrderId = order.Id,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            Items = order.Items.Select(oi => new CheckoutOrderItem
            {
                ProductId = oi.ProductId,
                ProductName = products[oi.ProductId].Name,
                Quantity = oi.Quantity,
                ItemPrice = oi.ItemPrice,
                TotalPrice = oi.TotalPrice,
                Modifiers = oi.ItemModifiers
            }).ToList()
        };
    }
}

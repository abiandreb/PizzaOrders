using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services;

public class CheckoutService : ICheckoutService
{
    private readonly ICartService _cartService;
    private readonly IPaymentService _paymentService;
    private readonly AppDbContext _context;
    private readonly ILogger<CheckoutService> _logger;

    public CheckoutService(ICartService cartService, IPaymentService paymentService, AppDbContext context, ILogger<CheckoutService> logger)
    {
        _cartService = cartService;
        _paymentService = paymentService;
        _context = context;
        _logger = logger;
    }

    public async Task<OrderDto> ProcessCheckout(Guid sessionId, int? userId = null)
    {
        // 1. Get cart from Redis
        var cart = await _cartService.GetCartAsync(sessionId);
        if (cart == null || !cart.Items.Any())
        {
            throw new InvalidOperationException("Cart is empty or not found.");
        }

        // 2. Batch load all products and toppings upfront (fixes N+1 query problem)
        var productIds = cart.Items.Select(i => i.ProductId).Distinct().ToList();
        var allToppingIds = cart.Items
            .Where(i => i.ToppingIds != null)
            .SelectMany(i => i.ToppingIds)
            .Distinct()
            .ToList();

        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        var toppings = await _context.Toppings
            .Where(t => allToppingIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id);

        // Validate all products exist
        var missingProducts = productIds.Except(products.Keys).ToList();
        if (missingProducts.Any())
        {
            throw new InvalidOperationException($"Products with IDs {string.Join(", ", missingProducts)} not found.");
        }

        // Validate all toppings exist
        var missingToppings = allToppingIds.Except(toppings.Keys).ToList();
        if (missingToppings.Any())
        {
            throw new InvalidOperationException($"Toppings with IDs {string.Join(", ", missingToppings)} not found.");
        }

        // 3. Use transaction to ensure atomicity
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 4. Create a new order
            var order = new OrderEntity
            {
                Status = OrderStatus.PaymentPending,
                Items = new List<OrderItemEntity>(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            decimal totalOrderPrice = 0;

            // 5. Process each item in the cart
            foreach (var cartItem in cart.Items)
            {
                var product = products[cartItem.ProductId];
                decimal itemPrice = product.BasePrice;
                var orderItemModifiers = new ItemModifiers();

                // Process toppings if any
                if (cartItem.ToppingIds != null && cartItem.ToppingIds.Any())
                {
                    var itemToppings = cartItem.ToppingIds
                        .Select(id => toppings[id])
                        .ToList();

                    itemPrice += itemToppings.Sum(t => t.Price);

                    orderItemModifiers.ExtraToppings = itemToppings.Select(t => new SelectedItemTopping
                    {
                        ToppingId = t.Id,
                        Price = t.Price,
                        Quantity = 1
                    }).ToList();
                }

                var orderItem = new OrderItemEntity
                {
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,
                    ItemPrice = itemPrice,
                    TotalPrice = itemPrice * cartItem.Quantity,
                    ItemModifiers = orderItemModifiers,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                order.Items.Add(orderItem);
                totalOrderPrice += orderItem.TotalPrice;
            }

            order.TotalPrice = totalOrderPrice;

            // 6. Save the order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 7. Commit transaction before clearing cart
            await transaction.CommitAsync();

            // 8. Auto-process payment (mocked - always succeeds)
            await _paymentService.ProcessPayment(new PaymentRequestDto
            {
                OrderId = order.Id,
                Amount = order.TotalPrice
            });
            // Refresh status after payment
            order.Status = OrderStatus.Paid;

            // 9. Clear the cart from Redis (outside transaction - Redis is separate)
            await _cartService.ClearCartAsync(sessionId);

            _logger.LogInformation("Order {OrderId} created and paid successfully for session {SessionId}", order.Id, sessionId);

            // 10. Map to DTO and return (using already-loaded data, no additional queries)
            return MapToOrderDto(order, products, toppings);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to process checkout for session {SessionId}", sessionId);
            throw;
        }
    }

    private static OrderDto MapToOrderDto(
        OrderEntity order,
        Dictionary<int, Domain.Entities.Products.ProductEntity> products,
        Dictionary<int, Domain.Entities.Toppings.ToppingEntity> toppings)
    {
        return new OrderDto
        {
            OrderId = order.Id,
            TotalPrice = order.TotalPrice,
            OrderDate = order.CreatedAt,
            Status = order.Status.ToString(),
            Items = order.Items.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.ItemPrice,
                ProductName = products.TryGetValue(oi.ProductId, out var product) ? product.Name : string.Empty,
                Modifiers = oi.ItemModifiers?.ExtraToppings?.Select(m => new OrderItemModifierDto
                {
                    Price = m.Price ?? 0,
                    ToppingId = m.ToppingId,
                    ToppingName = toppings.TryGetValue(m.ToppingId, out var topping) ? topping.Name : string.Empty
                }).ToList() ?? new List<OrderItemModifierDto>()
            }).ToList()
        };
    }
}
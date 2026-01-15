using Microsoft.EntityFrameworkCore;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartService _cartService;
        private readonly AppDbContext _context;

        public CheckoutService(ICartService cartService, AppDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        public async Task<OrderDto> ProcessCheckout(Guid sessionId, int? userId = null)
        {
            // 1. Get cart from Redis
            var cart = await _cartService.GetCartAsync(sessionId);
            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Cart is empty or not found.");
            }

            // 2. Create a new order
            var order = new OrderEntity
            {
                Status = OrderStatus.PaymentPending,
                Items = new List<OrderItemEntity>(),
                UserId = userId
            };

            decimal totalOrderPrice = 0;

            // 3. Process each item in the cart
            foreach (var cartItem in cart.Items)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == cartItem.ProductId);

                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {cartItem.ProductId} not found.");
                }

                decimal itemPrice = product.BasePrice;
                var orderItemModifiers = new ItemModifiers();

                // 4. Process toppings if any
                if (cartItem.ToppingIds != null && cartItem.ToppingIds.Any())
                {
                    var toppings = await _context.Toppings
                        .Where(t => cartItem.ToppingIds.Contains(t.Id))
                        .ToListAsync();

                    if (toppings.Count != cartItem.ToppingIds.Count)
                    {
                        var invalidIds = cartItem.ToppingIds.Except(toppings.Select(t => t.Id)).ToList();
                        throw new InvalidOperationException($"Toppings with IDs {string.Join(", ", invalidIds)} not found.");
                    }

                    foreach (var topping in toppings)
                    {
                        itemPrice += topping.Price;
                    }

                    orderItemModifiers.ExtraToppings = toppings.Select(t => new SelectedItemTopping
                    {
                        ToppingId = t.Id,
                        Price = t.Price
                    }).ToList();
                }

                var orderItem = new OrderItemEntity
                {
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,
                    ItemPrice = itemPrice,
                    TotalPrice = itemPrice * cartItem.Quantity,
                    ItemModifiers = orderItemModifiers
                };

                order.Items.Add(orderItem);
                totalOrderPrice += orderItem.TotalPrice;
            }

            order.TotalPrice = totalOrderPrice;

            // 5. Save the order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 6. Clear the cart from Redis
            await _cartService.ClearCartAsync(sessionId);

            // 7. Map to DTO and return
            return new OrderDto
            {
                OrderId = order.Id,
                TotalPrice = order.TotalPrice,
                OrderDate = order.CreatedAt,
                Status = order.Status.ToString(),
                Items = order.Items.Select(oi =>
                {
                    var product = _context.Products.Find(oi.ProductId);
                    return new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.ItemPrice,
                        ProductName = product?.Name ?? string.Empty,
                        Modifiers = oi.ItemModifiers.ExtraToppings.Select(m =>
                        {
                            var topping = _context.Toppings.Find(m.ToppingId);
                            return new OrderItemModifierDto()
                            {
                                Price = m.Price ?? 0,
                                ToppingId = m.ToppingId,
                                ToppingName = topping?.Name ?? string.Empty
                            };
                        }).ToList()
                    };
                }).ToList()
            };
        }
    }
}
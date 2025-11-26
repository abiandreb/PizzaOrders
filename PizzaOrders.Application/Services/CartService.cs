using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services;

public class CartService(AppDbContext dbContext, ILogger<CartService> logger)
{
    public async Task<CartDto> GetCartAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task AddToCartAsync(CartDto cartDto)
    {
        ArgumentNullException.ThrowIfNull(cartDto);

        var requestedProductsIds = cartDto.Items.Select(x => x.ProductId).Distinct().ToList();

        var productInDb = await dbContext.Products.Where(x => requestedProductsIds.Contains(x.Id)).ToListAsync();

        if (requestedProductsIds.Count != productInDb.Count)
        {
            var invalidProductsIds = requestedProductsIds.Except(productInDb.Select(x => x.Id)).ToList();

            logger.LogError("Products with ids {Join} not found", string.Join(", ", invalidProductsIds));

            throw new InvalidOperationException($"Products with ids {string.Join(", ", invalidProductsIds)} not found");
        }

        var requestedToppings = cartDto.Items
            .Where(x => x.Modifiers?.ExtraToppings.Count > 0)
            .SelectMany(x => x.Modifiers.ExtraToppings)
            .Select(x => x.ToppingId)
            .Distinct()
            .ToList();

        var existingToppings = await dbContext.Toppings.Where(x => requestedToppings.Contains(x.Id)).ToListAsync();

        if (requestedToppings.Count != existingToppings.Count)
        {
            var invalidToppingsIds = requestedToppings.Except(existingToppings.Select(x => x.Id)).ToList();

            logger.LogError("Toppings with ids {Join} not found", string.Join(", ", invalidToppingsIds));
        }
    }
    
    public async Task RemoveFromCartAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task ClearCartAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<CartDto> UpdateCartAsync(CartDto cart)
    {
        throw new NotImplementedException();
    }

    public async Task AddItems(Guid sessionId, int productId, ItemModifiers modifiers)
    {

    }
}
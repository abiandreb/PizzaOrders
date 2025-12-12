using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Products; // Added this line
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Data;
using System;

namespace PizzaOrders.Application.Services;

public class CartService(AppDbContext dbContext, ILogger<CartService> logger, ICacheService cacheService) : ICartService
{
    public async Task<CartDto> GetCartAsync(Guid sessionId)
    {
        var key = $"cart:{sessionId}";

        var cart = await cacheService.GetAsync<CartDto>(key);

        return cart ?? new CartDto(sessionId);
    }

    public async Task AddToCartAsync(Guid sessionId, int productId, int quantity, List<int> toppingIds)
    {
        var cart = await GetCartAsync(sessionId);

        // Validate product
        var product = await dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with id {productId} not found.");
        }

        // Validate toppings
        var existingToppings = await dbContext.Toppings
            .Where(t => toppingIds.Contains(t.Id))
            .ToListAsync();

        if (existingToppings.Count != toppingIds.Count)
        {
            var invalidToppingIds = toppingIds.Except(existingToppings.Select(t => t.Id)).ToList();
            throw new InvalidOperationException($"Toppings with ids {string.Join(", ", invalidToppingIds)} not found.");
        }

        var existingCartItem = cart.Items.FirstOrDefault(item =>
            item.ProductId == productId &&
            item.Modifiers.ExtraToppings.Select(t => t.ToppingId).SequenceEqual(toppingIds.OrderBy(id => id)));

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
            existingCartItem.TotalPrice = (product.BasePrice + existingToppings.Sum(t => t.Price)) * existingCartItem.Quantity;
        }
        else
        {
            var newCartItem = new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                Modifiers = new ItemModifiers
                {
                    ExtraToppings = existingToppings.Select(t => new SelectedItemTopping { ToppingId = t.Id, Price = t.Price }).ToList()
                }
            };
            newCartItem.TotalPrice = (product.BasePrice + existingToppings.Sum(t => t.Price)) * newCartItem.Quantity;
            cart.Items.Add(newCartItem);
        }

        await cacheService.SetAsync($"cart:{sessionId}", cart);
    }
    
    public async Task RemoveFromCartAsync(Guid sessionId, int productId)
    {
        var cart = await GetCartAsync(sessionId);

        var itemToRemove = cart.Items.FirstOrDefault(item => item.ProductId == productId);
        if (itemToRemove != null)
        {
            cart.Items.Remove(itemToRemove);
            await cacheService.SetAsync($"cart:{sessionId}", cart);
        }
    }

    public async Task ClearCartAsync(Guid sessionId)
    {
        await cacheService.RemoveAsync($"cart:{sessionId}");
    }

    public async Task UpdateCartAsync(Guid sessionId, int productId, int quantity)
    {
        var cart = await GetCartAsync(sessionId);
        var itemToUpdate = cart.Items.FirstOrDefault(item => item.ProductId == productId);

        if (itemToUpdate != null)
        {
            if (quantity <= 0)
            {
                cart.Items.Remove(itemToUpdate);
            }
            else
            {
                itemToUpdate.Quantity = quantity;
            }
            await cacheService.SetAsync($"cart:{sessionId}", cart);
        }
    }
}
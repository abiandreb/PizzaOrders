using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Orders;
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

        var product = await dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with id {productId} not found.");
        }

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
            item.ToppingIds.OrderBy(id => id).SequenceEqual(toppingIds.OrderBy(id => id)));

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
            existingCartItem.TotalPrice = (product.BasePrice + existingToppings.Sum(t => t.Price)) * existingCartItem.Quantity;
        }
        else
        {
            var newCartItem = new CartItemDto
            {
                ProductId = productId,
                ProductName = product.Name,
                Quantity = quantity,
                BasePrice = product.BasePrice,
                ToppingIds = toppingIds,
                Toppings = existingToppings.Select(t => new CartToppingDto
                {
                    ToppingId = t.Id,
                    ToppingName = t.Name,
                    Price = t.Price
                }).ToList(),
                TotalPrice = (product.BasePrice + existingToppings.Sum(t => t.Price)) * quantity
            };
            cart.Items.Add(newCartItem);
        }

        cart.TotalPrice = cart.Items.Sum(i => i.TotalPrice);
        await cacheService.SetAsync($"cart:{sessionId}", cart);
    }
    
    public async Task RemoveFromCartAsync(Guid sessionId, int productId, List<int> toppingIds)
    {
        var cart = await GetCartAsync(sessionId);

        var itemToRemove = cart.Items.FirstOrDefault(item =>
            item.ProductId == productId &&
            item.ToppingIds.OrderBy(id => id).SequenceEqual(toppingIds.OrderBy(id => id)));
        if (itemToRemove != null)
        {
            cart.Items.Remove(itemToRemove);
            cart.TotalPrice = cart.Items.Sum(i => i.TotalPrice);
            await cacheService.SetAsync($"cart:{sessionId}", cart);
        }
    }

    public async Task ClearCartAsync(Guid sessionId)
    {
        await cacheService.RemoveAsync($"cart:{sessionId}");
    }

    public async Task UpdateCartAsync(Guid sessionId, int productId, int quantity, List<int> toppingIds)
    {
        var cart = await GetCartAsync(sessionId);
        var itemToUpdate = cart.Items.FirstOrDefault(item =>
            item.ProductId == productId &&
            item.ToppingIds.OrderBy(id => id).SequenceEqual(toppingIds.OrderBy(id => id)));

        if (itemToUpdate != null)
        {
            if (quantity <= 0)
            {
                cart.Items.Remove(itemToUpdate);
            }
            else
            {
                itemToUpdate.Quantity = quantity;

                var product = await dbContext.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with id {productId} not found.");
                }
                var existingToppings = await dbContext.Toppings
                    .Where(t => toppingIds.Contains(t.Id))
                    .ToListAsync();
                itemToUpdate.TotalPrice = (product.BasePrice + existingToppings.Sum(t => t.Price)) * itemToUpdate.Quantity;
            }

            cart.TotalPrice = cart.Items.Sum(i => i.TotalPrice);
            await cacheService.SetAsync($"cart:{sessionId}", cart);
        }
    }
}
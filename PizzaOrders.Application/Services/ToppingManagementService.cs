using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services
{
    public class ToppingManagementService(AppDbContext context) : IToppingManagementService
    {
        public async Task<IEnumerable<ToppingResponseDto>> GetAllToppingsAsync()
        {
            var toppings = await context.Toppings
                .OrderBy(t => t.Name)
                .Select(t => new ToppingResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    Price = t.Price,
                    Stock = t.Stock
                }).ToListAsync();
            
            return toppings;
        }
        
        public async Task<ToppingResponseDto> CreateToppingAsync(CreateToppingRequestDto request)
        {
            var topping = new ToppingEntity
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock
            };

            context.Toppings.Add(topping);
            await context.SaveChangesAsync();
            
            return new ToppingResponseDto
            {
                Id = topping.Id,
                Name = topping.Name,
                Description = topping.Description,
                Price = topping.Price,
                Stock = topping.Stock
            };
        }

        public async Task<ToppingResponseDto> UpdateToppingAsync(UpdateToppingRequestDto request)
        {
            var topping = await context.Toppings.FirstOrDefaultAsync(t => t.Id == request.Id);

            if (topping == null)
            {
                throw new System.InvalidOperationException("Topping not found.");
            }

            if (request.Name != null) topping.Name = request.Name;
            if (request.Description != null) topping.Description = request.Description;
            if (request.Price.HasValue) topping.Price = request.Price.Value;
            if (request.Stock.HasValue) topping.Stock = request.Stock.Value;

            await context.SaveChangesAsync();
            
            return new ToppingResponseDto
            {
                Id = topping.Id,
                Name = topping.Name,
                Description = topping.Description,
                Price = topping.Price,
                Stock = topping.Stock
            };
        }

        public async Task DeleteToppingAsync(int toppingId)
        {
            var topping = await context.Toppings.FirstOrDefaultAsync(t => t.Id == toppingId);

            if (topping == null)
            {
                throw new System.InvalidOperationException("Topping not found.");
            }

            context.Toppings.Remove(topping);
            await context.SaveChangesAsync();
        }
    }
}

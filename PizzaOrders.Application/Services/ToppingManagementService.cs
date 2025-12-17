using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services
{
    public class ToppingManagementService : IToppingManagementService
    {
        private readonly AppDbContext _context;

        public ToppingManagementService(AppDbContext context)
        {
            _context = context;
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

            _context.Toppings.Add(topping);
            await _context.SaveChangesAsync();
            
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
            var topping = await _context.Toppings.FirstOrDefaultAsync(t => t.Id == request.Id);

            if (topping == null)
            {
                throw new System.InvalidOperationException("Topping not found.");
            }

            if (request.Name != null) topping.Name = request.Name;
            if (request.Description != null) topping.Description = request.Description;
            if (request.Price.HasValue) topping.Price = request.Price.Value;
            if (request.Stock.HasValue) topping.Stock = request.Stock.Value;

            await _context.SaveChangesAsync();
            
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
            var topping = await _context.Toppings.FirstOrDefaultAsync(t => t.Id == toppingId);

            if (topping == null)
            {
                throw new System.InvalidOperationException("Topping not found.");
            }

            _context.Toppings.Remove(topping);
            await _context.SaveChangesAsync();
        }
    }
}

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly AppDbContext _context;

        public ProductManagementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductResponse> CreateProductAsync(CreateProductRequestDto request)
        {
            var product = new ProductEntity
            {
                Name = request.Name,
                Description = request.Description,
                BasePrice = request.BasePrice,
                HasToppings = request.HasToppings,
                ProductType = request.ProductType,
                ImageUrl = request.ImageUrl,
                ProductProperties = request.Properties
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                HasToppings = product.HasToppings,
                ProductType = product.ProductType,
                ImageUrl = product.ImageUrl,
                Properties = product.ProductProperties
            };
        }

        public async Task<ProductResponse> UpdateProductAsync(UpdateProductRequestDto request)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

            if (product == null)
            {
                throw new System.InvalidOperationException("Product not found.");
            }

            if (request.Name != null) product.Name = request.Name;
            if (request.Description != null) product.Description = request.Description;
            if (request.BasePrice.HasValue) product.BasePrice = request.BasePrice.Value;
            if (request.HasToppings.HasValue) product.HasToppings = request.HasToppings.Value;
            if (request.ProductType.HasValue) product.ProductType = request.ProductType.Value;
            if (request.ImageUrl != null) product.ImageUrl = request.ImageUrl;
            if (request.Properties != null) product.ProductProperties = request.Properties;

            await _context.SaveChangesAsync();
            
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                HasToppings = product.HasToppings,
                ProductType = product.ProductType,
                ImageUrl = product.ImageUrl,
                Properties = product.ProductProperties
            };
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new System.InvalidOperationException("Product not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
